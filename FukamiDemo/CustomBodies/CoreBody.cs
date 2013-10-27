using CustomBodies.Models;
using Physics2DDotNet;
using Physics2DDotNet.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomBodies
{
    public class CoreBody : BaseModelBody
    {
        public CoreModel Model { get; set; }
        public IList<BaseModelBody> ConnectedChildren { get; set; }

        public CoreBody(PhysicsState state, IShape shape, MassInfo massInfo, Coefficients coefficients, Lifespan lifetime, Guid modelId)
            :base(state, shape, massInfo, coefficients, lifetime, modelId)
        {
        }
    }
}
