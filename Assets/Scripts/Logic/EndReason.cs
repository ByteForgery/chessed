namespace Chessed.Logic
{
    public enum EndReason
    {
        Checkmate,
        Timeout,
        Stalemate,
        FiftyMoveRule,
        InsufficientMaterial,
        ThreefoldRepetition
    }

    public static class EndReasonExtensions
    {
        public static string DisplayName(this EndReason reason) => reason switch
        {
            EndReason.Checkmate => "Checkmate",
            EndReason.Timeout => "Timeout",
            EndReason.Stalemate => "Stalemate",
            EndReason.FiftyMoveRule => "50-Move Rule",
            EndReason.InsufficientMaterial => "Insufficient Material",
            EndReason.ThreefoldRepetition => "Threefold Repetition",
            _ => null
        };
    }
}