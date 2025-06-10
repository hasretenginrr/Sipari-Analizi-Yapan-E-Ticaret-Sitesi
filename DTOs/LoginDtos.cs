using System.ComponentModel.DataAnnotations.Schema;

namespace BitirmeProjesi.DTOs
{
    public class LoginDtos
    {

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password {    get;set; }
    }
}
