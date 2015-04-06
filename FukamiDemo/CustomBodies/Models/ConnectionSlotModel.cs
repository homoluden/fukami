
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif

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

        public Scalar Size { get; set; }
        public Scalar MaxChildSize { get; set; }
        public Scalar MaxChildMass { get; set; }
        public bool IsOccupied { get; set; }

        public IConnectionSlot Duplicate()
        {
            return new ConnectionSlotModel
            {
                Size = this.Size,
                DistanceFromCenter = this.DistanceFromCenter,
                Direction = this.Direction,
                Orientation = this.Orientation,
                MaxChildSize = this.MaxChildSize,
                MaxChildMass = this.MaxChildMass,
                IsOccupied = false
            };
        }
    }
}
