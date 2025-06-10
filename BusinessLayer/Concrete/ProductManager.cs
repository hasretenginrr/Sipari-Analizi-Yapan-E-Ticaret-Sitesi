using BitirmeProjesi.BusinessLayer.Abstract;
using BitirmeProjesi.DataAccessLayer.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace BitirmeProjesi.BusinessLayer.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public void TAdd(Products t)
        {
            _productDal.Insert(t);
        }

        public void TDelete(Products t)
        {
            _productDal.Delete(t);
        }

        public Products TGetByID(int id)
        {
            return _productDal.GetById(id);
        }

        public List<Products> TGetList()
        {
            return _productDal.GetList();
        }


        public void TUpdate(Products t)
        {
            _productDal.Update(t);
        }

        public List<string> GetCategoriesFromProducts()
        {
            var products = _productDal.GetList(); // Tüm ürünleri getir

            // Ürün isimlerinde aranacak kategori kelimeleri
            var categoryKeywords = new List<string> { "Yelek", "Ceket", "Tulum", "Pantolon", "Etek", "Elbise", "Şort", "Dupatta", "Kurta", "Üst", "Bluz", "Şal","Şapka","Sweatshirt", "Shrug","Kazak", "Tunik","Crop" };

            // Ürün isimlerinde kategori kelimelerini bul ve tekrarlamayan bir liste oluştur
            var categories = products
                .SelectMany(p => categoryKeywords.Where(keyword => p.name.Contains(keyword)))
                .Distinct()
                .ToList();

            return categories;
        }
    }
}
