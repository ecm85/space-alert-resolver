using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL;
using PL.Models;

namespace PL.Controllers
{
	public class ResolutionController : Controller
	{
		public ActionResult Index()
		{
			var args = new[]
			{
				"-players",
				"player-index:0",
				"actions:-^b^aaaaaa",
				"player-color: blue",
				"player-index:1",
				"actions:<^c^-cxcxa",
				"player-color: green",
				"player-index:2",
				"actions:-c--c^c",
				"player-color: red",
				"player-index:3",
				"actions:^-----c>aa-a",
				"player-color: yellow",
				"player-index:4",
				"actions:-^----c",
				"player-color: purple",
				"-external-tracks",
				"blue:7",
				"red:2",
				"white:3",
				"-internal-track",
				"4",
				"-external-threats",
				//"id:E1-02",
				//"time:4",
				//"location:blue",
				"id:E1-07",
				"time:5",
				"location:red",
				"id:E1-07",
				"time:6",
				"location:white",
				"-internal-threats",
				"id:I1-01",
				"time:4",
				//"id:SI1-04",
				//"time:2",
				"id:SI2-05",
				"time:6",
				//"id:I1-06",
				//"time:6"
			};
			var game = GameParser.ParseArgsIntoGame(args);
			var nextPhase = 0;
			var models = new List<GameSnapshotModel> {new GameSnapshotModel(game, "Start of Game", () => nextPhase++)};
			game.NewThreatsAdded += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Threats Appeared", () => nextPhase++));
			game.PlayerActionsPerformed += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Player Action Finished", () => nextPhase++));
			game.ResolvedDamage += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Resolved Damage", () => nextPhase++));
			game.ThreatsMoved += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Threats Moved", () => nextPhase++));
			game.CheckedForComputer += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Checked Computer Maintenance", () => nextPhase++));
			game.TurnEnding += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "End of Turn", () => nextPhase++));
			for (var i = 0; i < game.NumberOfTurns; i++)
			{
				game.PerformTurn();
				nextPhase = 0;
			}
			var modelsString = JavaScriptConvert.SerializeObject(models.GroupBy(model => model.Turn).ToList());
			return View(modelsString);
		}

		public ActionResult StandardZone()
		{
			return PartialView();
		}

		public ActionResult ThreatTrack()
		{
			return PartialView();
		}
	}
}
