using System.ComponentModel.DataAnnotations;


using System.ComponentModel.DataAnnotations.Schema;



[Table("products")] // PostgreSQL'deki doğru tablo adı
public class Products
{
    [Key]
    public int p_id { get; set; } // Primary Key
    public string name { get; set; }
    public string description { get; set; }
    public double price { get; set; }
    public string brand { get; set; }
    public string p_attributes { get; set; }
    public string? colour { get; set; }
    public double? avg_rating { get; set; }
    public double? ratingcount { get; set; }
    public string img { get; set; }
}
