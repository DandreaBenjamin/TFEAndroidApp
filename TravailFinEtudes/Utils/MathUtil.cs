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
using TravailFinEtudes.Model;
using Android.Util;

namespace TravailFinEtudes.Utils
{
    static class MathUtil
    {
        public static float[] ProcessCoordinates(int[] distances, int interval)
        {
            int beginAngle = (int)((interval / 2) - 0.5);

            if (interval != 0)
            {
                int[] newDistances = SmoothedAverage(distances, interval);
            }
            
            float[] coordinates = processCoordinates(distances, beginAngle);
            return coordinates;
        }
        


        public static float[] processCoordinates(int[] distances,int angle)
        {
            //Le nouveau tableau est deux fois plus grand car chaque distances va être décomposée
            //en coordonnée X et Y
            int X = 405, Y = 444;
            int sizeCoordinatesArray = distances.Length * 2;

            float[] coordinates = new float[sizeCoordinatesArray];

            int index = 0;

            for (int pos = 0; pos < distances.Length; pos++)
            {
                coordinates[index] = (float)(X + (distances[pos] * Math.Cos(Math.PI * angle / 180)));
                index++;
                coordinates[index] = (float)(Y - (distances[pos] * Math.Sin(Math.PI * angle / 180)));
                index++;
                angle++;
            }
            return coordinates;
        }

        public static void FindCoordinates(int[] distances, float X, float Y, int angle, out float[] coordinates)
        {
            //Le nouveau tableau est deux fois plus grand car chaque distances va être décomposée
            //en coordonnée X et Y

            int sizeCoordinatesArray = distances.Length * 2;

            coordinates = new float[sizeCoordinatesArray];

            int index = 0;

            for (int pos = 0; pos < distances.Length; pos++)
            {
                coordinates[index] = (float)(X + (distances[pos] * Math.Cos(Math.PI * angle / 180)));
                index++;
                coordinates[index] = (float)(Y - (distances[pos] * Math.Sin(Math.PI * angle / 180)));
                index++;
                angle++;
            }
        }


        public static Path GetPathFromObstacleCoordinates(float[] coordinates)
        {
            Path pathObstacle = new Path();
            pathObstacle.MoveTo(coordinates[0], coordinates[1]);
            for (int k = 2; k < coordinates.Length; k += 2)
            {
                pathObstacle.LineTo(coordinates[k], coordinates[k + 1]);
            }
            return pathObstacle;
        }

        /*Tableau de 360 distances initiales + tableau par référence pour contenir
          les distances additionnées
        */
        public static void AddDistances(byte[] distances, out int[] realDistancesArray)
        {
            int length = distances.Length / 2;
            realDistancesArray = new int[length];
            int realArrayPos = 0;
            for (int k = 0; k < distances.Length; k += 2)
            {
                int sum = distances[k] + distances[k + 1];
                realDistancesArray[realArrayPos] = (sum <= 400) ? sum : 400;
                realArrayPos++;
            }
        } 

        public static int[] SmoothedAverage(int[] rawDistances, int interval)
        {
            int size = rawDistances.Length - (interval - 1);

            int[] filteredDistances = new int[size];
            
            int beginingInOldArray = (int)((interval / 2.0) - 0.5);

            int step = beginingInOldArray;

            int filteredPos = 0;

            int tmp = 0;

            for (; beginingInOldArray <= size; beginingInOldArray++)
            {
                for (int k = -step; k <= step; k++)
                {
                    tmp += rawDistances[beginingInOldArray + k];
                }
                filteredDistances[filteredPos] = (int)(tmp / interval);
              
                tmp = 0;
              
                filteredPos++;
            }
            return filteredDistances;
        }

        public static float[] ProcessNewCoordinates(int[] rawDistances, int interval)
        {
            int[] distances = SmoothedAverage(rawDistances, interval);
            int angle = (int)((interval / 2) - 0.5);
            float[] coordinates = processCoordinates(distances, angle);
            return coordinates;
        }
 
        public static double[] FindStatistics(int[] realDistances) => new double[] { realDistances.Min(), realDistances.Max(), realDistances.Average() };

        public static void FindStats(int[] realDistances, out double[] stats)
        {
            stats = new double[3];
            stats[0] = realDistances.Min();
            stats[1] = realDistances.Max();
            stats[2] = realDistances.Average();
        }
    }
}