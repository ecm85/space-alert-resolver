using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.External.Minor.White;
using BLL.Threats.Internal;
using BLL.Threats.Internal.Minor.White;
using BLL.Threats.Internal.Serious.White;
using BLL.Threats.Internal.Serious.Yellow;
using BLL.Tracks;

namespace ConsoleResolver
{
	public class Program
	{
		private static void Main(string[] args)
		{
			if (!args.Any())
			{
				Console.WriteLine("Usage: ConsoleResolver");
				Console.WriteLine("-tracks blue:<int> white:<int> red:<int>");
				Console.WriteLine(
					"-threats [id:<string> time:<int> (optional)location:<red|white|blue> [extra-threat-id:<string>]? ]+");
				Console.WriteLine("-players <int> [player-index:<int> actions:<string>]+)");
			}
		}
	}
}
