
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif

using Physics2DDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics2DDotNet.Shapes;

namespace CustomBodies
{
    public class BaseBodyModel : Body
    {
        #region Properties

        public Guid Guid { get; set; }
        public Scalar Age { get { return Lifetime.Age; } }

        #endregion


        #region Ctors

        public BaseBodyModel(PhysicsState state, IShape shape, double mass, Coefficients coefficients, Lifespan lifetime, Guid guid)
            : this(state, shape, new MassInfo { Mass = mass }, coefficients, lifetime, guid)
        {
        }

        public BaseBodyModel(PhysicsState state, IShape shape, MassInfo massInfo, Coefficients coefficients, Lifespan lifetime, Guid guid) 
            : base(state, shape, massInfo, coefficients, lifetime)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("'guid' cannot be empty!");
            }

            Guid = guid;
        }

        #endregion

    }
}
