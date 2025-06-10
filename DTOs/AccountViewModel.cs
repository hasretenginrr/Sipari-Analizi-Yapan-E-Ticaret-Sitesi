namespace BitirmeProjesi.DTOs
{
    public class AccountViewModel
    {
        public Users CurrentUser { get; set; }
        public List<Users> Users { get; set; }
        public List <Orders> Orders { get; set; }

        public List<Order_Details> Order_Details { get; set; }

        public List<Products> Products { get; set; }

    }
}
