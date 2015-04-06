using AdvanceMath;
using CustomBodies;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics.Joints;
using System.Threading;

namespace WorldControllers
{
    

    public class Will
    {
        #region Fields

        internal World _world;
        Stopwatch _timer;

        CancellationTokenSource _worldUpdateToken;
        Task _worldUpdateTask;
        
        //private readonly ConcurrentDictionary<Guid, Body> _bodies = new ConcurrentDictionary<Guid, Body>();

        #endregion // Fields


        #region Properties

        public IEnumerable<Body> Bodies 
        {
            get { return _world.BodyList.AsEnumerable(); }
        }

        #endregion // Properties


        #region Events

        //public event EventHandler<UpdatedEventArgs> Updated;

        #endregion // Events


        #region Private Methods

        #endregion // Private Methods


        #region Public Methods

        public void Purge()
        {
            RunPauseWilling(false);

            foreach (var body in _world.BodyList)
            {
                _world.RemoveBody(body);
            }
            
            foreach (var joint in _world.JointList)
            {
                _world.RemoveJoint(joint);
            }
        }

		public void Run()
		{
			StartWork();
		}

		public void Stop()
		{
			StopWork();
		}

        /// <summary>
        /// Adds the body into internal dictionary and underlying physics engine
        /// </summary>
        public Body CreateDynamicBody()
        {
            var dynBody = BodyFactory.CreateBody(_world, default(Vector2), 0f, BodyType.Dynamic);
            dynBody.SleepingAllowed = true;
			
            return dynBody;
        }

        /// <summary>
        /// Creates a model for Body and links them together
        /// </summary>
        public T CreateModelForBody<T>(Body body) where T : BaseModelBody, new()
        {
            var model = new T();

			model.Body = body;
            body.UserData = model;

            return model;
        }

        public void CreateAngleJoint(BaseModelBody bodyA, BaseModelBody bodyB)
        {
            JointFactory.CreateAngleJoint(_world, bodyA.Body, bodyB.Body);
        }

        /// <summary>
        /// Creates the copy of current World State
        /// </summary>
        /// <returns></returns>
        public WorldSnapshot GetWorldSnapshot()
        {
            var snapshot = new WorldSnapshot()
            {
                Bodies = _world.BodyList.ToArray(),
                Joints = _world.JointList.ToArray()
            };

            return snapshot;
        }

        #endregion // Public Methods


        #region Singleton
        private static volatile Will _instance;
        private static readonly object SyncRoot = new Object();

        private Will() 
        {
            _world = new World(new Vector2(0f, 9.82f));

            _timer = new Stopwatch();
        }

        public static Will Instance
        {
            get 
            {
                if (_instance == null) 
                {
                lock (SyncRoot) 
                {
                    if (_instance == null)
                        _instance = new Will();
                }
                }

                return _instance;
            }
        }
        #endregion


        #region Private Methods

        void StopWork()
        {
            _worldUpdateToken.Cancel();

            try
            {
                _timer.Stop();
                _worldUpdateTask.Wait();
            }
            catch (AggregateException) { }
        }

        void StartWork()
        {
            _worldUpdateToken = new CancellationTokenSource();

            _worldUpdateTask = Task.Run(async () =>  // <- marked async
            {
                _timer.Stop();
                _timer.Start();

                while (true)
                {
                    var elapsed = _timer.ElapsedMilliseconds * 0.001f;
                    UpdateWorld(elapsed);
                    var elapsedWithUpdate = _timer.ElapsedMilliseconds * 0.001f;
                    var updateTime = elapsedWithUpdate - elapsed;
                    var dynamicDelay = Math.Min((int)((0.033333f - updateTime) * 1000), 10); // Min Delay is 10 msec

                    _timer.Reset();

                    await Task.Delay(dynamicDelay, _worldUpdateToken.Token); // <- await with cancellation
                }
            }, _worldUpdateToken.Token);
        }

        public void UpdateWorld(float dt)
        {
            _world.Step(dt);
        }
        #endregion // Private Methods
    }
}
