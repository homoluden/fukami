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
        public Point(double x, double y)
        {
            X = x; Y = y;
        }
        public double X;
        public double Y;
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
