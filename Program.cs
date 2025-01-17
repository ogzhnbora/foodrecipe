var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<FirebaseService>();

// MealService'i HttpClient ile ekle
builder.Services.AddHttpClient<YemekTarifiSitesi.Services.MealService>(client =>
{
    client.BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/");
});

// Diğer servisleri ekleyin
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
