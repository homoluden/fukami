using SteeleSky.Voronoi;
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

namespace VoroTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Random _rnd = new System.Random(DateTime.Now.Millisecond);
        Vector2[] _verts;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _verts = new Vector2[100];

            for (int i = 0; i < 100; i++)
            {
                _verts[i] = new Vector2((float)(300 * (_rnd.NextDouble() - 0.5)), (float)(300 * (_rnd.NextDouble() - 0.5)));
            }

            var graph = Fortune.GenerateGraph(_verts);

            var cells = _verts.Select(v =>
            {
                var edges = graph.Edges.Where(edge => edge.Left.Equals(v) || edge.Right.Equals(v)).ToArray();

                var cell = new Tuple<Vector2, IEnumerable<VoronoiEdge>, bool>(v,
                                                                              edges,
                                                                              edges.All(ed =>
                                                                                !ed.VVertexA.Equals(Fortune.VVUnkown) && !ed.VVertexB.Equals(Fortune.VVUnkown)));

                return cell;
            }).Where(c => c.Item3).ToArray();

            


        }
    }
}
