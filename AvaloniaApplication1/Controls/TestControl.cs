using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AvaloniaApplication1.Controls
{
    public class TestControl : UserControl
    {
        int _currentFps;
        private int _frameCount = 0;
        public int Fps => _currentFps;
        private readonly Stopwatch _stopwatch = new();
        public TestControl()
        {
            // 计时开始
            _stopwatch.Start();
        }
        static Typeface TextTypeface = new Typeface("Arial");
        static Pen Pen = new Pen(new SolidColorBrush(Color.Parse("#33ffffff")), 1);
        public override void Render(DrawingContext dc)
        {
            var width =  this.Bounds.Width;

            var height =  this.Bounds.Height;
            dc.FillRectangle(Brushes.Transparent, new Rect(0, 0, width, height));//保证范围内能够相应鼠标事件
            int x = 0;

          
            while (true)
            {
                x += 10;
                dc.DrawLine(Pen, new Point(x, 0), new Point(x, height - 1));


                if (x >= width)
                    break;
            }

            _frameCount++;

            // 每隔1秒计算一次并输出
            if (_stopwatch.ElapsedMilliseconds >= 1000)
            {
                _currentFps = (int)(_frameCount * 1000.0 / _stopwatch.ElapsedMilliseconds);

                // 重置计数器
                _frameCount = 0;
                _stopwatch.Restart();
            }

            FormattedText text = new FormattedText(_currentFps.ToString(), CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight, TextTypeface, 12, Brushes.White);
            dc.DrawText(text, new Point(10, 10));
        }
    }
}
