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
    interface ICommandActivity
    {
        void ShowConnexionStatus(int status);
        void Scan();
        void LoadPath(Path path);
        void LoadPathStats(double[] pathStats);
        void LoadReviewMode();
        void LoadCommandMode();
        void ShowSavingDialog();
        void SetAverageSelection(string[] intervalSelection);
        void FilterOn();
        void FilterOff();
    }
}