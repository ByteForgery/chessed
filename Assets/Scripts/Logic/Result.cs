namespace Chessed.Logic
{
    public class Result
    {
        public Side Winner { get; }
        public EndReason Reason { get; }

        public Result(Side winner, EndReason reason)
        {
            Winner = winner;
            Reason = reason;
        }

        public static Result Win(Side winner) => new(winner, EndReason.Checkmate);

        public static Result Draw(EndReason reason) => new(Side.None, reason);
    }
}