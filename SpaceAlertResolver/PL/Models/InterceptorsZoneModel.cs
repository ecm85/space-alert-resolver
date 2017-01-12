using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class InterceptorsZoneModel
	{
		public InterceptorsStationModel InterceptorsStation1 { get; }
		public InterceptorsStationModel InterceptorsStation2 { get; }
		public InterceptorsStationModel InterceptorsStation3 { get; }

		public InterceptorsZoneModel(Game game)
		{
			InterceptorsStation1 = new InterceptorsStationModel(game, StationLocation.Interceptor1);
			InterceptorsStation2 = new InterceptorsStationModel(game, StationLocation.Interceptor2);
			InterceptorsStation3 = new InterceptorsStationModel(game, StationLocation.Interceptor3);
		}
	}
}
