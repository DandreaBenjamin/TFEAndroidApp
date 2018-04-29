﻿using System;
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
using TravailFinEtudes.Presenters;
using Android.Util;
using Newtonsoft.Json;

namespace TravailFinEtudes
{
    [Activity(Label = "ObstacleListActivity")]
    public class ObstacleListActivity : ListActivity, IObstacleListeActivity
    {
        ObstacleListPresenter obstaclleListPresenter;
        Intent toCommandActivity;
        private TextView selectedTextView;
        private string[] objectsToDisplay;
        private string selectedObjectPath;

        public void SetObjectNameToDisplay(string[] objectsToDisplay)
        {
            this.objectsToDisplay = objectsToDisplay;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            obstaclleListPresenter = new ObstacleListPresenter(this);
            obstaclleListPresenter.OnCreate();
            ListAdapter = new ArrayAdapter<String>(this, Resource.Layout.ObstacleList, objectsToDisplay);
            toCommandActivity = new Intent(this, typeof(CommandActivity));
        }


        protected override void OnRestart()
        {
            base.OnRestart();
            obstaclleListPresenter.OnRestart();
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            Log.Debug("Liste Image :", "Je recois un click");
            selectedTextView = (TextView)v;
            string directory = GetString(Resource.String.JSONObjectsDirectory);
            selectedObjectPath = directory + "/" + selectedTextView.Text;
            Log.Debug("Liste Image image path :", selectedObjectPath);

            toCommandActivity.PutExtra("Mode", "Review");
            toCommandActivity.PutExtra("File", selectedObjectPath);
            StartActivity(toCommandActivity);
        }
    }
}