namespace Chessed.Logic
{
    public enum Side
    {
        None,
        White,
        Black
    }

    public static class PlayerExtensions
    {
        public static Side Opponent(this Side side) => side switch
        {
            Side.White => Side.Black,
            Side.Black => Side.White,
            _ => Side.None
        };
    }
}
