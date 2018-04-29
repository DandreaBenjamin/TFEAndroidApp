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

namespace TravailFinEtudes.Views
{

    class ScreenDescriptor
    {
        private static int X = 405;
        private static int Y = 444;

        private RectF[] distanceCircles;
        private RectF demiCercle;
        private float[] angles;

        public RectF[] DistanceCircles { get => distanceCircles; set => distanceCircles = value; }
        public RectF DemiCercle { get => demiCercle; set => demiCercle = value; }
        public float[] Angles { get => angles; set => angles = value; }

        public ScreenDescriptor()
        {
            demiCercle = new RectF();
            demiCercle.Top = 44; demiCercle.Bottom = 844; demiCercle.Left = 5; demiCercle.Right = 805;
            InitDistanceCircles();
            SetAngles();
        }
        

        public void InitDistanceCircles()
        {
            DistanceCircles = new RectF[4];

            DistanceCircles[0] = new RectF(); DistanceCircles[1] = new RectF(); DistanceCircles[2] = new RectF(); DistanceCircles[3] = new RectF();
            DistanceCircles[0].Top = 44; DistanceCircles[0].Right = 805; DistanceCircles[0].Left = 5; DistanceCircles[0].Bottom = 844;
            DistanceCircles[1].Top = 144; DistanceCircles[1].Right = 705; DistanceCircles[1].Left = 105; DistanceCircles[1].Bottom = 744;
            DistanceCircles[2].Top = 244; DistanceCircles[2].Right = 605; DistanceCircles[2].Left = 205; DistanceCircles[2].Bottom = 644;
            DistanceCircles[3].Top = 344; DistanceCircles[3].Right = 505; DistanceCircles[3].Left = 305; DistanceCircles[3].Bottom = 544;
        }

        public void SetAngles()
        {
            Angles = new float[8];

            Angles[0] = (float)(X + (400 * Math.Cos(Math.PI * 30 / 180))); Angles[1] = (float)(Y - (400 * Math.Sin(Math.PI * 30 / 180)));
            Angles[2] = (float)(X + (400 * Math.Cos(Math.PI * 60 / 180))); Angles[3] = (float)(Y - (400 * Math.Sin(Math.PI * 60 / 180)));
            Angles[4] = (float)(X + (400 * Math.Cos(Math.PI * 120 / 180))); Angles[5] = Angles[3];
            Angles[6] = (float)(X + (400 * Math.Cos(Math.PI * 150 / 180))); Angles[7] = Angles[1];
        }



    }
}