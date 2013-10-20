
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

        /// <summary>
        /// Adds new Rectange Body into World
        /// </summary>
        /// <param name="height">Height of the Body</param>
        /// <param name="width">Width of the Body</param>
        /// <param name="mass">Mass of the Body</param>
        /// <param name="position">Initial Direction and Linear Position of the Body</param>
        /// <returns>Return the actual value of the Body added into world.</returns>
        /// <remarks>The Guid of new Body will be stored in Body.Tags["Guid"]. The raw Colored Drawable of new Body will be stored in Body.Tags["Drawable"].</remarks>
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

        /// <summary>
        /// Builds the chain of Bodies with joints and add this chain into World.
        /// </summary>
        /// <param name="position">Direction and position of first chain member.</param>
        /// <param name="boxLength">Chain member (rectangle) length</param>
        /// <param name="boxWidth">Chain member (rectangle) height</param>
        /// <param name="boxMass">Chain member mass</param>
        /// <param name="spacing">Distance between chain members</param>
<<<<<<< HEAD
        /// <param name="length">The chain length</param>
=======
        /// <param name="length">The count of chain members</param>
>>>>>>> 7905a8d6824cd575bb1454039ed5e30e6122909b
        /// <returns>The list of Bodies created</returns>
        public static List<Body> AddChain(Vector2D position, Scalar boxLength, Scalar boxWidth, Scalar boxMass, Scalar spacing, Scalar length)
        {
            var bodies = new List<Body>();
            Body last = null;
            for (Scalar x = 0; x < length; x += boxLength + spacing, position.X += boxLength + spacing)
            {
                var current = AddRectangle(boxWidth, boxLength, boxMass, new ALVector2D(0, position));
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
