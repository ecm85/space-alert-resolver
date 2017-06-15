using System.Collections.Generic;

namespace BLL.Players
{
    public static class PlayerFactory
    {
        public static Player CreatePlayer(IEnumerable<PlayerAction> actions, int index, PlayerColor color, PlayerSpecialization? specialization)
        {
            return new Player(actions, index, color, specialization);
        }
    }
}
