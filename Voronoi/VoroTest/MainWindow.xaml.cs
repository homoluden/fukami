using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UnityEngine;
using FortuneVoronoi.Common;
using VectorF = FortuneVoronoi.Common.Vector;
using FortuneVoronoi;
using VoroTest.Helpers;
using FortuneVoronoi.Tools;

namespace VoroTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Random _rnd = new System.Random(DateTime.Now.Millisecond);
        VectorF[] _verts;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();

            var cells = new Dictionary<FortuneVoronoi.Common.Point, VoronoiCell>();

            const int internalSitesCnt = 800;
            const int horBorderSitesCnt = 50;
            const int vertBorderSitesCnt = 50;
            const int resolution = 24;
            const double realWidth = 1000.0;
            const double realHeight = 700.0;
            var dx = realWidth/horBorderSitesCnt/resolution;
            var dy = realHeight/vertBorderSitesCnt/resolution;
            
            var sitesGrid = SitesGridGenerator.GenerateSymmetricIntGrid(horBorderSitesCnt, vertBorderSitesCnt, resolution, internalSitesCnt);

            foreach (var site in sitesGrid)
            {
                var x = site.X*dx;
                var y = site.Y*dy;
                var v = new FortuneVoronoi.Common.Point(x, y);

                if (cells.ContainsKey(v))
                {
                    continue;
                }

                cells.Add(v, new VoronoiCell{ IsVisible = !site.IsBorder, Site = new FortuneVoronoi.Common.Point(x, y)});
            }
            
            var graph = Fortune.ComputeVoronoiGraph(cells);

            foreach (var cell in cells.Values.Where(c => c.IsVisible && c.IsClosed))
            {
                var rgbFill = new byte[3];
                _rnd.NextBytes(rgbFill);
                var fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(rgbFill[0], rgbFill[1], rgbFill[2]));

                var triangles = cell.CreateTriangles();
                foreach (var triangle in triangles)
                {
                    triangle.Fill = fill;

                    Canvas.SetLeft(triangle, MainCanvas.ActualWidth * 0.5);
                    Canvas.SetTop(triangle, MainCanvas.ActualHeight * 0.5);
                    MainCanvas.Children.Add(triangle);
                }
            }

        }

    }
}
