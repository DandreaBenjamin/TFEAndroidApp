using Android.App;
using Android.Widget;
using Android.OS;
using TravailFinEtudes.Views;
using TravailFinEtudes.Presenters;
using Android.Content;
using Newtonsoft.Json;
using Android.Util;
using System.Net.Sockets;

namespace TravailFinEtudes
{
    [Activity(Label = "TravailFinEtudes", MainLauncher = true)]
    public class MainActivity : Activity, IMainActivity
    {
        private MainPresenter mainPresenter;
        private Button listActivityButton, commandActivityButton;
        private CheckBox defaultConnexionParameters;
        private EditText ipAddress, port;
        private Intent toCommandActivity;
        private Intent toObstacleListActivity;
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            toCommandActivity = new Intent(this, typeof(CommandActivity));
            toObstacleListActivity = new Intent(this, typeof(ObstacleListActivity));
            mainPresenter = new MainPresenter(this);
            SetComponents();
            SetComponentsActions();
        }

        public void SetComponents()
        {
            listActivityButton = (Button)FindViewById(Resource.Id.buttonList);
            commandActivityButton = (Button)FindViewById(Resource.Id.buttonStart);
            defaultConnexionParameters = (CheckBox)FindViewById(Resource.Id.checkBoxParam);
            ipAddress = (EditText)FindViewById(Resource.Id.editTextIp);
            port = (EditText)FindViewById(Resource.Id.editTextPort);
        }


        public void SetComponentsActions()
        {
            defaultConnexionParameters.Click += delegate
            {
                if(defaultConnexionParameters.Checked)
                {
                    ipAddress.Text = GetString(Resource.String.DefaultIpAdress);
                    port.Text = GetString(Resource.String.DefaultConnexionPort);
                    ipAddress.Enabled = false;
                    port.Enabled = false;
                }
                else
                {
                    ipAddress.Enabled = true;
                    port.Enabled = true;
                }
            };

            listActivityButton.Click += delegate
            {
                Log.Debug("MainActivity", "Press ListActivityButton");
                mainPresenter.OnlistActivityClick();
            };


            commandActivityButton.Click += delegate
            {
                Log.Debug("MainActivity", "Press CommandActivityButton");
                mainPresenter.OnCommandActivityClick(ipAddress.Text, port.Text);
            };
        }

        public void LoadListActivity()
        {
            Log.Debug("MainActivity", "LoadObstacleListActivity");
            StartActivity(toObstacleListActivity);
        }

        public void LoadCommandActivity(string mode, string ip, string port)
        {
            Log.Debug("MainActivity", "LoadCommandActivity");
            toCommandActivity.PutExtra("Mode", mode);
            toCommandActivity.PutExtra("IP", ip);
            toCommandActivity.PutExtra("Port", port);
            StartActivity(toCommandActivity);
        }
    }
}

