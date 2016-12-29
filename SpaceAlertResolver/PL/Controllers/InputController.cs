using System.Web.Mvc;

namespace PL.Controllers
{
	public class InputController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult ActionsDialog()
		{
			return PartialView();
		}
	}
}
