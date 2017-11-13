﻿using System;

namespace Com.CodeGame.CodeWars2017.DevKit.CSharpCgdk {
    public struct Vector {
        public Vector(double x, double y) : this() {
            X = x;
            Y = y;
        }

        public IntVector IntVector { get { return new IntVector((int)X, (int)Y); } }
        public double X { get; set; }
        public double Y { get; set; }

        internal double SquareDistanceTo (double x, double y) {
            var dx = X - x;
            var dy =Y - y;
            return dx * dx + dy * dy;
        }

        internal double DistanceTo (int i, int j) {
            return Math.Sqrt(SquareDistanceTo(i, j));
        }

        public override bool Equals (object obj) {
            var v = (Vector)obj;
            return v.X == X && v.Y == Y;
        }
    }

    public struct IntVector {
        public IntVector (int x, int y) : this() {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        internal int SquareDistanceTo (int x, int y) {
            var dx = X - x;
            var dy = Y - y;
            return dx * dx + dy * dy;
        }

        public override string ToString () {
            return "["+X + "; " + Y + ";]";
        }
    }
}