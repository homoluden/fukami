
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif

using Physics2DDotNet;
using System;

namespace Interfaces
{
    public interface IConnectionSlot : ICloneable
    {
        ALVector2D RelativePosition { get; set; }
        Scalar MaxSize { get; set; }
        Scalar MaxMass { get; set; }
        bool IsOccupied { get; set; }
    }
}
