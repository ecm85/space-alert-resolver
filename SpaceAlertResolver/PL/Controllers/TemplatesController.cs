using System.Web.Mvc;

namespace PL.Controllers
{
	public class TemplatesController : Controller
	{
		public ActionResult ActionsDialog()
		{
			return PartialView();
		}

		public ActionResult StandardZone()
		{
			return PartialView();
		}

		public ActionResult TrackWithThreats()
		{
			return PartialView();
		}

		public ActionResult TrackSpaces()
		{
			return PartialView();
		}

		public ActionResult TrackDialog()
		{
			return PartialView();
		}

		public ActionResult ThreatsDialog()
		{
			return PartialView();
		}

		public ActionResult Threat()
		{
			return PartialView();
		}
	}
}