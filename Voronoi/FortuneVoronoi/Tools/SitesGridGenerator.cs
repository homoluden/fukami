using System;
using System.Collections.Generic;
using BenTools.Data;

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

            var cornerSites = new HashSet<IntSite>();
            for (int i = 1, j = resolution; i < height; i++, j += resolution)
            {
                cornerSites.Add(new IntSite{ IsBorder = true, X = 0, Y = j });
                cornerSites.Add(new IntSite { IsBorder = false, X = resolution, Y = j });
            }

            cornerSites.Add(new IntSite { IsBorder = true, X = 0, Y = 0 }); // Left-Bottom Corner

            for (int i = 1, j = resolution; i < width; i++, j += resolution)
            {
                cornerSites.Add(new IntSite { IsBorder = true, X = j, Y = 0 });
                cornerSites.Add(new IntSite { IsBorder = false, X = j, Y = resolution });
            }

            var randomizedCorner = Randomize(Shift(cornerSites, -halfX, -halfY), resolution);

            var symBorder = MirrorVertically(MirrorHorizontally(randomizedCorner));

            var resultingSites = new List<IntSite>(internalSitesCount + symBorder.Count);
            for (int i = 0; i < internalSitesCount; i++)
            {
                resultingSites.Add(new IntSite
                    {
                        IsBorder = false,
                        X = Rnd.Next(-halfX + resolution + 1, halfX - resolution - 1),
                        Y = Rnd.Next(-halfY + resolution + 1, halfY - resolution - 1)
                    });
            }


            resultingSites.AddRange(symBorder);

            return resultingSites;
        }

        public static List<IntSite> Randomize(IEnumerable<IntSite> regularGrid, int resolution)
        {
            var result = new List<IntSite>();
            foreach (var site in regularGrid)
            {
                result.Add(new IntSite{ IsBorder = site.IsBorder, X = site.X + Rnd.Next(0, resolution), Y = site.Y + Rnd.Next(0, resolution)});
            }

            return result;
        }

        public static List<IntSite> MirrorVertically(IEnumerable<IntSite> originalSites)
        {
            var result = new List<IntSite>();
            foreach (var site in originalSites)
            {
                result.Add(new IntSite{ IsBorder = site.IsBorder, X = site.X, Y = - site.Y});
                result.Add(site);
            }

            return result;
        }

        public static List<IntSite> MirrorHorizontally(IEnumerable<IntSite> originalSites)
        {
            var result = new List<IntSite>();
            foreach (var site in originalSites)
            {
                result.Add(new IntSite{ IsBorder = site.IsBorder, X = - site.X, Y = site.Y});
                result.Add(site);
            }

            return result;
        }

        public static List<IntSite> Shift(IEnumerable<IntSite> originalSites, int dx, int dy)
        {
            var result = new List<IntSite>();
            foreach (var site in originalSites)
            {
                result.Add(new IntSite{ IsBorder = site.IsBorder, X = site.X + dx, Y = site.Y + dy});
            }

            return result;
        }
    }
}
