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
        public BoneModel Model 
        {
            get { return (BoneModel)Tags["Model"]; }
            set { Tags["Model"] = value; }
        }
        public IEnumerable<IConnectionSlot> Slots { get { return Model.ChildSlots; } }
        public ALVector2D Position { get { return State.Position; } }

        public BaseModelBody Parent { get; set; }

        private IList<BaseModelBody> _children;
        public IList<BaseModelBody> Children 
        {
            get 
            {
                if (_children == null)
                {
                    _children = new List<BaseModelBody>();
                }
                return _children;
            }
            set { _children = value; }
        }

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
