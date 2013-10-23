using AdvanceMath;
using AdvanceMath.Geometry2D;
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
using AdvanceMath.Geometry2D;

namespace Renderers
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CanvasRenderer : UserControl, IDisposable, IRenderer
    {
        private readonly DrawingVisual _drawing = new DrawingVisual();
        private WriteableBitmap _wbmp;
        private Rect _fullscreenRect;

        private readonly SolidColorBrush _defaultBrush = new SolidColorBrush(Colors.WhiteSmoke);

        public CanvasRenderer()
        {
            InitializeComponent();

            _wbmp = BitmapFactory.New(1024, 768);
            _fullscreenRect = new Rect(0, 0, _wbmp.PixelWidth, _wbmp.PixelHeight);
            RenderingImage.Source = _wbmp;

            Representation.Instance.RegisterRenderer(this, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Dispose()
        {
            Representation.Instance.UnregisterRenderer(this);
        }

        private static StreamGeometry BuildPolygonGeometry(BasePolygonBody body)
        {
            var geom = new StreamGeometry { FillRule = FillRule.Nonzero };

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

        private void DrawBodyPolygon(BitmapContext ctx, BasePolygonBody body)
        {
            var pts = new int[body.Shape.Vertexes.Count() * 2 + 2];
            var mtx = Matrix2x3.Identity;
            BoundingRectangle rect; // = body.Rectangle;
            body.Shape.CalcBoundingRectangle(ref mtx, out rect);
            var width = rect.Max.X - rect.Min.X;
            var height = rect.Max.Y - rect.Min.Y;
            var cx = width / 2;
            var cy = height / 2;

            var bodyBmp = BitmapFactory.New((int)width + 1, (int)height + 1);

            var pos = body.State.Position;

            using (var bodyCtx = bodyBmp.GetBitmapContext())
            {

                int i = 0;
                foreach (var v in body.Drawable.Polygon.Vertices)
                {
                    pts[i] = (int)(v.X + cx); pts[i + 1] = (int)(v.Y + cy);
                    i += 2;
                }
                pts[i] = pts[0]; pts[i + 1] = pts[1];

                bodyBmp.FillPolygon(pts, new Color { ScA = 1.0f, ScR = 1.0f, ScG = 1.0f, ScB = 1.0f });

                bodyBmp = bodyBmp.RotateFree(MathHelper.ToDegrees(pos.Angular), false);

            }

            width = bodyBmp.PixelWidth;
            height = bodyBmp.PixelHeight;
            cx = width / 2;
            cy = height / 2;

            ctx.WriteableBitmap.Blit(new Rect(pos.X - cx, pos.Y - cy, width, height), bodyBmp, new Rect(0, 0, width, height));
        }

        public void RenderWorld(IWorldSnapshot snapshot)
        {
            var op = _drawing.Dispatcher.InvokeAsync(() => {
                var polygonBodies = snapshot.Bodies.OfType<BasePolygonBody>();

                var newFrame = BitmapFactory.New(_wbmp.PixelWidth, _wbmp.PixelHeight);

                using (var ctx = newFrame.GetBitmapContext())
                {
                    foreach (var body in polygonBodies)
                    {
                        DrawBodyPolygon(ctx, body);
                    }
                }

                newFrame.Blit(_fullscreenRect, _wbmp, _fullscreenRect, Color.FromRgb(99, 99, 99), WriteableBitmapExtensions.BlendMode.Additive);
                RenderingImage.Source = newFrame;
            });

            if (!op.Task.IsCanceled)
            {
                op.Wait();
            }
        }
    }
}
