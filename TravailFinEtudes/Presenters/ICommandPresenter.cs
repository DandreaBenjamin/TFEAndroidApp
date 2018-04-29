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
    interface ICommandPresenter
    {
        //Android Activities Lifecycle delegate methods

        void OnCreate();
        void OnResume();
        void OnDestroy();
        void OnPause();
        //


        int Connect(String ip, int ports);
        int Disconnect();
        Boolean CheckIpAdressFormat(String ip);
        int TryConnect(String ip, String port);
        void ClickOnSave();
        void LoadPathStats();
        void LoadObstaclePath();
        void DeletePath();
        Boolean DeleteObstacle(string fileName);
        void Scan();
        void SetConnexionDetails(string ip, string port);
        void Move(String direction);
        void SaveFile(string fileName);
        Boolean IsAppConnected();
        void MapAveragePickerToIntegerValue(int value);
     
        void SetAverageSelection();
        void SetMode(string mode);
        void SetFile(string filePath);

        void OnScanClicked();
        Boolean OnDeleteDrawClicked();
        void OnSaveFileClicked();
        void OnDrivePushed();
        void OnDriveReleased();
        void OnSensorChanged(float X, float Y, long timestamp);
        void UpdateDirection(float x, float y);
        void Stop();
        void OnAveragePickerScroll(int average);
        void OnFilterClick();
    }
}