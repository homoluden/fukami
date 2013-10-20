
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif


using AdvanceMath;
using Physics2DDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics2DDotNet.Shapes;
using Factories;
using Shapes;
using Physics2DDotNet.Joints;
using Drawables;

namespace WorldControllers
{
    public static class WillHelper
    {
        #region Constants

        public static readonly Coefficients Coefficients = new Coefficients(.5f, 1);

        #endregion


        public static Body AddRectangle(Scalar height, Scalar width, Scalar mass, ALVector2D position)
        {
            Vector2D[] vertexes = VertexHelper.CreateRectangle(width, height);
            vertexes = VertexHelper.Subdivide(vertexes, Math.Min(height, width) / 5);

            var boxShape = ShapeFactory.GetOrCreateColoredPolygonShape(vertexes, Math.Min(height, width) / 5);

            var boxDrawable = DrawableFactory.GetOrCreateColoredPolygonDrawable((ColoredPolygon)boxShape.Tag);
            
            Body tempBody = new Body(new PhysicsState(position), boxShape, mass, Coefficients.Duplicate(), new Lifespan());
            
            var newGuid = Guid.NewGuid();

            var actualBody = Will.Instance.AddOrReplaceBody(newGuid, tempBody);
            actualBody.Tags["Guid"] = newGuid;
            actualBody.Tags["Drawable"] = boxDrawable;
            
            return actualBody;
        }

        public static List<Body> AddChain(Vector2D position, Scalar boxLenght, Scalar boxWidth, Scalar boxMass, Scalar spacing, Scalar length)
        {
            var bodies = new List<Body>();
            Body last = null;
            for (Scalar x = 0; x < length; x += boxLenght + spacing, position.X += boxLenght + spacing)
            {
                var current = AddRectangle(boxWidth, boxLenght, boxMass, new ALVector2D(0, position));
                bodies.Add(current);
                if (last != null)
                {
                    Vector2D anchor = (current.State.Position.Linear + last.State.Position.Linear) * .5f;
                    var joint = new HingeJoint(last, current, anchor, new Lifespan());
                    joint.DistanceTolerance = 10;
                    Will.Instance.AddJoint(joint);
                }
                last = current;
            }
            return bodies;
        }

    }
}
