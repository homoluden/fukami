using System;
using System.Collections.Generic;
using System.Text;
using FortuneVoronoi.Common;


namespace FortuneVoronoi
{
    public class VoronoiCell
    {
        public Point Site;
        public List<VoronoiEdge> Edges = new List<VoronoiEdge>();

        public bool IsClosed 
        { 
            get 
            {
                bool isClosed = true;
                foreach (var edge in Edges)
                {
                    if (edge.IsInfinite || edge.IsPartlyInfinite)
                    {
                        isClosed = false;
                        break;
                    }
                }

                return isClosed;
            } 
        }

        public bool IsVisible;
    }

}
