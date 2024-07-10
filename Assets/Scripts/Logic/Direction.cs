using System;
using System.Linq;
using UnityEngine;

namespace Chessed.Logic
{
    public static class Direction
    {
        public static readonly Vector2Int NORTH = new Vector2Int(0, -1);
        public static readonly Vector2Int SOUTH = new Vector2Int(0, 1);
        public static readonly Vector2Int EAST = new Vector2Int(1, 0);
        public static readonly Vector2Int WEST = new Vector2Int(-1, 0);
        public static readonly Vector2Int NORTH_EAST = NORTH + EAST;
        public static readonly Vector2Int NORTH_WEST = NORTH + WEST;
        public static readonly Vector2Int SOUTH_EAST = SOUTH + EAST;
        public static readonly Vector2Int SOUTH_WEST = SOUTH + WEST;

        public static readonly Vector2Int[] CARDINALS = { NORTH, EAST, SOUTH, WEST };
        public static readonly Vector2Int[] ORDINALS = { NORTH_EAST, NORTH_WEST, SOUTH_EAST, SOUTH_WEST };
        public static readonly Vector2Int[] PRINCIPALS = CARDINALS.Concat(ORDINALS).ToArray();
    }
}