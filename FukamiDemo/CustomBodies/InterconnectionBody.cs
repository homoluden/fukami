using CustomBodies.Models;
using FarseerPhysics.Dynamics;

namespace CustomBodies
{
	public class InterconnectionBody : BaseModelBody
    {
        public InterconnectionModel Model { get; set; }

        public InterconnectionBody()
            : base()
        {
        }

        public InterconnectionBody(InterconnectionModel model, Body body)
            : base(body)
        {
			Model = model;
        }

        public ConnectionSlotBody BegSlot { get; set; }

        public ConnectionSlotBody EndSlot { get; set; }
    }
}
