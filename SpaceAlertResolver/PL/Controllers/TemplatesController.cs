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

		public ActionResult ThreatTrack()
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
	}
}