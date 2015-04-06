
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using CustomBodies;
using CustomBodies.Models;
using Interfaces;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Joints;

using MathHelper = Microsoft.Xna.Framework.MathHelper;

namespace WorldControllers
{
    public static class WillHelper
    {
        #region Constants

        public static readonly Scalar DefaultAngleSoftness = 0.00001f;

        public static readonly Scalar DefaultHingeSoftness = 1.0f;

        public static readonly Random Noise = new Random(Environment.TickCount);

        #endregion

        /// <summary>
        /// Adds the body into internal dictionary and underlying physics engine
        /// </summary>
        public static Body CreateStaticBody()
        {
            var statBody = BodyFactory.CreateBody(Will.Instance.World);
            statBody.SleepingAllowed = true;
            statBody.IgnoreGravity = true;

            return statBody;
        }

        /// <summary>
        /// Adds the body into internal dictionary and underlying physics engine
        /// </summary>
        public static Body CreateDynamicBody()
        {
            var dynBody = BodyFactory.CreateBody(Will.Instance.World, default(Vector2), 0f, BodyType.Dynamic);
            dynBody.SleepingAllowed = true;

            return dynBody;
        }

        /// <summary>
        /// Creates a model for Body and links them together
        /// </summary>
        public static T CreateModelForBody<T>(Body body) where T : BaseModelBody, new()
        {
            var model = new T {Body = body};

            body.UserData = model;

            return model;
        }

        public static void CreateAngleJoint(BaseModelBody bodyA, BaseModelBody bodyB)
        {
            JointFactory.CreateAngleJoint(Will.Instance.World, bodyA.Body, bodyB.Body);
        }

        /// <summary>
        /// Creates new Rectange Body
        /// </summary>
        public static void AttachRectangleFixture(this Body body, Scalar height, Scalar width, Scalar density, Vector2 position)
        {
            FixtureFactory.AttachRectangle(width, height, density, position, body);
        }

        /// <summary>
        /// Adds new Circle Body into World
        /// </summary>
        public static Fixture AttachCircleFixture(this Body body, Scalar radius, Scalar density, Vector2 position)
        {
            return FixtureFactory.AttachCircle(radius, density, body, position);
        }
        
        public static CoreBody CreateCoreBody(CoreModel core)
        {
            var dynBody = CreateDynamicBody();
            dynBody.AttachCircleFixture(core.Size, core.Density, new Vector2(0.0f, -0.5f * core.Size));

            dynBody.LinearDamping = 0.4f;
            dynBody.AngularDamping = 0.2f;
            
            var newCore = CreateModelForBody<CoreBody>(dynBody);
            newCore.Model = core;

            return newCore;
        }

        public static IList<BaseModelBody> BuildNodeSlots(IHaveConnectionSlots parent, Guid modelId)
        {
            var parPos = parent.Transform;

            var slots = parent.Slots.Where(s => !s.IsOccupied);

            var result = new List<BaseModelBody>();

            foreach (var slot in slots)
            {
                var nodeSlot = CreateConnectionSlotBody(slot);

                var slotXAngle = slot.Direction + parPos.q.GetAngle();

                var slotCenter = new Vector2(slot.DistanceFromCenter, 0.0f).Rotate(slotXAngle); // Zero Angle corresponds to X Axis

                var slotPos = new Vector2((Scalar)slotCenter.X + parPos.p.X, (Scalar)slotCenter.Y + parPos.p.Y);
                
                nodeSlot.Body.SetTransform(ref slotPos, slot.Orientation + slotXAngle);

                nodeSlot.Parent = parent as BaseModelBody;

                result.Add(nodeSlot);
            }

            return result;
        }

        public static ConnectionSlotBody CreateConnectionSlotBody(IConnectionSlot slot)
        {
            var size = slot.Size;

            var vertexList = PolygonTools.CreateRectangle(slot.Size, slot.Size);
            vertexList.Insert(0, new Vector2(size, 0));

            var rectBody = new Body(Will.Instance.World, Vector2.Zero, 0.0f, BodyType.Dynamic)
            {
                SleepingAllowed = true,
                LinearDamping = 0.1f,
                AngularDamping = 0.7f
            };

            var newSlot = new ConnectionSlotBody(slot, rectBody);

            return newSlot;
        }

        public static BoneBody CreateBoneBody(BoneModel boneModel)
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

        public static void AngleRevoluteWith(this Body bodyA, Body bodyB, Vector2 anchor)
        {
            JointFactory.CreateAngleJoint(Will.Instance.World, bodyA, bodyB);
            JointFactory.CreateRevoluteJoint(Will.Instance.World, bodyA, bodyB, anchor);
        }

        public static BoneBody AddBone(this ConnectionSlotBody slotObject, BoneModel boneModel)
        {
            var slot = slotObject.Model;
            slot.IsOccupied = true;

            Transform slotTransform;
            slotObject.Body.GetTransform(out slotTransform);

            var slotSize = slotObject.Model.Size;
            
            var centerLoc = new Vector2((boneModel.Length + slotSize) * 0.5f, 0.0f).Rotate(slotTransform.q.GetAngle());

            var bonePos = new Vector2(slotTransform.p.X + (Scalar)centerLoc.X, slotTransform.p.Y + (Scalar)centerLoc.Y);

            var boneBody = CreateDynamicBody();
            boneBody.AttachRectangleFixture(boneModel.Thickness, boneModel.Length, 0.0001f, Vector2.Zero);

            var newBone = CreateModelForBody<BoneBody>(boneBody);

            newBone.Model = boneModel;
            newBone.Parent = slotObject;

            slotObject.Body.AngleRevoluteWith(boneBody, (2 * bonePos + 8 * slotTransform.p) * 0.1f);
            
            return newBone;
        }
        
        public static T RandomOrDefault<T>(this IEnumerable<Body> items, Func<T, bool> predicate) where T : BaseModelBody
        {
            var randomItem = items.Where(b => b.UserData is T).Select(b => (T)b.UserData).Where(predicate).AsParallel().RandomOrDefault();

            return randomItem;
        }

        public static T RandomOrDefault<T>(this IEnumerable<T> items) where T : class
        {
            var itemsArray = items.ToArray();

            var n = itemsArray.Length;

            if (n == 0)
            {
                return null;
            }

            return itemsArray[Noise.Next(n)];
        }

        public static InterconnectionBody TryAddInterconnectionBody(this InterconnectionModel model, int maxTryCount)
        {
            Will.Instance.Stop();

            var allSlots = Will.Instance.Bodies.Where(b => b.UserData is ConnectionSlotBody)
                .Select(b => (ConnectionSlotBody)b.UserData)
                .Where(s => !s.Model.IsOccupied)
                .ToList();
            
            while (allSlots.Count > 0 && --maxTryCount > 0)
            {                
                var randSlot = allSlots.RandomOrDefault();

                if (randSlot == null)
                {
                    continue;
                }

                var begPos = randSlot.Body.Position;

                var alignedSlot = allSlots.Where(s => !s.Parent.Equals(randSlot.Parent)).FirstOrDefault(s =>
                {
                    var angle = MathHelper.Clamp(s.Body.Rotation + MathHelper.Pi, 0.0f, MathHelper.TwoPi);

                    var diff = Math.Abs(angle - randSlot.Body.Rotation);

                    var dist = (randSlot.Body.Position - s.Body.Position).Length();

                    return diff <= model.MaxMissAlign && dist <= model.MaxDistance;
                });

                if (alignedSlot == null)
                {
                    continue;
                }

                //Aligned slots pair found. Let's build a connection between

                randSlot.Model.IsOccupied = true;
                alignedSlot.Model.IsOccupied = true;
                
                var endPos = alignedSlot.Body.Position;

                var centerPos = Vector2.Lerp(begPos, endPos, 0.5f);
                var begToCenter = centerPos - begPos;
                
                var interconnBody = CreateDynamicBody();
                interconnBody.AttachRectangleFixture(1.0f, model.Length, 0.0001f, Vector2.Zero);
                interconnBody.SetTransform(centerPos, begToCenter.GetAngle().Value);

                var newInterconnObject = CreateModelForBody<InterconnectionBody>(interconnBody);

                newInterconnObject.Model = model;
                newInterconnObject.BegSlot = randSlot;
                newInterconnObject.EndSlot = alignedSlot;
                
                randSlot.Body.Rotation = begToCenter.GetAngle().Value;
                alignedSlot.Body.Rotation = (-begToCenter).GetAngle().Value;

                randSlot.Body.AngleRevoluteWith(interconnBody, (8 * begPos + 2 * centerPos) * 0.1f);
                newInterconnObject.Body.AngleRevoluteWith(alignedSlot.Body, (2 * centerPos + 8 * endPos) * 0.1f);
                
                Will.Instance.Run();

                return newInterconnObject;
            }

            // Sorry :( Aligned pair not found. Please, try again later.
            return null;
        }

        /// <summary>
        /// Returns the angle of the vector in radians using the standard
        /// polar coordinate system, or null if the vector's length is 0.
        /// Assumes the vector is on a grid where x increases to the left 
        /// and y increases downward.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Scalar? GetAngle(this Vector2 v)
        {
            if (Math.Abs(v.X) < 1e-10)
                if (Math.Abs(v.Y) < 1e-10)
                    return null;
                else
                    return MathHelper.PiOver2 * (v.Y < 0 ? 1 : -1);

            return MathHelper.WrapAngle((Scalar)(Math.Atan(-v.Y / v.X) + ((v.X < 0) ? Math.PI : 0)));
        }

        /// <summary>
        /// Returns a new Vector2 object specified by angle and length using
        /// the standard polar coordinate system.
        /// </summary>
        /// <param name="angle">The direction of the Vector2 in radians.</param>
        /// <param name="length">The magnitude of the Vector2.</param>
        /// <returns></returns>
        public static Vector2 CreateVector2(Scalar angle, Scalar length)
        {
            return new Vector2((Scalar)Math.Cos(angle) * length, (Scalar)Math.Sin(-angle) * length);
        }

        public static Vector2 Rotate(this Vector2 source, Scalar radianAngle)
        {
            var num1 = -radianAngle;
            var num2 = (Scalar)Math.Cos(num1);
            var num3 = (Scalar)Math.Sin(num1);
            Vector2 vector;
            vector.X = source.X * num2 + source.Y * num3;
            vector.Y = source.Y * num2 - source.X * num3;
            return vector;
        }

        public static void Rotate(this Vector2 source, Scalar radianAngle, out Vector2 result)
        {
            var num1 = -radianAngle;
            var num2 = (Scalar)Math.Cos(num1);
            var num3 = (Scalar)Math.Sin(num1);
            result.X = source.X * num2 + source.Y * num3;
            result.Y = source.Y * num2 - source.X * num3;
        }

        #endregion


    }
}
