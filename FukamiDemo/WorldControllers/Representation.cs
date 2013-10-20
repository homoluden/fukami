using Drawables;
using Physics2DDotNet;
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

        /// <summary>
        /// Adds or replaces the drawable into internal dictionary
        /// </summary>
        /// <param name="newGuid">GUID of new Drawable</param>
        /// <param name="body">The Drawable which value will be copied</param>
        /// <returns>The actual value of added Drawable object.</returns>
        public ColoredPolygonDrawable AddOrReplaceDrawable(Guid newGuid, ColoredPolygonDrawable drawable)
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
