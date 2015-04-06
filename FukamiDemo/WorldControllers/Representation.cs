using CustomBodies;
using Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldControllers
{
    public class Representation
    {
        #region Constants

        #endregion

        #region Fields

        private readonly ConcurrentDictionary<Guid, IList<BaseModelBody>> _gameModels = new ConcurrentDictionary<Guid, IList<BaseModelBody>>();

        private readonly WeakReference<IRenderer> _renderer = new WeakReference<IRenderer>(null);
        private TaskScheduler _renderingContext;
        private bool _isRenderingCompleted = true;

        #endregion


        #region Public Methods

        public void RegisterCompositeModel(Guid modelId, IList<BaseModelBody> modelBodies)
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
                    foreach (var bodyModel in list.Where(body => !modelBodies.Contains(body)))
                    {
                        Will.Instance.World.RemoveBody(bodyModel.Body);
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
        private static readonly object SyncRoot = new Object();

        private Representation()
        {
            Will.Instance.AddWorldUpdateCallback(OnWorldUpdated);
        }

        private async Task OnWorldUpdated(WorldSnapshot snapshot)
        {
            if (!_isRenderingCompleted)
            {
                return;
            }

            await Task.Delay(10).ContinueWith(t =>
            {
                _isRenderingCompleted = false;

                IRenderer renderer;
                if (_renderer.TryGetTarget(out renderer))
                {
                    renderer.RenderWorld(snapshot);
                }
                else
                {
                    throw new ArgumentNullException("_renderer");
                }
            }, _renderingContext).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        // Log Error call goes here
                    }

                    _isRenderingCompleted = true;
                });
        }

        public static Representation Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
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
