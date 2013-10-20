
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif


using AdvanceMath;
using Physics2DDotNet.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shapes;
using System.Collections.Concurrent;

namespace Factories
{
    public static class ShapeFactory
    {
        #region Consts

        public static readonly Scalar VERTICES_TOLERANCE = 0.1f;

        #endregion

        #region Fields

        internal static ConcurrentDictionary<ColoredPolygon, IShape> _colShapes = new ConcurrentDictionary<ColoredPolygon, IShape>();

        #endregion

        /// <summary>
        /// Creates the array of colors.
        /// </summary>
        /// <param name="first">The color of first item in array</param>
        /// <param name="rest">Color of all array items except first one</param>
        /// <param name="count">The length of array</param>
        /// <returns>The array of colors created.</returns>
        public static ScalarColor3[] CreateColor3Array(ScalarColor3 first, ScalarColor3 rest, int count)
        {
            var result = new ScalarColor3[count];
            result[0] = first;
            for (int index = 1; index < count; ++index)
            {
                result[index] = rest;
            }
            return result;
        }


        public static ScalarColor3[] CreateColor3Array(int count)
        {
            return CreateColor3Array(new ScalarColor3(1, .5f, 0), new ScalarColor3(1, 1, 1), count);
        }

        public static PolygonShape CreatePolygonShape(Polygon polygon, Scalar gridSpacing)
        {
            return new PolygonShape(polygon.Vertices.ToArray(), gridSpacing);
        }

        public static IShape GetOrCreateColoredPolygonShape(Vector2D[] vertices, Scalar gridSpacing)
        {
            var reduced = VertexHelper.Reduce(vertices);
            
            var polygon = new ColoredPolygon(reduced, CreateColor3Array(reduced.Length));

            var shape = _colShapes.GetOrAdd(polygon, p => CreatePolygonShape(p, gridSpacing));

            shape.Tag = polygon;

            return shape;
        }

    }
}
