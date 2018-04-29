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
using Android.Graphics;

namespace TravailFinEtudes.Utils
{
    static class PaintManager
    {
            static private Dictionary<String, Paint> paintsDictionary = new Dictionary<string, Paint>();

            static PaintManager()
            {
                InitPaints();
            }


            private static void InitPaints()
            {
                Paint framePaint = new Paint();
                Paint circlePaint = new Paint();
                Paint pathPaint = new Paint();
                Paint statPaint = new Paint();
                Paint obstacle = new Paint();

                framePaint.SetStyle(Paint.Style.Stroke);
                framePaint.SetARGB(255, 0, 0, 0);
                framePaint.StrokeWidth = 1;


                pathPaint.SetStyle(Paint.Style.Stroke);
                pathPaint.SetARGB(255, 255, 0, 0);
                pathPaint.StrokeWidth = 1;


                circlePaint.SetStyle(Paint.Style.Fill);
                circlePaint.Color = Color.DodgerBlue;
                circlePaint.StrokeWidth = 1;

                statPaint.SetStyle(Paint.Style.FillAndStroke);
                statPaint.Color = Color.OrangeRed;

                obstacle.SetStyle(Paint.Style.Stroke);
                obstacle.StrokeWidth = 1;
                obstacle.Color = Color.AliceBlue;

                paintsDictionary.Add("Frame", framePaint);
                paintsDictionary.Add("Circle", circlePaint);
                paintsDictionary.Add("Path", pathPaint);
                paintsDictionary.Add("Stat", statPaint);
                paintsDictionary.Add("Obstacle", obstacle);

            }

            public static Paint GetPainter(String name)
            {
                return paintsDictionary[name];
            }
    }
   
}