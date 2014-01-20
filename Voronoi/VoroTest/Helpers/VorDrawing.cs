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
            var triangles = new List<Polygon>(cell.Edges.Count);
            var hiddenEdges = cell.Edges.Where(e => e.IsBorder).ToList();
            var visibleEdges = cell.Edges.Where(e => !e.IsBorder);
            
            foreach(var edge in visibleEdges)
            {
                
                if (edge.IsBorder)
                {
                    continue;
                }

                Point a = site, b, c;

                bool hideA = false, hideB = false;
                if (hiddenEdges.Any(e => e.VVertexA == edge.VVertexA || e.VVertexB == edge.VVertexA))
                {
                    hideA = true;
                }
                if (hiddenEdges.Any(e => e.VVertexA == edge.VVertexB || e.VVertexB == edge.VVertexB))
                {
                    hideB = true;
                }

                if (hideA && hideB)
                {
                    continue;
                }
                else if (!hideA && !hideB)
                {
                    b = edge.VVertexA;
                    c = edge.VVertexB;
                }
                else
                {
                    var opposite = edge.LeftData != site ? edge.LeftData : edge.RightData;
                    var mid = (site + opposite)*0.5;
                    b = new Point(mid.X, mid.Y);
                    c = hideB ? edge.VVertexA : edge.VVertexB;
                }

                Polygon triangle = CreateTriangle(a, b, c);

                triangles.Add(triangle);
            }
            
            return triangles.ToArray();
        }

    }
}
