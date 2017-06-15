using System.Collections.Generic;
using PL.Models;

namespace PL
{
    public class GamePhaseModel
    {
        public string Description { get; set; }
        public IList<GameSnapshotModel> SubPhases { get; } = new List<GameSnapshotModel>();
    }
}
