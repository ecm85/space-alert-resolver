namespace API.Models
{
    public class GamePhaseModel
    {
        public string Description { get; set; }
        public IList<GameSnapshotModel> SubPhases { get; } = new List<GameSnapshotModel>();
    }
}
