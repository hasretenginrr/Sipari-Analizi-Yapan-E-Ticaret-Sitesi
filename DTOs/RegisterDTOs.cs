using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitirmeProjesi.DTOs
{
    public class RegisterDTOs
    {

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("gender")]
        [MaxLength(10)]
        public string Gender { get; set; }





    }
}
