using System;
using System.Collections.Generic;

namespace Chessed.Logic
{
    public class PieceCounter
    {
        private readonly Dictionary<PieceType, int> whiteCount = new();
        private readonly Dictionary<PieceType, int> blackCount = new();
        
        public int TotalCount { get; private set; }

        public PieceCounter()
        {
            foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
            {
                whiteCount[type] = 0;
                blackCount[type] = 0;
            }
        }

        public void Increment(Side side, PieceType type)
        {
            if (side == Side.White)
                whiteCount[type]++;
            else if (side == Side.Black)
                blackCount[type]++;

            TotalCount++;
        }

        public int White(PieceType type) => whiteCount[type];
        public int Black(PieceType type) => blackCount[type];
    }
}