
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

namespace CustomBodies.Models
{
    public class InterconnectionModel
    {
        public Scalar MaxMissAlign { get; set; }
        public Scalar MaxDistance { get; set; }
        public Scalar Length { get; set; }

        public InterconnectionModel Duplicate()
        {
            return new InterconnectionModel
            {
                MaxMissAlign = this.MaxMissAlign,
                MaxDistance = this.MaxDistance,
                Length = this.Length
            };
        }
    }
}
