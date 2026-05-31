using KooperatifYonetim.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KooperatifYonetim.Web.ViewComponents
{
    public class MesajZiliViewComponent : ViewComponent
    {
        private readonly IMesajService _mesajService;
        public MesajZiliViewComponent(IMesajService mesajService) => _mesajService = mesajService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Content(string.Empty);
            var count = await _mesajService.GetOkunmamisSayiAsync(userId);
            return View(count);
        }
    }
}
