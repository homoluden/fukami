using Drawables;
using Physics2DDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldControllers
{
    public static class RepresentationHelper
    {
        /// <summary>
        /// Adds all elements of the chain as Drawables into Representation
        /// </summary>
        /// <param name="chainMembers">The elements of the chain excluding the begining and ending fixed bodies</param>
        /// <remarks>Make it sure that all items in List has "Guid" and "Drawable" values in their "Body.Tags" dictionary</remarks>
        public static void AddChainDrawablesUnsafe(IList<Body> chainMembers)
        {
            foreach (var body in chainMembers)
            {
                Representation.Instance.AddOrReplaceDrawable((Guid)body.Tags["Guid"], (ColoredPolygonDrawable)body.Tags["Drawable"]);
            }
        }

    }
}
