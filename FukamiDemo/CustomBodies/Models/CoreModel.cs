
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

namespace CustomBodies.Models
{
    public class CoreModel
    {
        public Scalar Size { get; set; }
        public Scalar Mass { get; set; }
        public ALVector2D StartPosition { get; set; }
        public IList<ConnectionSlotModel> ConnectionSlots { get; set; }
    }
}
