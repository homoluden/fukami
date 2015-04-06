using System.Collections.Concurrent;
using System.Linq;
using CustomBodies;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace WorldControllers
{
    

    public class Will
    {
        #region Fields

        internal World World;
        readonly Stopwatch _timer;

        CancellationTokenSource _worldUpdateToken;
        Task _worldUpdateTask;
        
        private readonly ConcurrentBag<Func<WorldSnapshot, Task>> _worldUpdateCallbacksAsync = new ConcurrentBag<Func<WorldSnapshot, Task>>(); 

        #endregion // Fields


        #region Properties

        public bool IsRunning {
            get { return _timer.IsRunning; }
        }

        public IEnumerable<Body> Bodies 
        {
            get { return World.BodyList.AsEnumerable(); }
        }

        #endregion // Properties

        
        #region Private Methods

        #endregion // Private Methods


        #region Public Methods

        public void AddWorldUpdateCallback(Func<WorldSnapshot, Task> callbackAsync)
        {
            if (callbackAsync == null)
            {
                throw new ArgumentNullException("callbackAsync");
            }

            _worldUpdateCallbacksAsync.Add(callbackAsync);
        }

        public void Purge()
        {
            Stop();

            foreach (var body in World.BodyList)
            {
                World.RemoveBody(body);
            }
            
            foreach (var joint in World.JointList)
            {
                World.RemoveJoint(joint);
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
        /// Creates the copy of current World State
        /// </summary>
        /// <returns></returns>
        public WorldSnapshot GetWorldSnapshot()
        {
            var snapshot = new WorldSnapshot()
            {
                Bodies = World.BodyList.ToArray(),
                Joints = World.JointList.ToArray()
            };

            return snapshot;
        }

        #endregion // Public Methods


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
                    var snapshot = GetWorldSnapshot();
                    var elapsedWithUpdate = _timer.ElapsedMilliseconds * 0.001f;
                    var updateTime = elapsedWithUpdate - elapsed;

                    // Run rendering tasks w/o waiting their completion
                    _worldUpdateCallbacksAsync.ToList().ForEach(t => t.Invoke(snapshot).Start()); 

                    var dynamicDelay = Math.Min((int)((0.033333f - updateTime) * 1000), 10); // Min Delay is 10 msec

                    _timer.Reset();

                    await Task.Delay(dynamicDelay, _worldUpdateToken.Token); // <- await with cancellation
                }
            }, _worldUpdateToken.Token);
        }

        public void UpdateWorld(float dt)
        {
            World.Step(dt);
        }
        #endregion // Private Methods


        #region Singleton
        private static volatile Will _instance;
        private static readonly object SyncRoot = new Object();

        private Will()
        {
            World = new World(new Vector2(0f, 9.82f));

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
    }
}
