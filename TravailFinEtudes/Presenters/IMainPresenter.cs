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
    interface IMainPresenter
    {


        //Android Activities Lifecycle delegate methods

         void OnCreate();
         void OnResume();
         void OnDestroy();
         void OnPause();
         void OnCommandActivityClick(string ip, string port);
         void OnlistActivityClick();


         Boolean CheckIpAdressFormat(String ip);
         Boolean CreateJsonDirectoryIfNotExists();


    }
}