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

            var margin = 2;

            var vBorderSites = new HashSet<IntSite>();
            for (int i = margin, j = resolution; i < height - margin; i++, j += resolution)
            {
                vBorderSites.Add(new IntSite{ IsBorder = true, X = 0, Y = j });
                vBorderSites.Add(new IntSite { IsBorder = false, X = resolution, Y = j });
            }

            //cornerSites.Add(new IntSite { IsBorder = true, X = 0, Y = 0 }); // Left-Bottom Corner

            var vBorderRepeated = RepeatHorizontally(Randomize(Shift(vBorderSites, -halfX, -halfY),resolution - 1), fullWidth - resolution);

            var hBorderSites = new HashSet<IntSite>();
            for (int i = margin, j = resolution; i < width - margin; i++, j += resolution)
            {
                hBorderSites.Add(new IntSite { IsBorder = true, X = j, Y = 0 });
                hBorderSites.Add(new IntSite { IsBorder = false, X = j, Y = resolution });
            }

            var hBorderRepeated = RepeatVertically(Randomize(Shift(hBorderSites, -halfX, -halfY), resolution / 2 + 2), fullHeight - resolution);

            var borderWithoutCorners = new HashSet<IntSite>();
            borderWithoutCorners.AddRange(vBorderRepeated);
            borderWithoutCorners.AddRange(hBorderRepeated);

            var resultingSites = new List<IntSite>(internalSitesCount + borderWithoutCorners.Count);

            resultingSites.AddRange(new[]
            {
                new IntSite{ IsBorder = true, X = -halfX, Y = -halfY},
                new IntSite{ IsBorder = true, X = -halfX, Y = halfY},
                new IntSite{ IsBorder = true, X = halfX, Y = -halfY},
                    new IntSite{ IsBorder = true, X = halfX, Y = halfY},
            });

            for (int i = 0; i < internalSitesCount; i++)
            {
                resultingSites.Add(new IntSite
                    {
                        IsBorder = false,
                        X = Rnd.Next(-halfX + resolution + margin, halfX - resolution - margin),
                        Y = Rnd.Next(-halfY + resolution + margin, halfY - resolution - margin)
                    });
            }


            resultingSites.AddRange(borderWithoutCorners);

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

        public static List<IntSite> RepeatVertically(IEnumerable<IntSite> originalSites, int deltaY)
        {
            var result = new List<IntSite>();
            foreach (var site in originalSites)
            {
                result.Add(new IntSite{ IsBorder = !site.IsBorder, X = site.X, Y = site.Y + deltaY});
                result.Add(site);
            }

            return result;
        }

        public static List<IntSite> RepeatHorizontally(IEnumerable<IntSite> originalSites, int deltaX)
        {
            var result = new List<IntSite>();
            foreach (var site in originalSites)
            {
                result.Add(new IntSite{ IsBorder = !site.IsBorder, X = site.X + deltaX, Y = site.Y});
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
