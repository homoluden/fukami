using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using FortuneVoronoi;
using VectorF = FortuneVoronoi.Common.Vector;


namespace VoroTest.Helpers
{
    public static class VorDrawing
    {

        public static Polygon CreateTriangle(VectorF v1, VectorF v2, VectorF v3)
        {
            var poly = new Polygon
            {
                Points = new PointCollection { 
                    new System.Windows.Point(v1[0], v1[1]),
                    new System.Windows.Point(v2[0], v2[1]),
                    new System.Windows.Point(v3[0], v3[1])
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

                Polygon triangle = VorDrawing.CreateTriangle(site, edge.VVertexA, edge.VVertexB);

                triangles[i] = triangle;
            }
            
            return triangles;
        }


        public static Polygon CreateTriangle(FortuneVoronoi.Common.Point site, VectorF vA, VectorF vB)
        {
            return CreateTriangle(new VectorF(site.X, site.Y), vA, vB);
        }
    }
}
