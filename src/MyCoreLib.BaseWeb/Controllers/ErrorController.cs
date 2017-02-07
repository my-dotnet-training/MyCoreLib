
namespace MyCoreLib.BaseWeb.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The shared controller to send out friendly error pages.
    /// </summary>
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [ActionName("500")]
        public ActionResult UnkownError()
        {
            return View();
        }

        [ActionName("404")]
        public ActionResult PageNotFound()
        {
            return View();
        }

        [ActionName("401")]
        public ActionResult Unauthroized()
        {
            return View();
        }
    }
}
