using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YemekTarifiSitesi.Services;
using YemekTarifiSitesi.Models;

namespace YemekTarifiSitesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly MealService _mealService;

        public HomeController(MealService mealService)
        {
            _mealService = mealService;
        }

         public async Task<IActionResult> Index()
        {
            var mealsResponse = await _mealService.GetMealsAsync();
            var categoriesResponse = await _mealService.GetCategoriesAsync();
            
            ViewBag.Categories = categoriesResponse.categories; // Kategorileri View'e gönder
            return View(mealsResponse);
        }

        public async Task<IActionResult> Category(string category)
        {
            var mealsResponse = await _mealService.GetMealsAsync();
            var filteredMeals = mealsResponse.meals
                .Where(m => m.strCategory != null && m.strCategory.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();

            ViewBag.Categories = (await _mealService.GetCategoriesAsync()).categories; // Kategorileri yeniden yükle
            return View("Index", new MealResponse { meals = filteredMeals });
        }
public async Task<IActionResult> Search(string query)
{
    ViewData["Query"] = query; // Arama sorgusunu View'e gönder

    if (string.IsNullOrWhiteSpace(query))
    {
        return RedirectToAction("Index");
    }

    // API'den yemek tariflerini çek
    var mealsResponse = await _mealService.GetMealsAsync();

    // Filtreleme işlemi
    var filteredMeals = mealsResponse.meals
        .Where(m => m.strMeal != null && m.strMeal.Contains(query, StringComparison.OrdinalIgnoreCase))
        .ToList();

    // Kategorileri yeniden yükle ve ViewBag'e ata
    var categoriesResponse = await _mealService.GetCategoriesAsync();
    ViewBag.Categories = categoriesResponse.categories;

    // Filtrelenmiş sonuçları MealResponse modeline koy
    var result = new MealResponse
    {
        meals = filteredMeals
    };

    return View("Index", result); // Doğru tipte model gönderimi
}


    
      public async Task<IActionResult> Detail(string id)
{
    if (string.IsNullOrWhiteSpace(id))
    {
        return NotFound("Yemek ID'si geçersiz.");
    }

    // API'den tarifleri çek
    var mealsResponse = await _mealService.GetMealsAsync();

    // ID'ye göre yemek bulma
    var meal = mealsResponse.meals.FirstOrDefault(m => m.idMeal == id);

    if (meal == null)
    {
        return NotFound("Yemek bulunamadı.");
    }

    // Malzemeleri bir liste olarak döndür
    var ingredients = new List<string>();
    for (int i = 1; i <= 20; i++) // 20 malzemeye kadar
    {
        var ingredient = meal.GetType().GetProperty($"strIngredient{i}")?.GetValue(meal, null)?.ToString();
        var measure = meal.GetType().GetProperty($"strMeasure{i}")?.GetValue(meal, null)?.ToString();

        if (!string.IsNullOrEmpty(ingredient) && !string.IsNullOrEmpty(measure))
        {
            ingredients.Add($"{measure} {ingredient}");
        }
    }

    ViewBag.Ingredients = ingredients; // Malzemeleri View'e gönder
    return View(meal);
}

    }
}
