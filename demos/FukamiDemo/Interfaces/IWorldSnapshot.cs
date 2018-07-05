using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IWorldSnapshot
    {
        Physics2DDotNet.Body[] Bodies { get; set; }

        Physics2DDotNet.Joints.Joint[] Joints { get; set; }
    }
}
