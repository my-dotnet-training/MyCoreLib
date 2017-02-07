

namespace MyCoreLib.BaseWeb.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    /// <summary>
    /// 
    /// </summary>
    [AllowAnonymous]
    public class IOController : BaseController
    {
        [HttpPost]
        public ActionResult Download()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            return View();
        }
    }
}
