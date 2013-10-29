
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif

using AdvanceMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics2DDotNet;
using Interfaces;

namespace CustomBodies.Models
{
    public class ConnectionSlotModel : IConnectionSlot
    {
        public ALVector2D RelativePosition { get; set; }
        public Scalar MaxSize { get; set; }
        public Scalar MaxMass { get; set; }
        public bool IsOccupied { get; set; }

        public object Clone()
        {
            return new ConnectionSlotModel
            {
                RelativePosition = this.RelativePosition,
                MaxSize = this.MaxSize,
                MaxMass = this.MaxMass,
                IsOccupied = false
            };
        }
    }
}
