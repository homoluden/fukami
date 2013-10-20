using Drawables;
using Physics2DDotNet;
using Physics2DDotNet.Joints;
using Physics2DDotNet.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomBodies
{
    public class ChainMember : BasePolygonBody
    {
        public Joint BegJoint 
        {
            get { return (Joint)Tags["J1"]; }
            set { Tags["J1"] = value; } 
        }

        public Joint EndJoint
        {
            get { return (Joint)Tags["J2"]; }
            set { Tags["J2"] = value; }
        }

        /// <summary>
        /// Creates ChainMember wrap over the Body with additional properies from Tags
        /// </summary>
        /// <param name="body">Original Body</param>
        /// <returns>The copy of original Body</returns>
        public static ChainMember Create(Body body)
        {
            var result = new ChainMember(body.State, body.Shape, body.Mass, body.Coefficients, body.Lifetime);
            object j1, j2, guid, drawable;

            body.Tags.TryGetValue("J1", out j1);
            body.Tags.TryGetValue("J2", out j2);
            body.Tags.TryGetValue("Guid", out guid);
            body.Tags.TryGetValue("Drawable", out drawable);

            result.BegJoint = j1 as Joint;
            result.EndJoint = j2 as Joint;
            result.Guid = guid is Guid ? (Guid)guid : Guid.Empty;
            result.Drawable = drawable as ColoredPolygonDrawable;

            return result;
        }


        public ChainMember(PhysicsState state, IShape shape, double mass, Coefficients coefficients, Lifespan lifetime)
            : base(state, shape, mass, coefficients, lifetime)
        {
        }
        public ChainMember(PhysicsState state, IShape shape, MassInfo mass, Coefficients coefficients, Lifespan lifetime)
            : base(state, shape, mass, coefficients, lifetime)
        {
        }
    }
}
