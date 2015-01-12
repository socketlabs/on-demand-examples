using System.Web.Mvc;

namespace SocketLabs.NotificationApi.TestEndpoint.Mvc5.Controllers
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