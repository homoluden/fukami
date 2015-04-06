using FarseerPhysics.Dynamics;
using Interfaces;

namespace CustomBodies
{
	public class ConnectionSlotBody : BaseModelBody
    {
        public BaseModelBody Parent { get; set; }
        public IConnectionSlot Model { get; set; }

        public ConnectionSlotBody()
            : base()
        {
        }

        public ConnectionSlotBody(IConnectionSlot model, Body body)
            : base(body)
        {
			Model = model;
        }
    }
}
