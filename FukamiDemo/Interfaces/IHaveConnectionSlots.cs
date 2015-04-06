using System.Collections.Generic;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

namespace Interfaces
{
	public interface IHaveConnectionSlots
    {
        IEnumerable<IConnectionSlot> Slots { get; }
		Transform Transform { get; }
    }
}
