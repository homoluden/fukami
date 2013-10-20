using AdvanceMath;
using CustomBodies;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorldControllers;

namespace Renderers
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CanvasRenderer : UserControl, IDisposable, IRenderer
    {
        DrawingVisual _drawing;
        Brush _defaultBrush = new SolidColorBrush(Colors.WhiteSmoke);

        public CanvasRenderer()
        {
            InitializeComponent();

            _drawing = new DrawingVisual();

            AddVisualChild(_drawing);

            Representation.Instance.RegisterRenderer(this as IRenderer, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Dispose()
        {
            Representation.Instance.UnregisterRenderer(this as IRenderer);
        }

        private StreamGeometry BuildPolygonGeometry(BasePolygonBody body)
        {
            var geom = new StreamGeometry() { FillRule = FillRule.EvenOdd };

            using (var figCtx = geom.Open())
            {
                Vector2D start = body.Drawable.Polygon.Vertices[0];
                figCtx.BeginFigure(new Point(start.X, start.Y), false, true);

                foreach (var vertex in body.Drawable.Polygon.Vertices.Skip(1))
                {
                    figCtx.LineTo(new Point(vertex.X, vertex.Y), false, false);
                }
            }
            geom.Freeze();

            return geom;
        }

        public void RenderWorld(IWorldSnapshot snapshot)
        {
            var op = _drawing.Dispatcher.InvokeAsync(() => {
                var polygonBodies = snapshot.Bodies.OfType<BasePolygonBody>();

                var centerTransform = new TranslateTransform(ActualWidth / 2, ActualHeight / 2);
                _drawing.Transform = centerTransform;

                // Retrieve the DrawingContext in order to create new drawing content.


                using (var drawCtx = _drawing.RenderOpen())
                {

                    foreach (var body in polygonBodies)
                    {
                        var geom = BuildPolygonGeometry(body);

                        drawCtx.DrawGeometry(null, new Pen(_defaultBrush, 2), geom);
                    }

                }
            });

            op.Wait();
        }
    }
}
