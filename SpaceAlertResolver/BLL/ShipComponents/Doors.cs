namespace BLL.ShipComponents
{
    public class Doors
    {
        public bool CanUse { get { return !Sealed; }}
        public bool Sealed { get; set; }
    }
}
