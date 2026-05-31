using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici,Uretici")]
    public class BesiGirisController : Controller
    {
        public IActionResult Index() => RedirectToAction("Index", "BesiStok");
        public IActionResult Create() => RedirectToAction("StokEkle", "BesiStok");
    }
}
