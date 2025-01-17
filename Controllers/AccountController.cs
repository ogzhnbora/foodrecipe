using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly FirebaseService _firebaseService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(FirebaseService firebaseService, ILogger<AccountController> logger)
    {
        _firebaseService = firebaseService;
        _logger = logger;
    }

    // Giriş Yap Sayfasını Göster
    [HttpGet]
    public IActionResult Login()
    {
        _logger.LogInformation("Login sayfası görüntülendi.");
        return View();
    }

    // Giriş İşlemi
    [HttpPost]
public async Task<IActionResult> Login(string email, string password)
{
    _logger.LogInformation("Login işlemi başlatıldı.");

    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
    {
        _logger.LogWarning("Login sırasında eksik bilgi gönderildi.");
        ModelState.AddModelError("", "Email ve şifre gereklidir.");
        return View();
    }

    try
    {
        var user = await _firebaseService.GetUserByEmailAsync(email);

        if (user != null)
        {
            // DisplayName null ise default bir değer atayın
            var displayName = string.IsNullOrEmpty(user.DisplayName) ? "Kullanıcı" : user.DisplayName;

            // Kullanıcı adını Session'da sakla
            HttpContext.Session.SetString("UserName", displayName);

            _logger.LogInformation($"Kullanıcı giriş yaptı: {displayName}");
            return RedirectToAction("Index", "Home");
        }
        else
        {
            _logger.LogWarning($"Giriş başarısız. Kullanıcı bulunamadı: {email}");
            ModelState.AddModelError("", "Giriş başarısız.");
            return View();
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Login işlemi sırasında bir hata oluştu.");
        ModelState.AddModelError("", "Bir hata oluştu.");
        return View();  
    }
}
    // Kayıt Ol Sayfasını Göster
    [HttpGet]
    public IActionResult Register()
    {
        _logger.LogInformation("Register sayfası görüntülendi.");
        return View();
    }

    // Kayıt Ol İşlemi
    [HttpPost]
    public async Task<IActionResult> Register(string email, string password, string firstName, string lastName)
    {
        _logger.LogInformation("Register işlemi başlatıldı.");

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || 
            string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            _logger.LogWarning("Register sırasında eksik bilgi gönderildi.");
            ModelState.AddModelError("", "Tüm alanlar doldurulmalıdır.");
            return View();
        }

        try
        {
            // Kullanıcıyı Firebase'e kaydet
            var userId = await _firebaseService.RegisterUserAsync(email, password);

            // Kullanıcının adını ve soyadını Firebase'e eklemek için
            await _firebaseService.UpdateUserProfileAsync(userId, firstName, lastName);

            _logger.LogInformation($"Kullanıcı başarıyla kaydedildi: {email}");
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Register işlemi sırasında bir hata oluştu.");
            ModelState.AddModelError("", $"Kayıt sırasında bir hata oluştu: {ex.Message}");
            return View();
        }
    }
    [HttpPost]
public IActionResult Logout()
{
    // Session temizleme
    HttpContext.Session.Clear();
    _logger.LogInformation("Kullanıcı oturumu kapattı.");
    return RedirectToAction("Login");
}

}
