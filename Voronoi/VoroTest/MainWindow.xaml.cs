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
            _verts = new Vector2[10];

            for (int i = 0; i < 10; i++)
            {
                _verts[i] = new Vector2((float)(300 * (_rnd.NextDouble() - 0.5)), (float)(300 * (_rnd.NextDouble() - 0.5)));
            }

            var graph = Fortune.GenerateGraph(_verts);

            var site = _verts[0];

            var edges = graph.Edges.Where(edge => edge.Left.Equals(site) || edge.Right.Equals(site));
        }
    }
}
