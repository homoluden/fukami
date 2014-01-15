using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using FortuneVoronoi;
using FortuneVoronoi.Common;
using VectorF = FortuneVoronoi.Common.Vector;


namespace VoroTest.Helpers
{
    public static class VorDrawing
    {

        public static Polygon CreateTriangle(Point v1, Point v2, Point v3)
        {
            var poly = new Polygon
            {
                Points = new PointCollection { 
                    new System.Windows.Point(v1.X, v1.Y),
                    new System.Windows.Point(v2.X, v2.Y),
                    new System.Windows.Point(v3.X, v3.Y)
                },
                Stroke = Brushes.Black,
                StrokeThickness = 0.125
            };

            return poly;
        }

        public static Polygon[] CreateTriangles(this VoronoiCell cell)
        { 
            
            var site = cell.Site;
            var triangles = new Polygon[cell.Edges.Count];

            for (int i = 0; i < cell.Edges.Count; i++)
            {
                var edge = cell.Edges[i];

                if (edge.IsInfinite || edge.IsPartlyInfinite)
                {
                    continue;
                }

                Polygon triangle = CreateTriangle(site, edge.VVertexA, edge.VVertexB);

                triangles[i] = triangle;
            }
            
            return triangles;
        }

    }
}
