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
using Physics2DDotNet;

namespace Renderers
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CanvasRenderer : UserControl, IDisposable, IRenderer
    {
private readonly DrawingVisual _drawing = new DrawingVisual();
        private WriteableBitmap _wbmp;
        private readonly Rect _fullscreenRect;

        private readonly SolidColorBrush _defaultBrush = new SolidColorBrush(Colors.WhiteSmoke);

        private readonly Color _dynBodyColor = new Color { ScA = 1.0f, ScR = 1.0f, ScG = 1.0f, ScB = 1.0f };
        private readonly Color _statBodyColor = new Color { ScA = 1.0f, ScR = 0.5f, ScG = 0.5f, ScB = 0.5f };

        public CanvasRenderer()
        {
            InitializeComponent();

            _wbmp = BitmapFactory.New(1280, 1024);
            _fullscreenRect = new Rect(0, 0, _wbmp.PixelWidth, _wbmp.PixelHeight);
            RenderingImage.Source = _wbmp;

            Representation.Instance.RegisterRenderer(this, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Dispose()
        {
            Representation.Instance.UnregisterRenderer(this);
        }

        private void DrawBodyPolygon(BitmapContext ctx, Body body)
        {
            var isStatic = body.IgnoresGravity;

            var pts = new int[body.Shape.Vertexes.Count() * 2 + 2];
            
            var pos = body.State.Position;

            int i = 0;

            foreach (var v in body.Shape.Vertexes)
            {
                var vr = Vector2D.Rotate(pos.Angular, v);
                pts[i] = (int)(vr.X + pos.X); pts[i + 1] = (int)(vr.Y + pos.Y);
                i += 2;
            }
            pts[i] = pts[0]; pts[i + 1] = pts[1];

            ctx.WriteableBitmap.FillPolygon(pts, isStatic ? _statBodyColor : _dynBodyColor);            
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
    }
}
