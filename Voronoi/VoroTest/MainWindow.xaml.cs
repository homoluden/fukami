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

            var cells = new Dictionary<VectorF, VoronoiCell>();
            
            for (int i = 0; i < 400; i++)
            {
                var v = new VectorF((500f * ((float)_rnd.NextDouble() - 0.5f)), (500f * ((float)_rnd.NextDouble() - 0.5f)));
                cells.Add(v, new VoronoiCell { Site = new FortuneVoronoi.Common.Point(v[0], v[1]), IsVisible = true });
            }
            
            var edgeV = new VectorF(-300f, 0f);
            cells.Add(edgeV, new VoronoiCell { Site = new FortuneVoronoi.Common.Point(edgeV[0], edgeV[1]) });

            edgeV = new VectorF(300f, 0f);
            cells.Add(edgeV, new VoronoiCell { Site = new FortuneVoronoi.Common.Point(edgeV[0], edgeV[1]) });
            edgeV = new VectorF(0f, 300f);
            cells.Add(edgeV, new VoronoiCell { Site = new FortuneVoronoi.Common.Point(edgeV[0], edgeV[1]) });
            edgeV = new VectorF(0f, -300f);
            cells.Add(edgeV, new VoronoiCell { Site = new FortuneVoronoi.Common.Point(edgeV[0], edgeV[1]) });

            edgeV = new VectorF(-300f, -300f);
            cells.Add(edgeV, new VoronoiCell { Site = new FortuneVoronoi.Common.Point(edgeV[0], edgeV[1]) });
            edgeV = new VectorF(300f, -300f);
            cells.Add(edgeV, new VoronoiCell { Site = new FortuneVoronoi.Common.Point(edgeV[0], edgeV[1]) });
            edgeV = new VectorF(-300f, 300f);
            cells.Add(edgeV, new VoronoiCell { Site = new FortuneVoronoi.Common.Point(edgeV[0], edgeV[1]) });
            edgeV = new VectorF(300f, 300f);
            cells.Add(edgeV, new VoronoiCell { Site = new FortuneVoronoi.Common.Point(edgeV[0], edgeV[1]) });            

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
