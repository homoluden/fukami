using Physics2DDotNet;
using Physics2DDotNet.Joints;
using Physics2DDotNet.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shapes.Physic
{
    public class ChainMember : Body
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


        public ChainMember(PhysicsState state, IShape shape, double mass, Coefficients coefficients, Lifespan lifetime)
            : base(state, shape, mass, coefficients, lifetime)
        {
        }
        public ChainMember(PhysicsState state, IShape shape, MassInfo mass, Coefficients coefficients, Lifespan lifetime)
            : base(state, shape, mass, coefficients, lifetime)
        {
        }
    }
}
