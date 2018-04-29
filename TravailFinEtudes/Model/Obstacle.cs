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
using TravailFinEtudes.Utils;

namespace TravailFinEtudes.Model
{
    class Obstacle
    {
        public int[] rawDistances;
        public double min, max, average;
        public float[] rawCoordinates;
        public double[] stats;
        /*
        public int[] RawDistances { get => rawDistances; set => rawDistances = value; }
        public double Min { get => min; set => min = value; }
        public double Max { get => max; set => max = value; }
        public double Average { get => average; set => average = value; }
        public float[] RawCoordinates { get => rawCoordinates; set => rawCoordinates = value; }
        public double[] Stats { get => stats; set => stats = value; }
        */
        public Obstacle()
        {
        }

        public Obstacle(int[] rawDistances,float[] rawCoordinates, double[] stats)
        {
            this.rawCoordinates = rawCoordinates;
            this.rawDistances = rawDistances;
            this.stats = stats;
            this.min = stats[0];
            this.max = stats[1];
            this.average = stats[2];
        }

        /*
        public Obstacle(byte[] distances)
        {
            this.rawDistances = MathUtil.AddDistances(distances);
            double[] stats = MathUtil.FindStatistics(this.rawDistances);
            this.stats = stats;
            this.min = stats[0];
            this.max = stats[1];
            this.average = stats[2];
            rawCoordinates = MathUtil.FindCoordinates(this.rawDistances, 405, 444, 0);
        }*/
    }
}