using Drawables;
using Physics2DDotNet;
using Physics2DDotNet.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomBodies
{
    public class BasePolygonBody : Body
    {
        
        public ColoredPolygonDrawable Drawable 
        {
            get { return (ColoredPolygonDrawable)Tags["Drawable"]; }
            set { Tags["Drawable"] = value; } 
        }

        public Guid Guid
        {
            get { return (Guid)Tags["Guid"]; }
            set { Tags["Guid"] = value; }
        }

        /// <summary>
        /// Creates ChainMember wrap over the Body with additional properies from Tags
        /// </summary>
        /// <param name="body">Original Body</param>
        /// <returns>The copy of original Body</returns>
        public static BasePolygonBody Create(Body body)
        {
            var result = new BasePolygonBody(body.State, body.Shape, body.Mass, body.Coefficients, body.Lifetime);
            object guid, drawable;

            body.Tags.TryGetValue("Guid", out guid);
            body.Tags.TryGetValue("Drawable", out drawable);

            result.Guid = guid is Guid ? (Guid)guid : Guid.Empty;
            result.Drawable = drawable as ColoredPolygonDrawable;

            return result;
        }


        public BasePolygonBody(PhysicsState state, IShape shape, double mass, Coefficients coefficients, Lifespan lifetime)
            : base(state, shape, mass, coefficients, lifetime)
        {
        }
        public BasePolygonBody(PhysicsState state, IShape shape, MassInfo mass, Coefficients coefficients, Lifespan lifetime)
            : base(state, shape, mass, coefficients, lifetime)
        {
        }
    }
}
