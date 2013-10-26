
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

namespace CustomBodies.Models
{
    public class ConnectionSlotModel
    {
        public ALVector2D RelativePosition { get; set; }
        public Scalar MaxSize { get; set; }
        public Scalar MaxMass { get; set; }
    }
}
