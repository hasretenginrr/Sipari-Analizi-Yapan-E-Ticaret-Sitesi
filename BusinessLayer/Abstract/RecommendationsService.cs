/*using System.Net.Http;
using System.Threading.Tasks;
using BitirmeProjesi.DTO;
using Newtonsoft.Json;

namespace BitirmeProjesi.Services
{
    public class RecommendationService
    {
        private readonly HttpClient _httpClient;

        public RecommendationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<RecommendResponse> GetRecommendationsAsync(int userId)
        {
            string apiUrl = $"http://localhost:5062/recommend/{userId}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    var recommendResponse = JsonConvert.DeserializeObject<RecommendResponse>(jsonData, new JsonSerializerSettings
                    {
                        FloatParseHandling = FloatParseHandling.Double,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        Error = (sender, args) =>
                        {
                            Console.WriteLine("Deserialize hatası: " + args.ErrorContext.Error.Message);
                            args.ErrorContext.Handled = true;
                        }
                    });

                    if (recommendResponse != null)
                        recommendResponse.UserId = userId;

                    return recommendResponse;
                }

                else
                {
                    Console.WriteLine("API çağrısı başarısız: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("API çağrısı hatası: " + ex.Message);
            }

            return new RecommendResponse
            {
                Recommendations = new List<ProductDto>()
            };
        }

    }
}
*/