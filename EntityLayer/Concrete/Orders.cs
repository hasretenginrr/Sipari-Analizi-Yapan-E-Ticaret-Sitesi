using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;



[Table("orders")] // PostgreSQL'deki doğru tablo adı
public class Orders
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? id { get; set; } // Primary Key
    public double? user_id { get; set; }

    public DateTime order_date { get; set; } = DateTime.UtcNow;

    public ICollection<Order_Details> Order_Details { get; set; }

}
