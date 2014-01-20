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
                var randomShiftX = Rnd.Next(resolution - 3);
                var randomShiftY = Rnd.Next(resolution - 3);

                vertBorder.Add(new IntSite{IsBorder = true, X = - halfX + randomShiftX, Y = j + randomShiftY - halfY});
                vertBorder.Add(new IntSite { IsBorder = false, X = fullWidth + randomShiftX - resolution - halfX, Y = j + randomShiftY - halfY });

                randomShiftX = Rnd.Next(resolution - 2);
                randomShiftY = Rnd.Next(resolution - 2);
                vertBorder.Add(new IntSite { IsBorder = false, X = resolution + randomShiftX - halfX, Y = j + randomShiftY - halfY });
                vertBorder.Add(new IntSite { IsBorder = true, X = fullWidth + randomShiftX - halfX, Y = j + randomShiftY - halfY });
            }


            var horBorder = new List<IntSite>();
            for (int i = 0, j = 0; i < width-1; i++, j += resolution)
            {
                var randomShiftX = Rnd.Next(resolution - 3);
                var randomShiftY = Rnd.Next(resolution - 3);

                horBorder.Add(new IntSite { IsBorder = true, X = j + randomShiftX - halfX, Y = -halfY + randomShiftY });
                horBorder.Add(new IntSite { IsBorder = false, X = j + randomShiftX - halfX, Y = fullHeight + randomShiftY - resolution - halfY });

                randomShiftX = Rnd.Next(resolution - 2);
                randomShiftY = Rnd.Next(resolution - 2);
                horBorder.Add(new IntSite { IsBorder = false, X = j + randomShiftX - halfX, Y = resolution + randomShiftY - halfY });
                horBorder.Add(new IntSite { IsBorder = true, X = j + randomShiftX - halfX, Y = fullHeight + randomShiftY - halfY });
            }

            var resultingSites = new List<IntSite>(internalSitesCount + vertBorder.Count + horBorder.Count);
            var internalSitesPadding = resolution * 3;
            for (int i = 0; i < internalSitesCount; i++)
            {
                resultingSites.Add(new IntSite
                    {
                        IsBorder = false,
                        X = Rnd.Next(-halfX + internalSitesPadding, halfX - internalSitesPadding),
                        Y = Rnd.Next(-halfY + internalSitesPadding, halfY - internalSitesPadding)
                    });
            }

            resultingSites.AddRange(vertBorder);
            resultingSites.AddRange(horBorder);

            return resultingSites;
        }
    }
}
