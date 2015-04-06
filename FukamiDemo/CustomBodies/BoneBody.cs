using CustomBodies.Models;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;

namespace CustomBodies
{
    public class BoneBody : BaseModelBody, IHaveConnectionSlots
    {
        public BoneModel Model { get; set; }
        public IEnumerable<IConnectionSlot> Slots { get { return Model.ChildSlots; } }
        public Transform Transform
		{
			get
			{
				Transform transform;
				Body.GetTransform(out transform);

				return transform;
			}
		}

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

        public BoneBody(BoneModel model, Body body)
            : base(body)
        {
			Model = model;
        }

        public BoneBody()
            : base()
        {

        }
    }
}
