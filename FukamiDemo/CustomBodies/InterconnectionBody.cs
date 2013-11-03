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
    public class InterconnectionBody : BaseModelBody
    {
        public InterconnectionModel Model
        {
            get { return (InterconnectionModel)Tags["Model"]; }
            set { Tags["Model"] = value; }
        }

        public InterconnectionBody(PhysicsState state, IShape shape, MassInfo massInfo, Coefficients coefficients, Lifespan lifetime, Guid modelId)
            : base(state, shape, massInfo, coefficients, lifetime, modelId)
        {

        }

        public ConnectionSlotBody BegSlot { get; set; }

        public ConnectionSlotBody EndSlot { get; set; }
    }

    public static class InterconnectionBodyExtensions
    {
        public static InterconnectionBody CopyAsInterconnection(this Body originalBody, Guid modelId)
        {
            return new InterconnectionBody(originalBody.State, originalBody.Shape, originalBody.Mass, originalBody.Coefficients, originalBody.Lifetime, modelId);
        }
    }
}
