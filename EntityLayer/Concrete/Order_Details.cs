using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



[Table("order_details")] // PostgreSQL'deki doğru tablo adı
public class Order_Details
{
    [Key]
    public int id { get; set; } // Primary Key
    public int? order_id { get; set; }
    public int? product_id { get; set; }
    public int? quantity { get; set; }

    public Orders Order { get; set; } // 'orders' tablosu ile ilişki


}
