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
using System.Net.Sockets;
using Newtonsoft.Json;
using TravailFinEtudes.Presenters;
using TravailFinEtudes.Model;
using Android.Graphics;
using Android.Util;
using TravailFinEtudes.Utils;
using Android.Hardware;

namespace TravailFinEtudes
{
    [Activity(Label = "CommandActivity")]
    public class CommandActivity : Activity, ICommandActivity, ISensorEventListener
    {
        private IScreenDrawer screenDrawer;
        private ICommandPresenter commandPresenter;
        private SensorManager sensorManager;
        private Sensor accelerometer;
        private Button scan, drive, saveFile, deleteDraw;
        private NumberPicker averagePicker;
        private TextView useMode;
        private Button filter;
        private AlertDialog.Builder alertSaveDialogBuilder;
        private AlertDialog alertSaveDialog;
        private EditText fileName;
        private LinearLayout commandsLayout;
        private string ipAdrress;
        private string port;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Command);
            screenDrawer = (ScreenDrawer)FindViewById(Resource.Id.screenDrawer);
            string mode = Intent.GetStringExtra("Mode");
            Console.WriteLine("CommandActivity setting mode:" + mode);
            SetComponents();
            commandPresenter = new CommandPresenter(this, screenDrawer, mode);
            commandPresenter.SetMode(mode);
            SetComponentsActions();
            FilterOff();
            Log.Debug("CommandActivity", "Mode :" + mode);           
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            //commandPresenter.OnDestroy();
        }

        private void SetComponents()
        {
            commandsLayout = (LinearLayout)FindViewById(Resource.Id.linearLayout1);
            scan = (Button)FindViewById(Resource.Id.buttonScan);
            saveFile = (Button)FindViewById(Resource.Id.buttonSave);
            deleteDraw = (Button)FindViewById(Resource.Id.buttonDelete);
            drive = (Button)FindViewById(Resource.Id.buttonPilot);
            averagePicker = (NumberPicker)FindViewById(Resource.Id.averagePicker);
            useMode = (TextView)FindViewById(Resource.Id.selectedMode);
            filter = (Button)FindViewById(Resource.Id.ButtonFilter);
            fileName = new EditText(this);
            
            InitAlert();       
        }

        private void SetComponentsActions()
        {
            filter.Click += delegate
            {
                commandPresenter.OnFilterClick();
                Log.Debug("CommandActivity : "  ,"filter.CLick()");
            };

            scan.Click += delegate
            {
                commandPresenter.OnScanClicked();
            };

            saveFile.Click += delegate
            {
                commandPresenter.OnSaveFileClicked();
            };

            deleteDraw.Click += delegate
            {
                commandPresenter.OnDeleteDrawClicked();
            };

            drive.Touch += (object sender, View.TouchEventArgs e) =>
            {
                if (e.Event.Action == MotionEventActions.Down)
                {
                    commandPresenter.SetFirstUpdate(e.Event.EventTime);
                    sensorManager.RegisterListener(this, accelerometer, SensorDelay.Normal);
                }
                else if (e.Event.Action == MotionEventActions.Up)
                {
                    commandPresenter.Stop();
                    sensorManager.UnregisterListener(this, accelerometer);
                }
            };
            averagePicker.ValueChanged += delegate
            {
                commandPresenter.OnAveragePickerScroll(averagePicker.Value);
            };
        }

      

        public void Scan()
        {
            commandPresenter.Scan();
        }

        public void InitAlert()
        {
            alertSaveDialogBuilder = new AlertDialog.Builder(this)
           .SetTitle("Enregistrer l'image")
           .SetMessage("Entrez le nom à donner à l'image")
           .SetView(fileName)
           .SetPositiveButton("Save", (senderAlert, args) =>
           {
               string name = fileName.Text;

               commandPresenter.SaveFile(name);

               alertSaveDialog.Dismiss();
           })
           .SetNegativeButton("Cancel", (senderAlert, args) =>
           {
               alertSaveDialog.Dismiss();
           });

           alertSaveDialog = alertSaveDialogBuilder.Create();
           Log.Debug("ImageSaver : ", "Alert initialisée");
        }

       
        public void LoadPath(Path path)
        {
            screenDrawer.LoadPath(path);
        }

        public void LoadReviewMode()
        {
            scan.Enabled = false;
            drive.Enabled = false;
            useMode.Text = "Review Mode";
            useMode.SetTextColor(Color.DarkRed);
            commandsLayout.SetBackgroundColor(Color.OrangeRed);
            commandPresenter.SetFile(Intent.GetStringExtra("File"));
        }
        
        public void LoadCommandMode()
        {
            scan.Enabled = true;
            drive.Enabled = true;
            this.ipAdrress = Intent.GetStringExtra("IP");
            this.port = Intent.GetStringExtra("Port");
            commandPresenter.SetConnexionDetails(ipAdrress, port);
            useMode.Text = "Command Mode";
            useMode.SetTextColor(Color.Green);
            commandsLayout.SetBackgroundColor(Color.DarkSeaGreen);
            sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            accelerometer = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
        }


        public void ShowSavingDialog()
        {
            alertSaveDialog.Show();
        }

        public void LoadPathStats(double[] pathStats)
        {
            screenDrawer.LoadPathStats(pathStats);
        }

        public void SetAverageSelection(string[] intervalSelection)
        {
            averagePicker.SetDisplayedValues(intervalSelection);
            averagePicker.MinValue = 0;
            averagePicker.MaxValue = intervalSelection.Length - 1;
            averagePicker.WrapSelectorWheel = true;
        }

        public void ShowConnexionStatus(int status)
        {
            Toast.MakeText(this, status, ToastLength.Short);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            
        }

        public void OnSensorChanged(SensorEvent e)
        {
            commandPresenter.OnSensorChanged(e.Values[0], e.Values[1],e.Timestamp);
        }






















        public void FilterOn()
        {
            averagePicker.Enabled = true;
            filter.SetBackgroundColor(Color.Aquamarine);
        }

        public void FilterOff()
        {
            averagePicker.Enabled = false;
            filter.SetBackgroundColor(Color.Crimson);
        }
    }
}