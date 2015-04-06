using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Interfaces;

namespace WorldControllers
{
    public class WorldSnapshot : IWorldSnapshot
    {

        public Body[] Bodies { get; set; }

        public Joint[] Joints { get; set; }
    }
}
