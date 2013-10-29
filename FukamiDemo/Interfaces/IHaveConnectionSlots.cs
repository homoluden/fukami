using Physics2DDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IHaveConnectionSlots
    {
        IEnumerable<IConnectionSlot> Slots { get; }
        ALVector2D Position { get; }
    }
}
