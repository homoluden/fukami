using CustomBodies;
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

        private readonly ConcurrentDictionary<Guid, IList<BaseModelBody>> _gameModels = new ConcurrentDictionary<Guid, IList<BaseModelBody>>();

        private readonly WeakReference<IRenderer> _renderer = new WeakReference<IRenderer>(null);
        private TaskScheduler _renderingContext;
        private bool _isRenderingCompleted = true;

        private sbyte _updatesToSkip = WILL_UPDATE_SKIPS_COUNT;

        #endregion


        #region Public Methods

        public void RegisterModel(Guid modelId, IList<BaseModelBody> modelBodies)
        {
            if (modelId == Guid.Empty)
            {
                throw new ArgumentException("'modelId cannot be empty!'");
            }
            if (modelBodies == null || modelBodies.Count == 0)
            {
                throw new ArgumentException("'modelBodies' cannot be empty!");
            }

            _gameModels.AddOrUpdate(modelId, modelBodies, (id, list) =>
                {
                    foreach (var body in list.Where(body => !modelBodies.Contains(body)))
                    {
                        body.Lifetime.IsExpired = true;
                    }
                    return modelBodies;
                });
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
                        var snapshot = Will.Instance.GetWorldSnapshot();                        
                        renderer.RenderWorld(snapshot);
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
