using Chessed.Logic;

namespace Chessed
{
    public abstract class MoveProvider
    {
        public abstract Move RequestMove(GameState state);
    }
}