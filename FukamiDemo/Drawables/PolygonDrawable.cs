using Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawables
{
    public class PolygonDrawable
    {
        public Polygon Polygon { get; private set; }

        public PolygonDrawable(Polygon polygon)
        {
            if (polygon == null) { throw new ArgumentNullException("polygon"); }

            Polygon = polygon;
        }
    }
    public class ColoredPolygonDrawable
    {
        public ColoredPolygon Polygon { get; private set; }

        public ColoredPolygonDrawable(ColoredPolygon polygon)
        {
            if (polygon == null) { throw new ArgumentNullException("polygon"); }

            Polygon = polygon;
        }
    }
}
