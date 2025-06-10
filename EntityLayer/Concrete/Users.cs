using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;



[Table("users")] // PostgreSQL'deki doğru tablo adı
public class Users
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public double? user_id { get; set; } // Primary Key
    public string? user_name { get; set; }
    public string? email { get; set; }
    public string? password { get; set; }
    public string? Adress { get; set; }


    [Column(TypeName = "VARCHAR(10)")]
    [MaxLength(10)]
    public string? gender { get; set; }

}
