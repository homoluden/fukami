using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;

namespace Interfaces
{
    public interface IWorldSnapshot
    {
        Body[] Bodies { get; set; }

        Joint[] Joints { get; set; }
    }
}
