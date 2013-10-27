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
    public class BoneBody : BaseModelBody
    {
        public BoneModel Model { get; set; }
        public BaseModelBody Parent { get; set; }
        public BaseModelBody Child { get; set; }

        public BoneBody(PhysicsState state, IShape shape, MassInfo massInfo, Coefficients coefficients, Lifespan lifetime, Guid modelId)
            : base(state, shape, massInfo, coefficients, lifetime, modelId)
        {

        }
    }
}
