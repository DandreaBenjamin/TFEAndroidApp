using System;
using System.Net;
using TravailFinEtudes.Views;
using System.Net.Sockets;
using Android.Util;
using System.IO;

namespace TravailFinEtudes.Presenters
{
    class MainPresenter : IMainPresenter
    {
        private IMainActivity mainActivity;
        private string baseDirectory;
        private readonly int[] connexionMessagesCodes = { 0, 2130903046, 2130903044, 2130903047, 2130903045, 2130903043 };

        public MainPresenter(IMainActivity mainActivity)
        {
            baseDirectory = Android.App.Application.Context.GetString(Resource.String.JSONObjectsDirectory);
            this.mainActivity = mainActivity;
            CreateJsonDirectoryIfNotExists();
        }

        public bool CheckIpAdressFormat(string ip)
        {
            IPAddress ipAddr;
            return IPAddress.TryParse(ip, out ipAddr);
        }
        
        public void OnCreate()
        {
            throw new NotImplementedException();
        }

        public void OnDestroy()
        {
            throw new NotImplementedException();
        }

        public void OnPause()
        {
            throw new NotImplementedException();
        }

        public void OnResume()
        {
            throw new NotImplementedException();
        }



        public bool CreateJsonDirectoryIfNotExists()
        {
            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }
            return Directory.Exists(baseDirectory);
        }

        public void OnCommandActivityClick(string ip, string port)
        {
            Log.Debug("MainPresenter", "LoadObstacleListActivity");
            string mode = "Command";
            mainActivity.LoadCommandActivity(mode, ip, port);
        }

        public void OnlistActivityClick()
        {
            mainActivity.LoadListActivity();
        }
    }
}