
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
        public Scalar Thickness { get; set; }
        public Scalar Length { get; set; }
        public IList<IConnectionSlot> ChildSlots { get; set; }

        public BoneModel Duplicate()
        {
            return new BoneModel
            {
                ChildSlots = this.ChildSlots.Select(s => s.Duplicate()).ToList(),
                Length = this.Length,
                Thickness = this.Thickness
            };
        }
    }
}
