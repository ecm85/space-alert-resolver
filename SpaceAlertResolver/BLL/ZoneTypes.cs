using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public static class ZoneTypes
	{
		public static ZoneType[] All()
		{
			return new[] {ZoneType.Blue, ZoneType.White, ZoneType.Red};
		}
	}
}
