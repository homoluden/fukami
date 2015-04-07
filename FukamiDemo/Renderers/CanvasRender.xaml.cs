using System.Linq;
using Drawables;
using Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework;
using WorldControllers;
using Color = System.Windows.Media.Color;

namespace Renderers
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CanvasRenderer : IDisposable, IRenderer
    {
private readonly DrawingVisual _drawing = new DrawingVisual();
        private readonly WriteableBitmap _wbmp;
        private readonly Vector2 _worldCenterOffset = new Vector2(64f, 51.2f);
        
        private readonly Color _dynBodyColor = new Color { ScA = 1.0f, ScR = 1.0f, ScG = 1.0f, ScB = 1.0f };
        private readonly Color _statBodyColor = new Color { ScA = 1.0f, ScR = 0.5f, ScG = 0.5f, ScB = 0.5f };

        public CanvasRenderer()
        {
            InitializeComponent();

            _wbmp = BitmapFactory.New(1280, 1024);
            RenderingImage.Source = _wbmp;

            Representation.Instance.RegisterRenderer(this, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Dispose()
        {
            Representation.Instance.UnregisterRenderer(this);
        }

        private void DrawBodyPolygon(BitmapContext ctx, Body body)
        {
            var isStatic = body.IsStatic;

            body.FixtureList.ForEach(f =>
            {
                var shape = f.Shape;

                ColoredPolygon polygon = null;
                var pos = body.Position + _worldCenterOffset;
                var rot = body.Rotation;

                var baseColor = isStatic ? _statBodyColor : _dynBodyColor;
                var border = new Vector4(baseColor.R * 0.003921f, baseColor.G * 0.003921f, baseColor.B * 0.003921f, 1.0f);
                var fill = new Vector4(border.X, border.Y, border.Z, 0.2f);

                if (shape.ShapeType == ShapeType.Circle)
                {
                    var circle = (CircleShape)shape;
                    var circleSpec = new CircleSpec
                    {
                        Radius = circle.Radius,
                        SegmentsCount = 16,
                        Border = border,
                        Fill = fill
                    };

                    pos += circle.Position;

                    polygon = DrawableFactory.GetOrCreateCircleDrawable(circleSpec);
                }
                else if (shape.ShapeType == ShapeType.Polygon)
                {
                    var rectangle = (PolygonShape)shape;
                    var rectangleSpec = new PolygonSpec
                    {
                        Vertices = rectangle.Vertices,
                        Border = border,
                        Fill = fill
                    };

                    polygon = DrawableFactory.GetOrCreateColoredPolygonDrawable(rectangleSpec);
                }

                if (polygon != null)
                {
                    var pts = new int[polygon.Vertices.Count * 2 + 2];
            
                    int i = 0;

                    foreach (var vr in polygon.Vertices.Select(v => v.Rotate(rot)))
                    {
                        pts[i] = WorldToScreenCoordinates(vr.X + pos.X); pts[i + 1] = WorldToScreenCoordinates(vr.Y + pos.Y);
                        i += 2;
                    }
                    pts[i] = pts[0]; pts[i + 1] = pts[1];

                    var fillColor = Color.FromArgb((byte)(fill.W*255), (byte)(fill.X*255), (byte)(fill.Y*255), (byte)(fill.Z*255));
                    ctx.WriteableBitmap.FillPolygon(pts, fillColor);

                    var borderColor = Color.FromArgb((byte)(border.W * 255), (byte)(border.X * 255), (byte)(border.Y * 255), (byte)(border.Z * 255));
                    ctx.WriteableBitmap.DrawPolyline(pts, borderColor); 
                }
            


            });

          
        }

        private void RenderWorldInternal(IWorldSnapshot snapshot)
        {
            var bodies = snapshot.Bodies;

            using (var ctx = _wbmp.GetBitmapContext())
            {
                ctx.Clear();
                foreach (var body in bodies)
                {
                    DrawBodyPolygon(ctx, body);
                }
            }
        }

        public void RenderWorld(IWorldSnapshot snapshot)
        {
            var op = _drawing.Dispatcher.InvokeAsync(() => RenderWorldInternal(snapshot));

            if (!op.Task.IsCanceled)
            {
                op.Wait();
            }
        }

        public int WorldToScreenCoordinates(float worldCoords)
        {
            return (int)Math.Round(worldCoords*10);
        }
    }
}
