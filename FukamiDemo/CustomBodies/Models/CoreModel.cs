
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics2DDotNet;
using Interfaces;

namespace CustomBodies.Models
{
    public class CoreModel : ICloneable
    {
        public Guid Id { get; set; }
        public Scalar Size { get; set; }
        public Scalar Mass { get; set; }
        public ulong MaxHealth { get; set; }

        public ALVector2D StartPosition { get; set; }
        public IEnumerable<IConnectionSlot> ConnectionSlots { get; set; }

        public object Clone()
        {
            var clone = new CoreModel
            {
                Id = Guid.NewGuid(),
                Size = this.Size,
                Mass = this.Mass,
                MaxHealth = this.MaxHealth,
                StartPosition = this.StartPosition,
                ConnectionSlots = this.ConnectionSlots.Select(s => (IConnectionSlot)s.Clone())
            };
            return clone;
        }
    }
}
