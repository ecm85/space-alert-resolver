using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Players;

namespace BLL.ShipComponents
{
    public class RocketsComponent : ICharlieComponent
    {
        private List<Rocket> Rockets { get; } 
        public Rocket RocketFiredThisTurn { get; private set; }
        public Rocket RocketFiredLastTurn { get; private set; }

        public int RocketCount => Rockets.Count;

        public event EventHandler<RocketsRemovedEventArgs> RocketsModified = (sender, eventArgs) => { };

        internal RocketsComponent()
        {
            Rockets = Enumerable.Range(0, 3).Select(index => new Rocket()).ToList();
        }

        public void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
        {
            if (CanPerformCAction(performingPlayer))
            {
                var canFireDoubleRocket = RocketCount > 1;
                var firedRocket = Rockets.First();
                Rockets.Remove(firedRocket);
                RocketFiredThisTurn = firedRocket;
                var isFiringDoubleRocket = isAdvancedUsage && canFireDoubleRocket;
                if (isFiringDoubleRocket)
                {
                    Rockets.Remove(Rockets.First());
                    firedRocket.SetDoubleRocket();
                }
                RocketsModified(this, new RocketsRemovedEventArgs {RocketsRemovedCount = isFiringDoubleRocket ? 2 : 1});
            }
        }

        public bool CanPerformCAction(Player performingPlayer)
        {
            return RocketFiredThisTurn == null && Rockets.Any();
        }

        public void PerformEndOfTurn()
        {
            RocketFiredLastTurn = RocketFiredThisTurn;
            RocketFiredThisTurn = null;
        }

        public void RemoveRocket()
        {
            Rockets.Remove(Rockets.First());
            RocketsModified(this, new RocketsRemovedEventArgs {RocketsRemovedCount = 1});
        }

        public int RemoveAllRockets()
        {
            var rocketCountRemoved = Rockets.Count;
            Rockets.Clear();
            RocketsModified(this, new RocketsRemovedEventArgs {RocketsRemovedCount = rocketCountRemoved});
            return rocketCountRemoved;
        }
    }
}
