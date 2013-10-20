using Drawables;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldControllers
{
    public class Representation
    {
        #region Fields

        private readonly ConcurrentDictionary<Guid, ColoredPolygonDrawable> _colPolygons = new ConcurrentDictionary<Guid, ColoredPolygonDrawable>();

        #endregion


        #region Public Methods

        internal ColoredPolygonDrawable AddOrReplaceDrawable(Guid newGuid, ColoredPolygonDrawable drawable)
        {
            var actualAdded = _colPolygons.AddOrUpdate(newGuid, drawable, (g, d) =>
            {
                var ancestor = new ColoredPolygonDrawable(drawable.Polygon);

                return ancestor;
            });

            return actualAdded;
        }

        #endregion



        #region Singleton

        private static volatile Representation _instance;
        private static object syncRoot = new Object();

        private Representation() { }

        public static Representation Instance
        {
            get 
            {
                if (_instance == null) 
                {
                lock (syncRoot) 
                {
                    if (_instance == null)
                        _instance = new Representation();
                }
                }

                return _instance;
            }
        }

	    #endregion

    }
}
