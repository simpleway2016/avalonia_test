using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1
{
    internal class MathHelper
    {
        public const double MAXDB = 6;
        public const double MINDB = -90;

        /// <summary>
        /// 根据采用数返回时间
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="sampleRate"></param>
        /// <returns></returns>
        public unsafe static string GetTimeString(ulong samples, int sampleRate)
        {
            if (sampleRate == 0)
                return "";
            var ms = samples * 1000 / (ulong)sampleRate;
            return GetTimeStringForMs(ms);
        }
        public unsafe static string GetTimeStringForMs(ulong ms)
        {
            var seconds = (ms - ms % 1000) / 1000;
            var hours = seconds / 3600;
            seconds = seconds % 3600;
            var minutes = seconds / 60;
            seconds = seconds % 60;
            var millseconds = ms % 1000;
            return $"{hours.ToString().PadLeft(2, '0')}:{minutes.ToString().PadLeft(2, '0')}:{seconds.ToString().PadLeft(2, '0')}.{millseconds.ToString().PadLeft(3, '0')}";
        }
        public static string GetTimeStringForSecond(int seconds)
        {
            int hours = seconds / 3600;
            seconds = seconds % 3600;
            int minutes = seconds / 60;
            seconds = seconds % 60;
            return hours.ToString().PadLeft(2, '0') + ":" + minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        /// <param name="maxHeight">最大音量时，能波动多高</param>
        /// <param name="minDB">最小的db值，如-60，表示大于等于-60db，音量才会开始波动</param>
        /// <param name="maxDB">正向最大的db值，如6，表示超过6db以6db计算</param>
        /// <returns></returns>
        internal static double GetDbByHeight(double height, double maxHeight, double minDB, double maxDB)
        {
            if (height > maxHeight)
                height = maxHeight;
            else if (height < 0)
                height = 0;
            //抛物线公式:y²=2px
            var p = Math.Pow(-minDB + maxDB, 2) / (2 * maxHeight);

            var y平方 = height * (2 * p);
            return Math.Sqrt(y平方) + minDB;
        }
        /// <summary>
        /// 根据db，获取音量波动高度
        /// </summary>
        /// <param name="db"></param>
        /// <param name="maxHeight">最大音量时，能波动多高</param>
        /// <param name="minDB">最小的db值，如-60，表示大于等于-60db，音量才会开始波动</param>
        /// <param name="maxDB">正向最大的db值，如6，表示超过6db以6db计算</param>
        /// <returns></returns>
        internal static double GetControlHeightByDB(double db, double maxHeight, double minDB, double maxDB)
        {
            if (db > maxDB)
                db = maxDB;
            else if (db < minDB)
                db = minDB;
            //抛物线公式:y²=2px
            var p = Math.Pow(-minDB + maxDB, 2) / (2 * maxHeight);
            var y平方 = Math.Pow(db - minDB, 2);
            return y平方 / (2 * p);
        }
        internal static float GetVolumeByDB(double db)
        {
            return (float)Math.Pow(10, db / 20);
        }
        internal static double GetDB(float volumn)
        {
            if (volumn != 0 && volumn < 0.00000001f)
                volumn = 0;
            var db = 20 * Math.Log10(volumn);
            //if (db < -10000)
            //    db = -10000;
            return db;
        }

        /// <summary>
        /// 获取圆上的某个坐标
        /// </summary>
        /// <param name="centerX">圆心</param>
        /// <param name="centerY">圆心</param>
        /// <param name="radius">半径</param>
        /// <param name="angleDegrees">角度，x轴水平（右）那个位置是0度，从0度往上是负数度</param>
        /// <returns></returns>
        public static Point GetPointOnCircle(double centerX, double centerY, double radius, double angleDegrees)
        {
            // 将角度转换为弧度（顺时针方向）
            double angleRadians = angleDegrees * Math.PI / 180.0;

            // 计算圆上的点坐标
            double x = centerX + radius * Math.Cos(angleRadians);
            double y = centerY + radius * Math.Sin(angleRadians);

            return new Point(x, y);
        }

    }
}
