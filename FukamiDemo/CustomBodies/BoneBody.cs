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
    public class BoneBody : BaseModelBody, IHaveConnectionSlots
    {
        public BoneModel Model { get; set; }
        public IEnumerable<IConnectionSlot> Slots { get { return Model.ChildSlots; } }

        public BaseModelBody Parent { get; set; }

        public IList<BaseModelBody> Children { get; set; }

        public BoneBody(PhysicsState state, IShape shape, MassInfo massInfo, Coefficients coefficients, Lifespan lifetime, Guid modelId)
            : base(state, shape, massInfo, coefficients, lifetime, modelId)
        {

        }
    }

    public static class BoneBodyExtensions
    {
        public static BoneBody CopyAsBone(this Body originalBody, Guid modelId)
        {
            return new BoneBody(originalBody.State, originalBody.Shape, originalBody.Mass, originalBody.Coefficients, originalBody.Lifetime, modelId);
        }
    }
}
