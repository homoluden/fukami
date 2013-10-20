using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldControllers
{
    public class WorldSnapshot
    {

        public Physics2DDotNet.Body[] Bodies { get; set; }

        public Physics2DDotNet.Joints.Joint[] Joints { get; set; }
    }
}
