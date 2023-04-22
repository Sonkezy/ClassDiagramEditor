using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media.Immutable;
using Avalonia.Media;
using DynamicData.Binding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagramEditor.Models
{
    public class ArrowConstructor : AbstractNotifyPropertyChanged
    {
        Point startPoint;
        Point endPoint;
        Path arrow;
        int type;
        double diag;
        readonly static int arrow_width = 20;
        readonly static int arrow_height = 10;
        readonly static int rod_step = 5;
        public Point StartPoint
        {
            get => startPoint;
            set => this.SetAndRaise(ref startPoint, value);
        }
        public Point EndPoint
        {
            get => endPoint;
            set => SetAndRaise(ref endPoint, value);
        }
        public int Type
        {
            get => type;
            set => SetAndRaise(ref type, value);
        }
        public Path Arrow
        {
            get => arrow;
            set=> SetAndRaise(ref arrow, value);
        }
        public ArrowConstructor()
        {
            Arrow = new Path() { Tag = "arrow", ZIndex = 3, IsVisible = false };
            Arrow.Stroke = Brush.Parse("Black");
            Arrow.StrokeThickness = 3;
            Arrow.Fill = Brush.Parse("LightGray");
            Draw(new Point(0,0), new Point(0,0)); 
        }

        void UpdatePath()
        {

            int rod = type switch
            {
                0 or 1 => (int)diag - arrow_width,
                3 or 4 => (int)diag - arrow_width * 2,
                2 or 5 or _ => (int)diag,
            };
            bool R = true;

            StringBuilder sb = new();
            sb.Append($"M {(int)StartPoint.X},{(int)StartPoint.Y}");
            if (type == 1 || type == 2)
                while (rod > 0)
                {
                    int dist = rod > rod_step ? rod_step : rod;
                    rod -= dist;
                    sb.Append(R ? " l" : " m");
                    sb.Append($" {dist},0");
                    R = !R;
                }
            else sb.Append($" l {rod},0");

            int w = arrow_width, h = arrow_height;
            var head = type switch
            {
                0 or 1 => $" l 0,-{h} {w},{h} -{w},{h} 0,-{h}",
                3 or 4 => $" l {w},-{h} {w},{h} -{w},{h} -{w},-{h}",
                2 or 5 or _ => $" m -{w},-{h} l {w},{h} m -{w},{h} l {w},-{h}",
            };
            sb.Append(head);

            var c = Arrow.Fill switch
            {
                ImmutableSolidColorBrush t => t.Color,
                SolidColorBrush t => t.Color,
                _ => new Color(),
            };
            Arrow.Fill = new SolidColorBrush(new Color((byte)(type == 3 ? 0 : 255), c.R, c.G, c.B));
            Arrow.Data = Geometry.Parse(sb.ToString());
        }
        public void Update()
        {
            var delta = StartPoint - EndPoint;
            double new_diag = Math.Sqrt(Math.Pow(delta.X, 2) + Math.Pow(delta.Y, 2));
            double orig_diag = new_diag > 0 ? new_diag : 0.001;
            if (new_diag < arrow_width * 1.5) new_diag = arrow_width * 1.5;
            diag = new_diag;

            UpdatePath();

            double angle = Math.Acos(delta.X / orig_diag);
            angle = angle * 180 / Math.PI;
            if (delta.Y < 0) angle = 360 - angle;
            angle = (angle + 180) % 360;
            Arrow.RenderTransform = new RotateTransform(angle, (StartPoint.X - diag) / 2, (StartPoint.Y - arrow_height) / 2);
        }
        public void Draw(Point start, Point end, int type = 0)
        {
            StartPoint = start;
            EndPoint = end;
            Type = type;
            Update();
        }
    }
}
