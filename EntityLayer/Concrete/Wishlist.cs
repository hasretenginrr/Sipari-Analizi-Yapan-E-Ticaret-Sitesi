using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

[Table("wishlist")]
public class Wishlist
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int? wishlist_id { get; set; }
    
    public int? user_id { get; set; }
    public int? product_id { get; set; }
    
    public Products Product { get; set; }
}