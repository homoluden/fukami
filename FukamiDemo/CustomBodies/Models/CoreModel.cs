using FarseerPhysics.Common;
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Microsoft.Xna.Framework;

namespace CustomBodies.Models
{
    public class CoreModel : ICloneable
    {
        public Guid Id { get; set; }
        public Scalar Size { get; set; }
        public Scalar Density { get; set; }
        public ulong MaxHealth { get; set; }

        public Transform StartingTransform { get; set; }

        public IEnumerable<IConnectionSlot> ConnectionSlots { get; set; }

        public object Clone()
        {
            var clone = new CoreModel
            {
                Id = Guid.NewGuid(),
                Size = this.Size,
                Density = this.Density,
                MaxHealth = this.MaxHealth,
                StartingTransform = this.StartingTransform,
                ConnectionSlots = this.ConnectionSlots.Select(s => (IConnectionSlot)s.Duplicate())
            };
            return clone;
        }
    }
}
