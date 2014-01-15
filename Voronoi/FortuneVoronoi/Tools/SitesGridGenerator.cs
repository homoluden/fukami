using System;
using System.Collections.Generic;

namespace FortuneVoronoi.Tools
{
    public static class SitesGridGenerator
    {
        private static readonly Random Rnd = new Random(DateTime.Now.Millisecond);

        public struct IntSite
        {
            public int X;
            public int Y;
            public bool IsBorder;
        }

        public static List<IntSite> GenerateSymmetricIntGrid(int width, int height, int resolution, int internalSitesCount)
        {
            if (width < 1 || height < 1 || internalSitesCount < 1)
            {
                throw new ArgumentException("'width', 'height', 'internalSitesCount' should be greater than zero.");
            }
            if (resolution < 2)
            {
                throw new ArgumentException("'resolution' should be greater than one.");
            }

            var fullWidth = width*resolution;
            var fullHeight = height*resolution;
            var halfX = (int) System.Math.Floor(fullWidth*0.5);
            var halfY = (int)System.Math.Floor(fullHeight * 0.5);

            var vertBorder = new List<IntSite>();
            for (int i = 0, j = 0; i < height; i++, j += resolution)
            {
                vertBorder.Add(new IntSite{IsBorder = true, X = - halfX, Y = j - halfY});
                vertBorder.Add(new IntSite { IsBorder = false, X = fullWidth - 1 - halfX, Y = j - halfY });

                var randomShift = Rnd.Next(resolution - 1);
                vertBorder.Add(new IntSite { IsBorder = false, X = 1 - halfX, Y = j + randomShift - halfY });
                vertBorder.Add(new IntSite { IsBorder = true, X = fullWidth - halfX, Y = j + randomShift - halfY });
            }


            var horBorder = new List<IntSite>();
            for (int i = 0, j = 0; i < width; i++, j += resolution)
            {
                horBorder.Add(new IntSite { IsBorder = true, X = j - halfX, Y = -halfY });
                horBorder.Add(new IntSite { IsBorder = false, X = j - halfX, Y = fullHeight - 1 - halfY });

                var randomShift = Rnd.Next(resolution - 1);
                horBorder.Add(new IntSite { IsBorder = false, X = j + randomShift - halfX, Y = 1 - halfY });
                horBorder.Add(new IntSite { IsBorder = true, X = j + randomShift - halfX, Y = fullHeight - halfY });
            }

            var resultingSites = new List<IntSite>(internalSitesCount + vertBorder.Count + horBorder.Count);
            for (int i = 0; i < internalSitesCount; i++)
            {
                resultingSites.Add(new IntSite
                    {
                        IsBorder = false,
                        X = Rnd.Next(- halfX + 2, halfX - 1),
                        Y = Rnd.Next(-halfY + 2, halfY - 1)
                    });
            }

            resultingSites.AddRange(vertBorder);
            resultingSites.AddRange(horBorder);

            return resultingSites;
        }
    }
}
