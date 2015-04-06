
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif

using System;
using AdvanceMath;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using System.Collections.Generic;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

namespace CustomBodies
{
    public class BaseModelBody
    {
        #region Properties

        public Guid ModelId { get; set; }

        public Body Body { get; set; }

        #endregion


        #region Ctors

        public BaseModelBody()
            : this(Guid.NewGuid(), null)
        {
        }

        public BaseModelBody(Body body)
            : this(Guid.NewGuid(), body)
        {
        }

        public BaseModelBody(Guid id, Body body)
        {
            ModelId = id;

            Body = body;
        }

        #endregion

    }

}
