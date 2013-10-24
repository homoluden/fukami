using AdvanceMath;
using Physics2DDotNet;
using Physics2DDotNet.Joints;
using Physics2DDotNet.PhysicsLogics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldControllers
{
    public class Will
    {
        #region Fields

        PhysicsEngine _engine;
        PhysicsTimer _timer;

        //private readonly ConcurrentDictionary<Guid, Body> _bodies = new ConcurrentDictionary<Guid, Body>();

        #endregion // Fields


        #region Properties

        #endregion // Properties


        #region Events

        public event EventHandler<UpdatedEventArgs> Updated;

        #endregion // Events


        #region Private Methods

        #endregion // Private Methods


        #region Public Methods

        public void RunPauseWilling(bool? setToIsRunning = null)
        {
            if (setToIsRunning.HasValue)
            {
                _timer.IsRunning = setToIsRunning.Value;
            }
            else
            {
                _timer.IsRunning = !_timer.IsRunning;
            }
        }

        /// <summary>
        /// Adds the body into internal dictionary and underlying physical engine
        /// </summary>
        /// <param name="newGuid">GUID of new Body</param>
        /// <param name="body">The Body which value will be copied</param>
        /// <returns>The actual value of added Body object.</returns>
        public void AddBody(Body body)
        {
            _engine.AddBody(body);
        }

        public void AddJoint(Joint joint)
        {
            _engine.AddJoint(joint);
        }

        /// <summary>
        /// Creates the copy of current World State
        /// </summary>
        /// <returns></returns>
        public WorldSnapshot GetWorldSnapshot()
        {
            var snapshot = new WorldSnapshot()
            {
                Bodies = _engine.Bodies.ToArray(),
                Joints = _engine.Joints.ToArray()
            };

            return snapshot;
        }

        #endregion // Public Methods


        #region Singleton
        private static volatile Will _instance;
        private static object syncRoot = new Object();

        private Will() 
        {
            _engine = new PhysicsEngine();
            _engine.BroadPhase = new Physics2DDotNet.Detectors.SelectiveSweepDetector();
            _engine.Solver = new Physics2DDotNet.Solvers.SequentialImpulsesSolver { 
                AllowedPenetration = 0.1f
            };
            _engine.AddLogic(new GravityField(new Vector2D(0, 1000), new Lifespan()));

            _engine.Updated += OnEngineUpdated;

            _timer = new PhysicsTimer(_engine.Update, .01f);

        }

        void OnEngineUpdated(object sender, UpdatedEventArgs e)
        {
            if (Updated != null)
            {
                Updated(sender, e);
            }
        }

        public static Will Instance
        {
            get 
            {
                if (_instance == null) 
                {
                lock (syncRoot) 
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
