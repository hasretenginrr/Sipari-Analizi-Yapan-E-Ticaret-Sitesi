using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class DataPreparation
{
    private readonly AppDbContext _context;
    private readonly FeatureVectorCache _cache;

    private const string DescriptionModelPath = "Models/DescriptionModel.zip";
    private const string NameModelPath = "Models/NameModel.zip";

    public DataPreparation(AppDbContext context, FeatureVectorCache cache)
    {
        _context = context;
        _cache = cache;
    }

    private ITransformer TrainOrLoadTextModel(MLContext mlContext, List<string> texts, string modelPath)
    {
        if (File.Exists(modelPath))
        {
            return mlContext.Model.Load(modelPath, out _);
        }

        var data = texts.Select(t => new TextData { Text = t }).ToList();
        var dataView = mlContext.Data.LoadFromEnumerable(data);

        var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(TextData.Text));
        var model = pipeline.Fit(dataView);

        Directory.CreateDirectory(Path.GetDirectoryName(modelPath));
        mlContext.Model.Save(model, dataView.Schema, modelPath);
        return model;
    }

    private List<float[]> ExtractFeatureVectors(MLContext mlContext, IEnumerable<string> texts, ITransformer model)
    {
        var engine = mlContext.Model.CreatePredictionEngine<TextData, TextFeatures>(model);
        var vectors = new List<float[]>();
        foreach (var text in texts)
        {
            var prediction = engine.Predict(new TextData { Text = text ?? "" });
            vectors.Add(prediction.Features);
        }
        return vectors;
    }

    private double CosineSimilarity(float[] vecA, float[] vecB)
    {
        double dot = 0, magA = 0, magB = 0;
        for (int i = 0; i < vecA.Length; i++)
        {
            dot += vecA[i] * vecB[i];
            magA += vecA[i] * vecA[i];
            magB += vecB[i] * vecB[i];
        }
        if (magA == 0 || magB == 0) return 0;
        return dot / (Math.Sqrt(magA) * Math.Sqrt(magB));
    }

    public List<Products> GetSimilarProductsByContent(int userId, List<Products> allProducts, MLContext mlContext)
    {
        if (!_cache.IsCached)
        {
            var cleanDescriptions = allProducts.Select(p => p.description).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
            var cleanNames = allProducts.Select(p => p.name).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

            var descriptionTransformer = TrainOrLoadTextModel(mlContext, cleanDescriptions, DescriptionModelPath);
            var nameTransformer = TrainOrLoadTextModel(mlContext, cleanNames, NameModelPath);

            var descriptionVectors = ExtractFeatureVectors(mlContext, allProducts.Select(p => p.description ?? ""), descriptionTransformer);
            var nameVectors = ExtractFeatureVectors(mlContext, allProducts.Select(p => p.name ?? ""), nameTransformer);

            _cache.SetCache(allProducts, descriptionVectors, nameVectors);
        }

        var purchasedProductIds = _context.Orders
            .Where(o => o.user_id == userId)
            .Join(_context.Order_Details,
                  order => order.id,
                  detail => detail.order_id,
                  (order, detail) => detail.product_id)
            .Distinct()
            .ToList();

        var purchasedProducts = allProducts.Where(p => purchasedProductIds.Contains(p.p_id)).ToList();

        if (!purchasedProducts.Any())
        {
            Console.WriteLine($"[INFO] Kullanıcı ({userId}) için satın alınan ürün bulunamadı.");
            return new List<Products>();
        }

        var recommendedProducts = new List<Products>();
        object lockObject = new object();

        foreach (var purchased in purchasedProducts)
        {
            var scores = new List<(Products Product, double Score)>();

            Parallel.ForEach(allProducts, product =>
            {
                if (product.p_id == purchased.p_id || purchasedProducts.Any(p => p.p_id == product.p_id))
                    return;

                var descVecPurchased = _cache.DescriptionVectors[purchased.p_id];
                var nameVecPurchased = _cache.NameVectors[purchased.p_id];

                var descVecProduct = _cache.DescriptionVectors[product.p_id];
                var nameVecProduct = _cache.NameVectors[product.p_id];

                double descSim = CosineSimilarity(descVecPurchased, descVecProduct);
                double nameSim = CosineSimilarity(nameVecPurchased, nameVecProduct);
                double colorSim = (purchased.colour ?? "") == (product.colour ?? "") ? 1.0 : 0.0;

                double score = (0.4 * descSim) + (0.4 * nameSim) + (0.2 * colorSim);

                lock (lockObject)
                {
                    scores.Add((product, score));
                }
            });

            var topN = scores.OrderByDescending(x => x.Score).Take(2).Select(x => x.Product).ToList(); // Her ürün için 2 öneri
            recommendedProducts.AddRange(topN);
        }

        // Aynı ürün birden fazla kez eklendiyse tekrarı kaldır
        var uniqueRecommendedProducts = recommendedProducts
            .GroupBy(p => p.p_id)
            .Select(g => g.First())
            .ToList();

        return uniqueRecommendedProducts;
    }


    private class TextData
    {
        public string Text { get; set; }
    }

    private class TextFeatures
    {
        [VectorType]
        public float[] Features { get; set; }
    }
}