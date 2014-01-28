using System;
using System.Collections.Generic;
using BenTools.Data;
using FortuneVoronoi.Common;

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

        public static BenTools.Data.HashSet<IntSite> GenerateTileBorder(int width, int height, int resolution, Func<int,int,IntPoint> randomGenerator)
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

            int deviation = resolution / 2 - 1;

            var vBorderSites = new BenTools.Data.HashSet<IntSite>();
            for (int i = margin, j = resolution; i < height - margin; i++, j += resolution)
            {
                vBorderSites.Add(new IntSite { IsBorder = true, X = 0, Y = j });
                vBorderSites.Add(new IntSite { IsBorder = false, X = resolution, Y = j });
            }

            var vBorderShifted = Shift(vBorderSites, -halfX, -halfY);
            var randomVBorder = new BenTools.Data.HashSet<IntSite>();

            vBorderShifted.ForEach(s => {
                var randomShift = randomGenerator(-deviation, deviation);
                s.X += randomShift.X;
                s.Y += randomShift.Y;
                
                randomVBorder.Add(s);
            });

            var vBorderRepeated = RepeatHorizontally(randomVBorder, fullWidth - resolution);

            var hBorderSites = new BenTools.Data.HashSet<IntSite>();
            for (int i = margin, j = resolution; i < width - margin; i++, j += resolution)
            {
                hBorderSites.Add(new IntSite { IsBorder = true, X = j, Y = 0 });
                hBorderSites.Add(new IntSite { IsBorder = false, X = j, Y = resolution });
            }

            var hBorderShifted = Shift(hBorderSites, -halfX, -halfY);
            var randomHBorder = new BenTools.Data.HashSet<IntSite>();
            hBorderShifted.ForEach(s =>
            {
                var randomShift = randomGenerator(-deviation, deviation);
                s.X += randomShift.X;
                s.Y += randomShift.Y;

                randomHBorder.Add(s);
            });

            var hBorderRepeated = RepeatVertically(randomHBorder, fullHeight - resolution);

            var resultingBorder = new BenTools.Data.HashSet<IntSite>();
            resultingBorder.AddRange(vBorderRepeated);
            resultingBorder.AddRange(hBorderRepeated);

            resultingBorder.AddRange(new[]
            {
                new IntSite{ IsBorder = true, X = -halfX, Y = -halfY},
                new IntSite{ IsBorder = true, X = -halfX, Y = halfY},
                new IntSite{ IsBorder = true, X = halfX, Y = -halfY},
                    new IntSite{ IsBorder = true, X = halfX, Y = halfY},
            });

            return resultingBorder;
        }

        public static BenTools.Data.HashSet<IntSite> GenerateTileBorder(int width, int height, int resolution)
        {
            return GenerateTileBorder(width, height, resolution, (min, max) => new IntPoint(Rnd.Next(min, max), Rnd.Next(min, max)));
        }

        public static BenTools.Data.HashSet<IntSite> GenerateInternalSites(int width, int height, int resolution, int internalSitesCount, Func<int, int, IntPoint> randomGenerator)
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
            
            int deviation = resolution / 2 - 1;

            var randomSites = new BenTools.Data.HashSet<IntSite>();

            for (int i = 0; i < internalSitesCount; i++)
            {
                var randomPoint = randomGenerator(-halfX + resolution + margin, halfX - resolution - margin);

                randomSites.Add(new IntSite
                {
                    IsBorder = false,
                    X = randomPoint.X,
                    Y = randomPoint.Y
                });
            }


            return randomSites;
        }

        public static BenTools.Data.HashSet<IntSite> GenerateInternalSites(int width, int height, int resolution, int internalSitesCount)
        {
            return GenerateInternalSites(width, height, resolution, internalSitesCount, (min, max) => new IntPoint(Rnd.Next(min, max), Rnd.Next(min, max)));
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
