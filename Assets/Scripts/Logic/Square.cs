using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chessed.Logic
{
    public class Square
    {
        public Vector2Int Position { get; }

        public int X
        {
            get => Position.x;
            set => Position.Set(value, Position.y);
        }

        public int Y
        {
            get => Position.y;
            set => Position.Set(Position.x, value);
        }

        public Square(Vector2Int position) => Position = position;
        public Square(int x, int y) : this(new(x, y)) {}

        public bool IsValid => (X is >= 0 and < 8) && (Y is >= 0 and < 8);

        public Side Color => ((X + Y) % 2 == 0) ? Side.White : Side.Black;

        public override bool Equals(object obj) => obj is Square square &&
                                                   X == square.X &&
                                                   Y == square.Y;

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public static bool operator ==(Square left, Square right) =>
            EqualityComparer<Square>.Default.Equals(left, right);

        public static bool operator !=(Square left, Square right) => !(left == right);

        public static Square operator +(Square square, Vector2Int dir) => new Square(square.Position + dir);
    }
}
