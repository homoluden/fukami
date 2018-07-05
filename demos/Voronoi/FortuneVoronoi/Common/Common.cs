using System;
using System.Collections.Generic;
using System.Text;

namespace FortuneVoronoi.Common
{
    public struct VorSiteF
    {
        public PointF Vertice;
        public bool IsVisible;
    }

    public struct IntPoint
    {
        public readonly int X;
        public readonly int Y;

        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public struct Point
    {
        public static double Precision = 1e-10;

        public Point(double x, double y)
        {
            X = x; Y = y;

            _lengthSqrInt = double.NaN;
            _lengthInt = double.NaN;
        }

        public readonly double X;
        public readonly double Y;

        private double _lengthSqrInt;
        public double LengthSqr { get { return double.IsNaN(_lengthSqrInt) ? _lengthSqrInt = X*X + Y*Y : _lengthSqrInt; } }

        private double _lengthInt;
        public double Length { get { return double.IsNaN(_lengthSqrInt) ? _lengthInt = Math.Sqrt(LengthSqr) : _lengthInt; } }

        /// <summary>
        /// Subtract two vectors
        /// </summary>
        public static Point operator -(Point A, Point B)
        {
            return new Point(A.X - B.X, A.Y - B.Y);
        }

        /// <summary>
        /// Add two vectors
        /// </summary>
        public static Point operator +(Point A, Point B)
        {
            return new Point(A.X + B.X, A.Y + B.Y);
        }

        /// <summary>
        /// Get the scalar product of two vectors
        /// </summary>
        public static double operator *(Point A, Point B)
        {
            return A.X*B.X + A.Y*B.Y;
        }

        /// <summary>
        /// Scale one vector
        /// </summary>
        public static Point operator *(Point A, double B)
        {
            return new Point(A.X * B, A.Y * B);
        }

        /// <summary>
        /// Scale one vector
        /// </summary>
        public static Point operator *(double A, Point B)
        {
            return B * A;
        }

        /// <summary>
        /// Get the distance of two vectors
        /// </summary>
        public static double DistSqr(Point V1, Point V2)
        {
            Point delta = V2 - V1;
            return delta*delta;
        }


        /// <summary>
        /// Scale one vector
        /// </summary>
        public static bool operator ==(Point A, Point B)
        {
            var diff = B - A;

            var dx = System.Math.Abs(diff.X);
            if ((double.IsNaN(A.X) && double.IsNaN(B.X)) || (double.IsPositiveInfinity(A.X) && double.IsPositiveInfinity(B.X)) || (double.IsNegativeInfinity(A.X) && double.IsNegativeInfinity(B.X)))
            {
                dx = 0;
            }

            var dy = System.Math.Abs(diff.Y);
            if ((double.IsNaN(A.Y) && double.IsNaN(B.Y)) || (double.IsPositiveInfinity(A.Y) && double.IsPositiveInfinity(B.Y)) || (double.IsNegativeInfinity(A.Y) && double.IsNegativeInfinity(B.Y)))
            {
                dy = 0;
            }

            return dx <= Precision && dy <= Precision;
        }

        /// <summary>
        /// Scale one vector
        /// </summary>
        public static bool operator !=(Point A, Point B)
        {
            var diff = B - A;
            var dx = System.Math.Abs(diff.X);
            if ((double.IsNaN(A.X) && double.IsNaN(B.X)) || (double.IsPositiveInfinity(A.X) && double.IsPositiveInfinity(B.X)) || (double.IsNegativeInfinity(A.X) && double.IsNegativeInfinity(B.X)))
            {
                dx = 0;
            }

            var dy = System.Math.Abs(diff.Y);
            if ((double.IsNaN(A.Y) && double.IsNaN(B.Y)) || (double.IsPositiveInfinity(A.Y) && double.IsPositiveInfinity(B.Y)) || (double.IsNegativeInfinity(A.Y) && double.IsNegativeInfinity(B.Y)))
            {
                dy = 0;
            }

            return dx > Precision && dy > Precision;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
            {
                return false;
            }

            Point other = (Point) obj;
            if (IsUndefined)
            {
                return other.IsUndefined;
            }

            return this == other;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public bool IsUndefined { get { return double.IsNaN(X) && double.IsNaN(Y); } }

        public override string ToString()
        {
            return string.Format("[{0}; {1}]", X, Y);
        }
    }

    public struct PointF
    {
        public PointF(float x, float y)
        {
            X = x; Y = y;
        }
        public float X;
        public float Y;
    }
}
