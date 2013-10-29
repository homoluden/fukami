
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
        /// <summary>
        /// The distance between slot and its owner
        /// </summary>
        public Scalar DistanceFromCenter { get; set; }

        /// <summary>
        /// The angle between X axes of slot and its owner
        /// </summary>
        public Scalar Direction { get; set; }

        /// <summary>
        /// The angle between slot's X axis and its angle of view
        /// </summary>
        public Scalar Orientation { get; set; }

        public Scalar MaxSize { get; set; }
        public Scalar MaxMass { get; set; }
        public bool IsOccupied { get; set; }

        public object Clone()
        {
            return new ConnectionSlotModel
            {
                DistanceFromCenter = this.DistanceFromCenter,
                Direction = this.Direction,
                Orientation = this.Orientation,
                MaxSize = this.MaxSize,
                MaxMass = this.MaxMass,
                IsOccupied = false
            };
        }
    }
}
