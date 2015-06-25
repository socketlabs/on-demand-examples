using System.Web.Mvc;

namespace NotificationApi.TestEndpoint.Mvc4.Controllers
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