using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL;
using PL.Models;

namespace PL.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult ProcessGame()
		{
			var args = new[]
			{
				"-players",
				"player-index:0",
				"actions:-^b^aaaaaa",
				"player-index:1",
				"actions:<^c^-cxcxa",
				"player-index:2",
				"actions:-c--c^c",
				"player-index:3",
				"actions:^-----c",
				"player-index:4",
				"actions:-^----c",
				"player-index:5",
				"actions:3425",
				"-external-tracks",
				"blue:1",
				"red:2",
				"white:3",
				"-internal-track",
				"4",
				"-external-threats",
				"id:E1-02",
				"time:4",
				"location:blue",
				"id:E1-07",
				"time:5",
				"location:red",
				"id:E1-07",
				"time:6",
				"location:white",
				"-internal-threats",
				"id:I1-01",
				"time:4",
				"id:SI1-04",
				"time:2",
				"id:SI2-05",
				"time:6"
			};
			var game = GameParser.ParseArgsIntoGame(args);
			var models = new List<GameSnapshotModel> {new GameSnapshotModel(game, "Start of Game")};
			game.NewThreatsAdded += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Threats Appeared"));
			game.PlayerActionsPerformed += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Player Action Finished"));
			game.ResolvedDamage += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Resolved Damage"));
			game.ThreatsMoved += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Threats Moved"));
			game.CheckedForComputer += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Checked Computer Maintenance"));
			game.TurnEnding += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "End of Turn"));
			for (var i = 0; i < game.NumberOfTurns; i++)
			{
				game.PerformTurn();
			}
			return View(models.GroupBy(model => model.Turn).ToList());
		}
	}
}