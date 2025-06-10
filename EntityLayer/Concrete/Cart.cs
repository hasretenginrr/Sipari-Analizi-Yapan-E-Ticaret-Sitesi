using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("cart")]
public class Cart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int cart_id { get; set; } 

    public int? user_id { get; set; } 
    public int? product_id { get; set; } 
    public int? quantity { get; set; } 
    
    public Products Product { get; set; }
}