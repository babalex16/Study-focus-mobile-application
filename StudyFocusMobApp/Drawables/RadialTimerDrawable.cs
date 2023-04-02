using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyFocusMobApp.Drawables
{
    public class RadialTimerDrawable : IDrawable
    {
        public int Seconds { get; set; }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DateTime curTime = DateTime.Now;
            var clockCenterPoint = new PointF(dirtyRect.Width/2, dirtyRect.Height/2);
            var circleRadius = 100;

            canvas.StrokeColor = Colors.Black;
            canvas.FillColor = Color.FromArgb("#ece08e");
            canvas.FillCircle(clockCenterPoint, circleRadius);

            canvas.StrokeSize = 2;
            var secondPoint = GetSecondHand(curTime, circleRadius, clockCenterPoint);
            canvas.DrawLine(clockCenterPoint, secondPoint);
        }
        

        internal static PointF GetSecondHand(DateTime curTime, int radius, PointF center)
        {

            int currentSecond = curTime.Second;

            var angleDegrees = (currentSecond * 360) / 60;
            var angle = (Math.PI / 180.0) * angleDegrees;

            PointF outerPoint = new((float)(radius * Math.Sin(angle) + center.X), (float)(-radius * Math.Cos(angle)) + center.Y);

            return outerPoint;
        }
    }
}
