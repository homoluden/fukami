
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
using Shapes.Abstract;
using CustomBodies;
using CustomBodies.Models;

namespace WorldControllers
{
    public static class WillHelper
    {
        #region Constants

        public static readonly Coefficients Coefficients = new Coefficients(.5f, 1);

        #endregion

        /// <summary>
        /// Creates new Rectange Body
        /// </summary>
        /// <param name="height">Height of the Body</param>
        /// <param name="width">Width of the Body</param>
        /// <param name="mass">Mass of the Body</param>
        /// <param name="position">Initial Direction and Linear Position of the Body</param>
        /// <returns>Return the new value of the BasePolygonBody</returns>
        /// <remarks>The Guid of new Body will be stored in Body.Tags["Guid"]. The raw Colored Drawable of new Body will be stored in Body.Tags["Drawable"].</remarks>
        public static Body CreateRectangle(Scalar height, Scalar width, Scalar mass, ALVector2D position)
        {
            var vertices = VertexHelper.CreateRectangle(width, height);
            vertices = VertexHelper.Subdivide(vertices, Math.Min(height, width) / 5);

            var boxShape = ShapeFactory.GetOrCreateColoredPolygonShape(vertices, Math.Min(height, width) / 5);

            var newBody = new Body(new PhysicsState(position), boxShape, mass, Coefficients.Duplicate(), new Lifespan());
            
            return newBody;
        }

        /// <summary>
        /// Adds new Circle Body into World
        /// </summary>
        /// <param name="radius">Radius of the Circle Shape</param>
        /// <param name="verticesCount">Count of vertices  of the Circle Shape</param>
        /// <param name="mass">Mass of corresponding Body</param>
        /// <param name="position">Position of the Circle Shape</param>
        /// <param name="modelId">Id of the parent Model</param>
        /// <returns>Newly created and added into world Body object.</returns>
        public static BaseModelBody AddCircle(Scalar radius, ushort verticesCount, Scalar mass, ALVector2D position, Guid modelId)
        {
            var newBody = CreateCircle(radius, verticesCount, mass, modelId);

            newBody.State.Position = position;
            newBody.ApplyPosition();

            Will.Instance.AddBody(newBody);

            return newBody;
        }

        public static BaseModelBody CreateCircle(Scalar radius, ushort verticesCount, Scalar mass,
                                                 Guid modelId)
        {
            var shape = ShapeFactory.CreateColoredCircle(radius, verticesCount);

            var newBody = new BaseModelBody(new PhysicsState(), shape, mass, Coefficients.Duplicate(), new Lifespan(), modelId);

            return newBody;
        }


        /// <summary>
        /// Builds the chain of Bodies with joints and add this chain into World.
        /// </summary>
        /// <param name="position">Direction and position of first chain member.</param>
        /// <param name="boxLength">Chain member (rectangle) length</param>
        /// <param name="boxWidth">Chain member (rectangle) height</param>
        /// <param name="boxMass">Chain member mass</param>
        /// <param name="spacing">Distance between chain members</param>
        /// <param name="length">The chain length</param>
        /// <param name="modelId">Id of the parent Model entity</param>
        /// <returns>The list of Bodies created</returns>
        public static IList<BaseModelBody> BuildChain(Vector2D position, Scalar boxLength, Scalar boxWidth, Scalar boxMass, Scalar spacing, Scalar length, Guid modelId)
        {
            var bodies = new List<BaseModelBody>();
            ChainMember last = null;
            for (Scalar x = 0; x < length; x += boxLength + spacing, position.X += boxLength + spacing)
            {
                var current = ChainMember.Create(CreateRectangle(boxWidth, boxLength, boxMass, new ALVector2D(0, position)), modelId);
                Will.Instance.AddBody(current);

                if (last != null)
                {
                    var anchor = (current.State.Position.Linear + last.State.Position.Linear) * .5f;

                    var joint = new HingeJoint(last, current, anchor, new Lifespan()) {DistanceTolerance = 50, Softness = 0.005f};

                    last.EndJoint = current.BegJoint = joint;

                    Will.Instance.AddJoint(joint);
                }

                bodies.Add(current);
                
                last = current;
            }
            return bodies;
        }

        public static IList<BaseModelBody> BuildCoreBody(CoreModel core, Guid modelId)
        {
            var newCoreBody = CreateCircle(core.Size, 5, core.Mass, modelId);

            newCoreBody.State.Position = core.StartPosition;
            newCoreBody.ApplyPosition();

            return new [] { newCoreBody };
        }


        #region Extensions

        public static BaseModelBody AsModelBody(this Body body, Guid modelId)
        {
            if (body is BaseModelBody)
            {
                return (BaseModelBody) body;
            }
            return new BaseModelBody(body.State, body.Shape, body.Mass, body.Coefficients, body.Lifetime, modelId);
        }

        #endregion

    }
}
