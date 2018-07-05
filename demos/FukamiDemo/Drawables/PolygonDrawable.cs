using Shapes;
using Shapes.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawables
{
    public enum ShapeType
    {
        Triangle,
        Rectangle,
        Elipsis,
        Mesh
    }

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

        public ShapeType BaseShape { get; private set; }

        public ColoredPolygonDrawable(ColoredPolygon polygon, ShapeType shapeType = ShapeType.Mesh)
        {
            if (polygon == null) { throw new ArgumentNullException("polygon"); }

            Polygon = polygon;
            BaseShape = shapeType;
        }
    }
}
