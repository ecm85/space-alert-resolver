using System.Web.Mvc;

namespace PL.Controllers
{
	public class SpaceAlertController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}
	}
}
