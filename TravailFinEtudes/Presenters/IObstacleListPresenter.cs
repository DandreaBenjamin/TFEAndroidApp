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

namespace TravailFinEtudes.Presenters
{
    interface IObstacleListPresenter
    {
        void LoadSelectedObstacle();
        void Load();
        void GetObstacleNamesArray();
        void OnCreate();
        void OnResume();
        void OnDestroy();
        void OnPause();
        void OnRestart();
    }
}