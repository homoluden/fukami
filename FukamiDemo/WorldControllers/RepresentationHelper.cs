using Drawables;
using Physics2DDotNet;
using CustomBodies;
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
        public static void AddChainDrawablesUnsafe(IList<ChainMember> chainMembers)
        {
            foreach (var member in chainMembers)
            {
                Representation.Instance.AddOrReplaceDrawable(member.Guid, member.Drawable, ShapeType.Rectangle);
            }
        }


    }
}
