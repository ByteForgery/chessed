using Chessed.Logic;

namespace Chessed
{
    public class AIMoveProvider : MoveProvider
    {
        private readonly StockfishController stockfish;

        public AIMoveProvider(StockfishController stockfish) => this.stockfish = stockfish;

        public override Move RequestMove(GameState state)
        {
            MoveSquares bestMoveSquares = stockfish.BestMoveSquares;
            if (bestMoveSquares == null) return null;
            
            foreach (Move move in state.LegalMovesForPiece(bestMoveSquares.From))
            {
                if (move.To == bestMoveSquares.To)
                    return move;
            }

            return null;
        }
    }
}