
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
    public class BaseModelBody : Body
    {
        #region Properties

        public Guid ModelId { get; set; }
        public Scalar Age { get { return Lifetime.Age; } }

        #endregion


        #region Ctors

        public BaseModelBody(PhysicsState state, IShape shape, double mass, Coefficients coefficients, Lifespan lifetime, Guid modelId)
            : this(state, shape, new MassInfo { Mass = mass }, coefficients, lifetime, modelId)
        {
        }

        public BaseModelBody(PhysicsState state, IShape shape, MassInfo massInfo, Coefficients coefficients, Lifespan lifetime, Guid modelId) 
            : base(state, shape, massInfo, coefficients, lifetime)
        {
            if (modelId == Guid.Empty)
            {
                throw new ArgumentException("'guid' cannot be empty!");
            }

            ModelId = modelId;
        }

        #endregion

    }
}
