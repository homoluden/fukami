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
        private readonly DrawingVisual _drawing = new DrawingVisual();
        private WriteableBitmap _wbmp;
        private readonly RenderTargetBitmap _rtbmp;

        private readonly Brush _defaultBrush = new SolidColorBrush(Colors.WhiteSmoke);

        public CanvasRenderer()
        {
            InitializeComponent();

            _rtbmp = new RenderTargetBitmap(1024, 768, 96, 96, PixelFormats.Pbgra32);
            _wbmp = BitmapFactory.ConvertToPbgra32Format(_rtbmp);

            RenderingImage.Source = _wbmp;

            Representation.Instance.RegisterRenderer(this, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Dispose()
        {
            Representation.Instance.UnregisterRenderer(this);
        }

        private static StreamGeometry BuildPolygonGeometry(BasePolygonBody body)
        {
            var geom = new StreamGeometry { FillRule = FillRule.EvenOdd };

            using (var figCtx = geom.Open())
            {
                var start = body.Drawable.Polygon.Vertices[0];
                figCtx.BeginFigure(new Point(start.X, start.Y), true, true);

                foreach (var vertex in body.Drawable.Polygon.Vertices.Skip(1))
                {
                    figCtx.LineTo(new Point(vertex.X, vertex.Y), false, true);
                }
            }

            var position = body.State.Position;
            var mtx = body.Transformation;
            geom.Transform = new MatrixTransform(mtx.m00, mtx.m01, mtx.m10, mtx.m11, position.X, position.Y);

            geom.Freeze();

            return geom;
        }

        public void RenderWorld(IWorldSnapshot snapshot)
        {
            var op = _drawing.Dispatcher.InvokeAsync(() => {
                var polygonBodies = snapshot.Bodies.OfType<BasePolygonBody>();

                //var centerTransform = new TranslateTransform(ActualWidth / 2, ActualHeight / 2);
                //_drawing.Transform = centerTransform;

                // Retrieve the DrawingContext in order to create new drawing content.


                using (var drawCtx = _drawing.RenderOpen())
                {

                    foreach (var geom in polygonBodies.Select(BuildPolygonGeometry))
                    {
                        drawCtx.DrawGeometry(_defaultBrush, null, geom);
                    }

                }

                _rtbmp.Clear();
                _rtbmp.Render(_drawing);
                var newFrame = BitmapFactory.ConvertToPbgra32Format(_rtbmp);
                newFrame.Blit(new Point(0,0), _wbmp, new Rect(0,0,1024,768), Color.FromRgb(55,55,55), WriteableBitmapExtensions.BlendMode.Additive);
                _wbmp = newFrame;

                RenderingImage.Source = newFrame;
            });

            op.Wait();
        }
    }
}
