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
using BenTools.Mathematics;
using FortuneVoronoi.Common;
using VectorF = FortuneVoronoi.Common.Vector;

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

            _verts = new VectorF[408];

            for (int i = 0; i < 400; i++)
            {
                _verts[i] = new VectorF((500f * ((float)_rnd.NextDouble() - 0.5f)), (500f * ((float)_rnd.NextDouble() - 0.5f)));
            }
            _verts[400] = new VectorF(-300f, 0f);
            _verts[401] = new VectorF(300f, 0f);
            _verts[402] = new VectorF(0f, 300f);
            _verts[403] = new VectorF(0f, -300f);

            _verts[404] = new VectorF(-300f, -300f);
            _verts[405] = new VectorF(300f, -300f);
            _verts[406] = new VectorF(-300f, 300f);
            _verts[407] = new VectorF(300f, 300f);

            var graph = Fortune.ComputeVoronoiGraph(_verts);

            //var cells = _verts.Select(v =>
            //{
            //    var edges = graph.Edges.Where(edge => edge.LeftData.Equals(v) || edge.RightData.Equals(v)).ToArray();

            //    var cell = new Tuple<VectorF, IEnumerable<VoronoiEdge>, bool>(v,
            //                                                                  edges,
            //                                                                  !edges.Any(ed => ed.IsInfinite || ed.IsPartlyInfinite));

            //    return cell;
            //}).ToArray();

            var cells = graph.Cells.Select(c =>
                new Tuple<VectorF, IList<VoronoiEdge>, bool>(
                    c.Key,
                    c.Value,
                    !c.Value.Any(ed => ed.IsInfinite || ed.IsPartlyInfinite)));

            foreach (var cell in cells.Where(c => c.Item3)) //
            {
                var site = cell.Item1;
                var edges = cell.Item2.ToArray();

                var rgbFill = new byte[3];
                _rnd.NextBytes(rgbFill);
                var fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(rgbFill[0], rgbFill[1], rgbFill[2]));
                foreach (var edge in edges)
                {
                    //if (double.IsNaN(edge.VVertexA[0]) || double.IsNaN(edge.VVertexA[1]) || double.IsNaN(edge.VVertexB[0]) || double.IsNaN(edge.VVertexB[1]))
                    if(edge.IsInfinite || edge.IsPartlyInfinite)
                    {
                        continue;
                    }

                    Polygon triangle;

                    if (site.Equals(edge.RightData))
                    {
                        triangle = CreateTriangle(site, edge.VVertexA, edge.VVertexB);
                    }
                    else
                    {
                        triangle = CreateTriangle(site, edge.VVertexB, edge.VVertexA);
                    }

                    triangle.Fill = fill;

                    Canvas.SetLeft(triangle, MainCanvas.ActualWidth * 0.5);
                    Canvas.SetTop(triangle, MainCanvas.ActualHeight * 0.5);
                    MainCanvas.Children.Add(triangle);
                }
            }

        }

        private Polygon CreateTriangle(VectorF v1, VectorF v2, VectorF v3)
        {
            var poly = new Polygon
            {
                Points = new PointCollection { 
                    new System.Windows.Point(v1[0], v1[1]),
                    new System.Windows.Point(v2[0], v2[1]),
                    new System.Windows.Point(v3[0], v3[1])
                },
                Stroke = Brushes.Black,
                StrokeThickness = 0.125
            };
            
            return poly;
        }
    }
}
