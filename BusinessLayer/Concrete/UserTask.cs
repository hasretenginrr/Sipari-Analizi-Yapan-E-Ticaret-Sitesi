using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

class UserTask
{
    static async Task Main(string[] args)
    {
        // Kullanıcıdan veri al
        Console.WriteLine("Kullanıcı adı girin:");
        string userName = Console.ReadLine();

        Console.WriteLine("E-posta adresini girin:");
        string email = Console.ReadLine();

        Console.WriteLine("Cinsiyet girin:");
        string gender = Console.ReadLine();

        Console.WriteLine("Şifreyi girin (En az 5 karakter):");
        string password = Console.ReadLine();

        var user = new Users
        {
            user_name = userName,
            email = email,
            gender = gender,
            password = password
        };

        
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:5147/"); 
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("adduser", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Kullanıcı başarıyla eklendi.");
            }
            else
            {
                Console.WriteLine("Bir hata oluştu: " + response.ReasonPhrase);
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Hata Detayı: " + responseContent);
            }
        }
    }
}