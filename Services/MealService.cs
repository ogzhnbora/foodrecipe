using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YemekTarifiSitesi.Models;

namespace YemekTarifiSitesi.Services
{
    public class MealService
    {
        private readonly HttpClient _httpClient;

        public MealService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<MealResponse> GetMealsAsync()
        {
            var response = await _httpClient.GetAsync("https://www.themealdb.com/api/json/v1/1/search.php?s=");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MealResponse>(jsonString);
        }
            public async Task<CategoryResponse> GetCategoriesAsync()
{
    var response = await _httpClient.GetAsync("https://www.themealdb.com/api/json/v1/1/categories.php");
    response.EnsureSuccessStatusCode();

    var jsonString = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<CategoryResponse>(jsonString);
}
    
    }


}
