
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
using Interfaces;

namespace WorldControllers
{
    public static class WillHelper
    {
        #region Constants

        public static readonly Coefficients Coefficients = new Coefficients(.5f, 1);

        public static readonly Random Noise = new Random(Environment.TickCount);

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

        public static BaseModelBody CreateCircle(Scalar radius, ushort verticesCount, Scalar mass, Guid modelId)
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

        public static CoreBody CreateCoreBody(CoreModel core, Guid modelId)
        {
            var newCircleBody = CreateCircle(core.Size, 5, core.Mass, modelId);
            newCircleBody.Coefficients = new Physics2DDotNet.Coefficients(0.1, 0.1);
            newCircleBody.State.Position = core.StartPosition;
            newCircleBody.ApplyPosition();

            var newCore = new CoreBody(newCircleBody.State, newCircleBody.Shape, newCircleBody.Mass, newCircleBody.Coefficients, newCircleBody.Lifetime, modelId) 
            { 
                Model = core
            };

            return newCore;
        }

        public static IList<BaseModelBody> BuildNodeSlots(IHaveConnectionSlots parent, Guid modelId)
        {
            var parPos = parent.Position;

            var slots = parent.Slots.Where(s => !s.IsOccupied);

            var result = new List<BaseModelBody>();

            foreach (var slot in slots)
            {
                slot.IsOccupied = false;
                var nodeSlot = CreateConnectionSlotBody(slot, modelId);

                var slotXAngle = slot.Direction + parPos.Angular;
                var slotCenter = Vector2D.Rotate(slotXAngle, new Vector2D(slot.DistanceFromCenter, 0.0f));
                var slotPos = new ALVector2D(slot.Orientation + slotXAngle, slotCenter + parPos.Linear);

                nodeSlot.State.Position = slotPos;
                nodeSlot.ApplyPosition();

                nodeSlot.Parent = parent as BaseModelBody;

                result.Add(nodeSlot);
            }

            return result;
        }

        private static ConnectionSlotBody CreateConnectionSlotBody(IConnectionSlot slot, Guid modelId)
        {
            var rectBody = CreateRectangle(6, 6, 0.05f, ALVector2D.Zero);
            rectBody.Coefficients = new Physics2DDotNet.Coefficients(0.1, 0.7);

            var newSlot = new ConnectionSlotBody(rectBody.State, rectBody.Shape, rectBody.Mass, rectBody.Coefficients, rectBody.Lifetime, modelId) 
            { 
                Model = slot
            };

            return newSlot;
        }

        public static BoneBody AddBoneBody(BoneModel boneModel)
        {
            var slotBody = Will.Instance.Bodies.RandomOrDefault<ConnectionSlotBody>(b => !b.Model.IsOccupied);
            if (slotBody == null)
	        {
                return null; // TODO: replace with unconnected bone
	        }

            var newBone = slotBody.AddBone(boneModel);

            return newBone;
        }

        #region Extensions

        public static BoneBody AddBone(this ConnectionSlotBody slotBody, BoneModel boneModel)
        {
            var slot = slotBody.Model;
            slot.IsOccupied = true;

            var slotPos = slotBody.State.Position;
            var centerLoc = Vector2D.FromLengthAndAngle(boneModel.Length * 0.6, slotPos.Angular); // +0.1 as gap

            var bonePos = new ALVector2D(slotPos.Angular, slotPos.Linear + centerLoc);

            var rectBody = CreateRectangle(boneModel.Thickness, boneModel.Length, 0.1f, bonePos);

            var newBone = rectBody.CopyAsBone(slotBody.ModelId);
            newBone.Model = boneModel;
            newBone.Parent = slotBody;

            var joints = slotBody.ConnectWith(newBone, (2 * bonePos.Linear + 8 * slotBody.State.Position.Linear) * 0.1f);

            Will.Instance.AddBody(newBone);
            Will.Instance.AddJoint(joints.Item1);
            Will.Instance.AddJoint(joints.Item2);

            return newBone;
        }

        public static BaseModelBody AsModelBody(this Body body, Guid modelId)
        {
            if (body is BaseModelBody)
            {
                return (BaseModelBody) body;
            }
            return new BaseModelBody(body.State, body.Shape, body.Mass, body.Coefficients, body.Lifetime, modelId);
        }

        public static T RandomOrDefault<T>(this IEnumerable<Body> items, Func<T, bool> predicate) where T : Body
        {
            var itemsArray = items.AsParallel().OfType<T>().Where(predicate).ToArray();

            var n = itemsArray.Length;

            if (n == 0)
            {
                return null;
            }

            return itemsArray[Noise.Next(n)];
        }

        #endregion


    }
}
