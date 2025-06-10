using Microsoft.AspNetCore.Mvc;

namespace proje3
{
    // ProductRecommendation sınıfı: Modelden tahmin edilen ürün önerilerini temsil eder
    public class ProductRecommendation
    {
        public int ProductId { get; set; }
        public float Score { get; set; }

        
    }
    
}

