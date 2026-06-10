using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Views
{
    /// <summary>
    /// 播放指针
    /// </summary>
    public class PlayPointer : UserControl , ICustomHitTest
    {
        static Brush BgBrush = new SolidColorBrush(Color.Parse("#d5d5d5"));
        static Pen BorderPen = new Pen(new SolidColorBrush(Color.Parse("#4a3d0d")), 1);
        static Brush InsideBgBrush = new SolidColorBrush(Color.Parse("#6e6e70"));
        static Brush InsideEnableBgBrush = new SolidColorBrush(Color.Parse("#e0ae00"));
        static Pen InsideBorderPen = new Pen(new SolidColorBrush(Color.Parse("#4c524b")), 1);
        static Pen InsideMiddlePen = new Pen(new SolidColorBrush(Color.Parse("#929294")), 1);
        public static readonly StyledProperty<double> HeaderHeightProperty =
       AvaloniaProperty.Register<PlayPointer, double>(nameof(HeaderHeight), 25.0);

        public double HeaderHeight
        {
            get
            {
                return GetValue(HeaderHeightProperty);
            }
            set
            {
                SetValue(HeaderHeightProperty, value);
            }
        }
        public static PlayPointer Instance;
        public PlayPointer()
        {
            Instance = this;
        }

      

        public unsafe override void Render(DrawingContext dc)
        {
         
            var width = Bounds.Width;
            var height = Bounds.Height;
            var headerHeight = this.HeaderHeight;
            const double centerLineWidth = 1;
            const double insidePadding = 2;//里面灰色块和外边框距离

            var rectwidth = width - 3 - 2 + 1;

            var geometry = new StreamGeometry();
            double y2 = 0;
            double y1 = 0;
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(2,5), true);
                context.QuadraticBezierTo(new Point(2, 2), new Point(5, 2));
                context.LineTo(new Point(width - 3 - 3, 2));
                context.QuadraticBezierTo(new Point(width - 3, 2), new Point(width - 3, 5));

                var y0 = headerHeight / 7;
                y1 = 2 + y0 * 5;
                y2 = y1 + y0 * 2;
                context.LineTo(new Point(width - 3, y1 - 1));
                context.QuadraticBezierTo(new Point(width - 3, y1), new Point(width - 3 - 1, y1 + 1));

                var x0 = width - 3 - rectwidth / 2 + centerLineWidth / 2 + 1;

                context.LineTo(new Point(x0 + 1, y2 - 1));
                context.QuadraticBezierTo(new Point(x0, y2), new Point(x0, y2 + 1));
                
                context.LineTo(new Point(x0, height + 2));
                context.LineTo(new Point(x0 - centerLineWidth - 1, height + 2));

                context.LineTo(new Point(x0 - centerLineWidth - 1, y2 + 1));
                context.QuadraticBezierTo(new Point(x0 - centerLineWidth - 1, y2), new Point(x0 - centerLineWidth - 1 - 1, y2 - 1));

                context.LineTo(new Point(2 + 1, y1 + 1));
                context.QuadraticBezierTo(new Point(2, y1), new Point(2, y1 - 1));
                context.LineTo(new Point(2, 5));
            }
            dc.DrawGeometry(BgBrush, BorderPen, geometry);

            var insideRectWidth = width - 3 - insidePadding - (2 + insidePadding) + 1;
            //里面灰色块（左边）
            geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
              
                context.BeginFigure(new Point(2 + insidePadding + insideRectWidth / 2 - 1, y2 - insidePadding), true);
             
                context.LineTo(new Point(2 + insidePadding, y1 - insidePadding/2));
                context.LineTo(new Point(2 + insidePadding, 4 + insidePadding));

                context.QuadraticBezierTo(new Point(2 + insidePadding, 2 + insidePadding), new Point(4 + insidePadding, 2 + insidePadding));

                context.LineTo(new Point(2 + insidePadding + insideRectWidth / 2 - 1, 2 + insidePadding));
            }
            dc.DrawGeometry(  InsideEnableBgBrush , InsideBorderPen, geometry);

            //里面灰色块（右边）
            geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(2 + insidePadding + insideRectWidth / 2, y2 - insidePadding), true);

                context.LineTo(new Point(width - 3 - insidePadding, y1 - insidePadding / 2));

                context.LineTo(new Point(width - 3 - insidePadding,4 + insidePadding));

                context.QuadraticBezierTo(new Point(width - 3 - insidePadding, 2 + insidePadding), new Point(width - 3 - 2 - insidePadding, 2 + insidePadding));

                context.LineTo(new Point(2 + insidePadding + insideRectWidth / 2, 2 + insidePadding));
            }
            dc.DrawGeometry( InsideEnableBgBrush  , InsideBorderPen, geometry);

            dc.DrawLine(InsideMiddlePen, new Point(2 + insidePadding + insideRectWidth / 2 - 0.5, 2 + insidePadding),
                new Point(2 + insidePadding + insideRectWidth / 2 - 0.5, y2 - insidePadding / 2 - 1));
        }

        public bool HitTest(Point point)
        {
            return point.Y < this.HeaderHeight;
        }
    }
}
