
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif

using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomBodies.Models
{
    public class BoneModel
    {
        public double Thickness { get; set; }
        public double Length { get; set; }
        public IEnumerable<IConnectionSlot> ChildSlots { get; set; }
    }
}
