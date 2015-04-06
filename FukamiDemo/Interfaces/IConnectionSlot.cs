
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif

using System;

namespace Interfaces
{
    public interface IConnectionSlot
    {
        Scalar DistanceFromCenter { get; set; }
        Scalar Direction { get; set; }
        Scalar Orientation { get; set; }
        Scalar Size { get; set; }
        Scalar MaxChildSize { get; set; }
        Scalar MaxChildMass { get; set; }
        bool IsOccupied { get; set; }

        IConnectionSlot Duplicate();
    }
}
