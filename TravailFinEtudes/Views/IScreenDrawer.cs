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
    interface IScreenDrawer
    {
        void LoadPath(Path path);
        void LoadPathStats(double[] pathStats);
        void DeletePath();
        void DrawFrame(Canvas canvas);
        void DrawObstacle(Canvas canvas);

    }
}