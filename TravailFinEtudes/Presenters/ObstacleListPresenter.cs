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
using TravailFinEtudes.Views;
using System.IO;

namespace TravailFinEtudes.Presenters
{
    class ObstacleListPresenter : IObstacleListPresenter
    {
        private IObstacleListeActivity iObstacleListeActivity;
        private IEnumerable<String> listObstacle;
        private String pathDirectory;
        
        public ObstacleListPresenter(IObstacleListeActivity iObstacleListeActivity)
        {
            this.iObstacleListeActivity = iObstacleListeActivity;
            this.pathDirectory = Android.App.Application.Context.GetString(Resource.String.JSONObjectsDirectory);
        }

        public void OnCreate()
        {
            listObstacle = Directory.EnumerateFiles(pathDirectory);
            GetObstacleNamesArray();
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
            //listObstacle = Directory.EnumerateFiles(pathDirectory);
        }

        public void OnRestart()
        {
            listObstacle = Directory.EnumerateFiles(pathDirectory);
            GetObstacleNamesArray();
        }

        public void GetObstacleNamesArray()
        {
            String[] arrayAdapter = listObstacle.ToArray();

            int position = 0;

            int nbrToRemove = pathDirectory.Length;

            while (position < arrayAdapter.Length)
            {
                arrayAdapter[position] = arrayAdapter[position].Remove(0, nbrToRemove);
                position++;
            }
            iObstacleListeActivity.SetObjectNameToDisplay(arrayAdapter);
        }


        public void LoadSelectedObstacle()
        {
            
        }

        public void Load()
        {
            
        }
    }
}