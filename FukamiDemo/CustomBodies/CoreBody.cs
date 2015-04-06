using CustomBodies.Models;
using Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;

namespace CustomBodies
{
    public class CoreBody : BaseModelBody, IHaveConnectionSlots
    {
        public CoreModel Model { get; set; }
        public IList<BaseModelBody> Children { get; private set; }
        public IEnumerable<IConnectionSlot> Slots
        {
            get { return Model.ConnectionSlots; }
        }

        public Transform Transform
		{
			get
			{
				Transform transform;
				Body.GetTransform(out transform);

				return transform;
			}
		}

        public CoreBody()
            :base()
        {
			Children = new List<BaseModelBody>();
        }

        public CoreBody(CoreModel model, Body body)
            :base(body)
        {
			Model = model;

			Children = new List<BaseModelBody>();
        }

    }
}
