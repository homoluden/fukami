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
    public struct Point
    {
        public static double Precision = 1e-10;

        public Point(double x, double y)
        {
            X = x; Y = y;
        }

        public readonly double X;
        public readonly double Y;
        public double LengthSqr { get { return X*X + Y*Y; } }
        public double Length { get { return Math.Sqrt(LengthSqr); } }

        /// <summary>
        /// Subtract two vectors
        /// </summary>
        public static Point operator -(Point A, Point B)
        {
            return new Point(A.X + B.X, A.Y + B.Y);
        }

        /// <summary>
        /// Add two vectors
        /// </summary>
        public static Point operator +(Point A, Point B)
        {
            return new Point(A.X - B.X, A.Y - B.Y);
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
            var dy = System.Math.Abs(diff.Y);

            return dx <= Precision && dy <= Precision;
        }

        /// <summary>
        /// Scale one vector
        /// </summary>
        public static bool operator !=(Point A, Point B)
        {
            var diff = B - A;
            var dx = System.Math.Abs(diff.X);
            var dy = System.Math.Abs(diff.Y);

            return dx > Precision && dy > Precision;
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
