using CustomBodies.Models;
using Interfaces;
using Physics2DDotNet;
using Physics2DDotNet.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomBodies
{
    public class ConnectionSlotBody : BaseModelBody
    {
        public BaseModelBody Parent { get; set; }
        public IConnectionSlot Model { get; set; }

        public ConnectionSlotBody(PhysicsState state, IShape shape, MassInfo massInfo, Coefficients coefficients, Lifespan lifetime, Guid modelId)
            : base(state, shape, massInfo, coefficients, lifetime, modelId)
        {

        }
    }
}
