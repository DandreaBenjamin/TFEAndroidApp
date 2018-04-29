using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Graphics;
using TravailFinEtudes.Utils;

namespace TravailFinEtudes.Views
{
    class ScreenDrawer:View, IScreenDrawer
    {
        ScreenDescriptor screen;
        Path obstacle = null;
        Boolean scanDone;
        double min, max, average;
        double minDisplay, maxDisplay, averageDisplay;

        public ScreenDrawer(Context context) : base(context)
        {
            this.screen = new ScreenDescriptor();
        }


        public ScreenDrawer(Context context, IAttributeSet attr) : base(context, attr)
        {
            this.screen = new ScreenDescriptor();
        }

        
        protected override void OnDraw(Canvas mapCanvas)
        {
            base.OnDraw(mapCanvas);
            DrawFrame(mapCanvas);

            if(scanDone)
            {
                DrawObstacle(mapCanvas);
            }     
        }

        public void DrawFrame(Canvas mapCanvas)
        {
            mapCanvas.DrawColor(Color.MistyRose);
            mapCanvas.DrawArc(screen.DemiCercle, 180, 180, false, PaintManager.GetPainter("Circle"));
            mapCanvas.DrawLine(5, 444, 805, 444, PaintManager.GetPainter("Frame"));
            foreach (RectF arc in screen.DistanceCircles)
            {
                mapCanvas.DrawArc(arc, 180, 180, false, PaintManager.GetPainter("Frame"));
            }

            mapCanvas.DrawLine(405, 444, 405, 44, PaintManager.GetPainter("Frame"));
            mapCanvas.DrawLine(405, 444, screen.Angles[0], screen.Angles[1], PaintManager.GetPainter("Frame"));
            mapCanvas.DrawLine(405, 444, screen.Angles[2], screen.Angles[3], PaintManager.GetPainter("Frame"));
            mapCanvas.DrawLine(405, 444, screen.Angles[4], screen.Angles[5], PaintManager.GetPainter("Frame"));
            mapCanvas.DrawLine(405, 444, screen.Angles[6], screen.Angles[7], PaintManager.GetPainter("Frame"));
        }

        public void DrawObstacle(Canvas mapCanvas)
        {
            mapCanvas.DrawPath(obstacle, PaintManager.GetPainter("Obstacle"));
            mapCanvas.DrawText("Max :" + maxDisplay, 225, 25, PaintManager.GetPainter("Stat"));
            mapCanvas.DrawText("Min :" + minDisplay, 325, 25, PaintManager.GetPainter("Stat"));
            mapCanvas.DrawText("Moyenne :" + averageDisplay, 425, 25, PaintManager.GetPainter("Stat"));
        }

        public void LoadPath(Path path)
        {
            this.obstacle = path;
            scanDone = true;
            Invalidate();
        }

        public void DeletePath()
        {
            this.obstacle = null;
            scanDone = false;
            Invalidate();
        }

        public void LoadPathStats(float[] pathStats)
        {
            min = pathStats[0];
            max = pathStats[1];
            average = pathStats[2];
        }

        public void LoadPathStats(double[] pathStats)
        {
            minDisplay = pathStats[0];
            maxDisplay = pathStats[1];
            averageDisplay = pathStats[2];
        }
    }
}