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

        private static void CalculateGridParameters(int width, int height, int resolution, out int fullWidth, out int fullHeight, out int halfX, out int halfY, out int margin)
        { 
            fullWidth = width * resolution;
            fullHeight = height * resolution;
            halfX = (int)System.Math.Floor(fullWidth * 0.5);
            halfY = (int)System.Math.Floor(fullHeight * 0.5);

            margin = 2;
        }

        public static BenTools.Data.HashSet<IntSite> GenerateTileBorder(int width, int height, int resolution)
        {
            if (width < 1 || height < 1)
            {
                throw new ArgumentException("'width', 'height' should be greater than zero.");
            }
            if (resolution < 2)
            {
                throw new ArgumentException("'resolution' should be greater than one.");
            }

            int fullWidth, fullHeight, halfX, halfY, margin;
            CalculateGridParameters(width, height, resolution, out fullWidth, out fullHeight, out halfX, out halfY, out margin);

            var vBorderSites = new BenTools.Data.HashSet<IntSite>();
            for (int i = margin, j = resolution; i < height - margin; i++, j += resolution)
            {
                vBorderSites.Add(new IntSite{ IsBorder = true, X = 0, Y = j });
                vBorderSites.Add(new IntSite { IsBorder = false, X = resolution, Y = j });
            }

            var vBorderRepeated = RepeatHorizontally(Randomize(Shift(vBorderSites, -halfX, -halfY),resolution - 1), fullWidth - resolution);

            var hBorderSites = new BenTools.Data.HashSet<IntSite>();
            for (int i = margin, j = resolution; i < width - margin; i++, j += resolution)
            {
                hBorderSites.Add(new IntSite { IsBorder = true, X = j, Y = 0 });
                hBorderSites.Add(new IntSite { IsBorder = false, X = j, Y = resolution });
            }

            var hBorderRepeated = RepeatVertically(Randomize(Shift(hBorderSites, -halfX, -halfY), resolution / 2 + 2), fullHeight - resolution);

            var borderWithoutCorners = new BenTools.Data.HashSet<IntSite>();
            borderWithoutCorners.AddRange(vBorderRepeated);
            borderWithoutCorners.AddRange(hBorderRepeated);

            borderWithoutCorners.AddRange(new[]
            {
                new IntSite{ IsBorder = true, X = -halfX, Y = -halfY},
                new IntSite{ IsBorder = true, X = -halfX, Y = halfY},
                new IntSite{ IsBorder = true, X = halfX, Y = -halfY},
                    new IntSite{ IsBorder = true, X = halfX, Y = halfY},
            });

            return borderWithoutCorners;
        }

        public static BenTools.Data.HashSet<IntSite> GenerateInternalSites(int width, int height, int resolution, int internalSitesCount)
        {
            if (width < 1 || height < 1 || internalSitesCount < 1)
            {
                throw new ArgumentException("'width', 'height', 'internalSitesCount' should be greater than zero.");
            }
            if (resolution < 2)
            {
                throw new ArgumentException("'resolution' should be greater than one.");
            }

            int fullWidth, fullHeight, halfX, halfY, margin;
            CalculateGridParameters(width, height, resolution, out fullWidth, out fullHeight, out halfX, out halfY, out margin);

            var randomSites = new BenTools.Data.HashSet<IntSite>();

            for (int i = 0; i < internalSitesCount; i++)
            {
                randomSites.Add(new IntSite
                {
                    IsBorder = false,
                    X = Rnd.Next(-halfX + resolution + margin, halfX - resolution - margin),
                    Y = Rnd.Next(-halfY + resolution + margin, halfY - resolution - margin)
                });
            }


            return randomSites;
        }

        public static List<IntSite> Randomize(IEnumerable<IntSite> regularGrid, int resolution)
        {
            var result = new List<IntSite>();

            int deviation = resolution / 2;

            foreach (var site in regularGrid)
            {
                result.Add(new IntSite { IsBorder = site.IsBorder, X = site.X + Rnd.Next(-deviation, deviation), Y = site.Y + Rnd.Next(-deviation, deviation) });
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
