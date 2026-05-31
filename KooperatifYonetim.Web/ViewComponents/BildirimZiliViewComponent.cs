using KooperatifYonetim.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KooperatifYonetim.Web.ViewComponents
{
    public class BildirimZiliViewComponent : ViewComponent
    {
        private readonly IBildirimService _bildirimService;
        public BildirimZiliViewComponent(IBildirimService bildirimService) => _bildirimService = bildirimService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Content(string.Empty);
            var count = await _bildirimService.GetOkunmamisSayiAsync(userId);
            return View(count);
        }
    }
}
