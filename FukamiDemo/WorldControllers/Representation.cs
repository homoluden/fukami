using Drawables;
using Interfaces;
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
        #region Constants

        /// <summary>
        /// Gets or Sets the count of Will Updates before do actual frame rendering
        /// </summary>
        public static sbyte WILL_UPDATE_SKIPS_COUNT = 2;

	    #endregion

        #region Fields

        private readonly ConcurrentDictionary<Guid, ColoredPolygonDrawable> _colPolygons = new ConcurrentDictionary<Guid, ColoredPolygonDrawable>();

        private WeakReference<IRenderer> _renderer = new WeakReference<IRenderer>(null);
        private TaskScheduler _renderingContext;
        private bool _isRenderingCompleted = true;

        private sbyte _updatesToSkip = WILL_UPDATE_SKIPS_COUNT);

        #endregion


        #region Public Methods

        /// <summary>
        /// Adds or replaces the drawable into internal dictionary
        /// </summary>
        /// <param name="newGuid">GUID of new Drawable</param>
        /// <param name="body">The Drawable which value will be copied</param>
        /// <returns>The actual value of added Drawable object.</returns>
        public ColoredPolygonDrawable AddOrReplaceDrawable(Guid newGuid, ColoredPolygonDrawable drawable, ShapeType shapeType = ShapeType.Mesh)
        {
            var actualAdded = _colPolygons.AddOrUpdate(newGuid, drawable, (g, d) =>
            {
                var ancestor = new ColoredPolygonDrawable(drawable.Polygon, shapeType);

                return ancestor;
            });

            return actualAdded;
        }

        public void RegisterRenderer(IRenderer renderer, TaskScheduler scheduler)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException("renderer");
            }

            IRenderer currentRenderer;
            if (_renderer.TryGetTarget(out currentRenderer))
            {
                UnregisterRenderer(currentRenderer);
            }

            _renderer.SetTarget(renderer);
            _renderingContext = scheduler;
        }

        public void UnregisterRenderer(IRenderer renderer)
        {
            _renderer.SetTarget(null);
        }

        #endregion



        #region Singleton

        private static volatile Representation _instance;
        private static object syncRoot = new Object();

        private Representation()
        {
            Will.Instance.Updated += OnWillUpdated;
        }

        void OnWillUpdated(object sender, UpdatedEventArgs e)
        {
            if (!_isRenderingCompleted)
	        {
                return;
	        }

            _updatesToSkip--;
            
            if (_updatesToSkip < 0 )
            {
                _updatesToSkip = WILL_UPDATE_SKIPS_COUNT;

                Task.Factory.StartNew<IRenderer>(s =>
                {
                    IRenderer renderer;
                    if (_renderer.TryGetTarget(out renderer))
                    {
                        // Rendering Call goes here
                    }
                    else
                    {
                        throw new ArgumentNullException("_renderer");
                    }

                    return renderer;
                }, 
                _renderingContext).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        // Log Error call goes here
                    }

                    _isRenderingCompleted = true;
                });
            }
        }

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
