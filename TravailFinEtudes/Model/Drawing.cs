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
    class Drawing
    {
        public int[] rawDistances;
        public double min, max, average;
        public float[] rawCoordinates;
        public double[] stats;
       
        public Drawing()
        {
            
        }

        public Drawing(int[] rawDistances,float[] rawCoordinates, double[] stats)
        {
            this.rawCoordinates = rawCoordinates;
            this.rawDistances = rawDistances;
            this.stats = stats;
            this.min = stats[0];
            this.max = stats[1];
            this.average = stats[2];
        }    
    }
}