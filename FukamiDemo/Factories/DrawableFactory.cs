using Drawables;
using Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Physics2DDotNet.Shapes;

namespace Factories
{
    public static class DrawableFactory
    {
        #region Fields

        private static readonly ConcurrentDictionary<Polygon, PolygonDrawable> _polygons = new ConcurrentDictionary<Polygon, PolygonDrawable>();

        private static readonly ConcurrentDictionary<ColoredPolygon, ColoredPolygonDrawable> _colPolygons = new ConcurrentDictionary<ColoredPolygon, ColoredPolygonDrawable>();

        #endregion

        public static PolygonDrawable GetOrCreatePolygonDrawable(Polygon polygon)
        {
            return _polygons.GetOrAdd(polygon, p => new PolygonDrawable(p));
        }

        public static ColoredPolygonDrawable GetOrCreateColoredPolygonDrawable(ColoredPolygon polygon)
        {
            return _colPolygons.GetOrAdd(polygon, p => new ColoredPolygonDrawable(p));
        }

        static DrawableFactory ()
	    {
	    }
    }
}
