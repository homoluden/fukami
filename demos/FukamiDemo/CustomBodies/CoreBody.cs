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
    public class CoreBody : BaseModelBody, IHaveConnectionSlots
    {
        public CoreModel Model { get; set; }
        public IList<BaseModelBody> Children { get; set; }
        public IEnumerable<IConnectionSlot> Slots
        {
            get { return Model.ConnectionSlots; }
        }
        public ALVector2D Position { get { return State.Position; } }

        public CoreBody(PhysicsState state, IShape shape, MassInfo massInfo, Coefficients coefficients, Lifespan lifetime, Guid modelId)
            :base(state, shape, massInfo, coefficients, lifetime, modelId)
        {
        }

    }
}
