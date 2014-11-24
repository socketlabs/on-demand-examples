using System.Web.Mvc;

namespace NotificationApiEndpoint.Controllers
{
    public class HomeController : Controller
    {
        // GET /
        [HttpGet, ActionName("index")]
        public ActionResult GetEvents()
        {
            return View("Index");
        }
    }
}