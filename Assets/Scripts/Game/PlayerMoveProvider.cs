using Chessed.Logic;

namespace Chessed
{
    public class PlayerMoveProvider : MoveProvider
    {
        public Move move;

        public override Move RequestMove(GameState state)
        {
            if (move == null) return null;
            
            Move tmpMove = move;
            move = null;
            return tmpMove;
        }
    }
}