public class FeatureVectorCache
{
    // Ürün id'sine göre description ve name vektörlerini saklayalım
    public Dictionary<int, float[]> DescriptionVectors { get; private set; } = new();
    public Dictionary<int, float[]> NameVectors { get; private set; } = new();

    // Vektörler hazır mı?
    public bool IsCached { get; private set; } = false;

    public void SetCache(List<Products> products, List<float[]> descriptionVectors, List<float[]> nameVectors)
    {
        DescriptionVectors = new Dictionary<int, float[]>();
        NameVectors = new Dictionary<int, float[]>();

        for (int i = 0; i < products.Count; i++)
        {
            DescriptionVectors[products[i].p_id] = descriptionVectors[i];
            NameVectors[products[i].p_id] = nameVectors[i];
        }

        IsCached = true;
    }
}
