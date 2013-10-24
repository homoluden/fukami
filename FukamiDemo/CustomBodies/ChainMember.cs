
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif

using Physics2DDotNet;
using Physics2DDotNet.Joints;
using Physics2DDotNet.Shapes;
using System;

namespace CustomBodies
{
    public class ChainMember : BaseBodyModel
    {
        public Joint BegJoint 
        {
            get { return (Joint)Tags["J1"]; }
            set { Tags["J1"] = value; } 
        }

        public Joint EndJoint
        {
            get { return (Joint)Tags["J2"]; }
            set { Tags["J2"] = value; }
        }

        /// <summary>
        /// Creates ChainMember wrap over the Body with additional properies from Tags
        /// </summary>
        /// <param name="body">Original Body</param>
        /// <returns>The copy of original Body</returns>
        /// <remarks>This method will generate new Guid internally</remarks>
        public static ChainMember Create(Body body)
        {
            var newGuid = Guid.NewGuid();
            var result = new ChainMember(body.State, body.Shape, body.Mass, body.Coefficients, body.Lifetime, newGuid);
            object j1, j2;

            body.Tags.TryGetValue("J1", out j1);
            body.Tags.TryGetValue("J2", out j2);

            result.BegJoint = j1 as Joint;
            result.EndJoint = j2 as Joint;

            return result;
        }


        public ChainMember(PhysicsState state, IShape shape, double mass, Coefficients coefficients, Lifespan lifetime, Guid guid)
            : base(state, shape, mass, coefficients, lifetime, guid)
        {
        }
        public ChainMember(PhysicsState state, IShape shape, MassInfo mass, Coefficients coefficients, Lifespan lifetime, Guid guid)
            : base(state, shape, mass, coefficients, lifetime, guid)
        {
        }
    }
}
