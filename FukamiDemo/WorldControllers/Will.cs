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

        private readonly ConcurrentDictionary<Guid, Body> _bodies = new ConcurrentDictionary<Guid, Body>();

        #endregion // Fields


        #region Properties

        #endregion // Properties


        #region Private Methods

        #endregion // Private Methods


        #region Public Methods

        /// <summary>
        /// Adds or replaces the body in internal dictionary and underlying physical engine
        /// </summary>
        /// <param name="newGuid">GUID of new Body</param>
        /// <param name="body">The Body value which will be copied</param>
        /// <returns>The actual value of added Body object.</returns>
        public Body AddOrReplaceBody(Guid newGuid, Body body)
        {
            var actualAdded = _bodies.AddOrUpdate(newGuid, body, (g, b) =>
            {
                b.Lifetime.IsExpired = true;

                var ancestor = new Body(body.State, body.Shape, body.Mass, body.Coefficients, new Lifespan() { Age = b.Lifetime.Age });
                
                return ancestor;
            });

            _engine.AddBody(actualAdded);

            return actualAdded;
        }

        public void AddJoint(HingeJoint joint)
        {
            throw new NotImplementedException();
        }

        #endregion // Public Methods


        #region Singleton
        private static volatile Will _instance;
        private static object syncRoot = new Object();

        private Will() 
        {
            _engine = new PhysicsEngine();
            _engine.BroadPhase = new Physics2DDotNet.Detectors.SelectiveSweepDetector();
            _engine.Solver = new Physics2DDotNet.Solvers.SequentialImpulsesSolver();

            _timer = new PhysicsTimer(_engine.Update, .01f);

            _engine.AddLogic(new GravityField(new Vector2D(0, 1000), new Lifespan()));
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
