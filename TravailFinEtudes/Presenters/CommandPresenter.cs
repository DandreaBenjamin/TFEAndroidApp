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
using Newtonsoft.Json;
using System.IO;
using System.Net.Sockets;
using TravailFinEtudes.Views;
using TravailFinEtudes.Model;
using TravailFinEtudes.Utils;
using System.Net;
using Android.Util;
using System.Xml.Serialization;

namespace TravailFinEtudes.Presenters
{
    class CommandPresenter:ICommandPresenter
    {
        private long lastUpdate = 0;
        private String baseDirectory;
        private Socket socket;
        private ICommandActivity commandActivity;
        private IScreenDrawer screenDrawer;
        private IEnumerable<String> files;
        private Drawing loadedObstacle = null;
        private string[] intervalSelection = { "3", "5", "7", "9", "11", "13" };
        private int selectedInterval;
        private Boolean ObstacleLoaded = false;
        private readonly int[] connexionMessagesCodes = {0, 2130903046, 2130903044, 2130903047, 2130903045, 2130903043 };
        private string ip = null;
        private string port = null;
        private string mode;
        private string openedFile = null;
        private Boolean firstUpdateDone = false;
        private Boolean isFilterChecked = false;
        private long timeFirstUpdate = 0;
        private readonly byte[] forward = Encoding.UTF8.GetBytes("forward");
        private readonly byte[] backward = Encoding.UTF8.GetBytes("backward");
        private readonly byte[] right = Encoding.UTF8.GetBytes("right");
        private readonly byte[] left = Encoding.UTF8.GetBytes("left");
        private readonly byte[] stay = Encoding.UTF8.GetBytes("stay");
        private readonly byte[] scan = Encoding.UTF8.GetBytes("scan");

        public CommandPresenter(ICommandActivity commandActivity,IScreenDrawer screenDrawer, string mode)
        {
            baseDirectory = Android.App.Application.Context.GetString(Resource.String.JSONObjectsDirectory);
            this.commandActivity = commandActivity;
            this.screenDrawer = screenDrawer;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SetAverageSelection();
        }


        public void OnCreate()
        {
            files = Directory.EnumerateFiles(baseDirectory);
            SetAverageSelection();
        }


        public void OnDestroy()
        {
           Disconnect();
        }

        public void OnPause()
        {
            throw new NotImplementedException();
        }

        public void OnResume()
        {
            throw new NotImplementedException();
        }
     
        public void SaveFile(string fileName)
        {
            var filePath = System.IO.Path.Combine(baseDirectory, fileName);
            using (StreamWriter streamWriter = File.CreateText(filePath))
            {
               JsonSerializer serializer = new JsonSerializer();
               serializer.Serialize(streamWriter, loadedObstacle);
            }
        }

        public void MapAveragePickerToIntegerValue(int value)
        {
            selectedInterval = int.Parse(intervalSelection[value]);
        }

        public void DeletePath()
        {
            loadedObstacle = null;
            ObstacleLoaded = false;
            screenDrawer.DeletePath();
        }

        public Boolean DeleteObstacle()
        { 
                try
                {
                    File.Delete(this.openedFile);
                    if (!File.Exists(this.openedFile))
                    {
                         ObstacleLoaded = false;
                         return true;
                    }
                    else
                        return false;
                }
                catch (Exception e)
                {
                    return false;
                }   
        }

        public void ClickOnSave()
        {
            commandActivity.ShowSavingDialog();
        }

        

        public void SetAverageSelection()
        {
            commandActivity.SetAverageSelection(intervalSelection);
        }

        

        ///////////////////////////////////Loading\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public void LoadPathStats()
        {
            commandActivity.LoadPathStats(loadedObstacle.stats);
        }

        public void LoadObstaclePath()
        {
            var filePath = System.IO.Path.Combine(baseDirectory, openedFile);
            Console.WriteLine(File.ReadAllText(filePath));
            loadedObstacle = JsonConvert.DeserializeObject<Drawing>(File.ReadAllText(filePath));
            ObstacleLoaded = true;
        }

        public void SetMode(string mode)
        {
            this.mode = mode;
            if (mode == "Review")
            {
                commandActivity.LoadReviewMode();
                if (openedFile != null)
                {
                    LoadObstaclePath();
                    if (loadedObstacle != null)
                    {
                        commandActivity.LoadPath(MathUtil.GetPathFromObstacleCoordinates(loadedObstacle.rawCoordinates));
                        LoadPathStats();
                    }
                }
            }
            else
            {
                commandActivity.LoadCommandMode();
             /*
              * 
              * 
                TryConnect(this.ip, this.port);
             */
            }
        }

        public void Scan()
        {       
            byte[] response = new byte[360];
            try
            {
                socket.Send(scan);
                socket.Receive(response);
                Console.WriteLine(response);
                int[] distances; double[] stats; float[] rawCoordinates;
                MathUtil.AddDistances(response, out distances);
                MathUtil.FindStats(distances, out stats);
                MathUtil.FindCoordinates(distances, 405, 444, 0, out rawCoordinates);
                loadedObstacle = new Drawing(distances, rawCoordinates, stats);
                ObstacleLoaded = true;
                LoadPathStats();
                commandActivity.LoadPath(MathUtil.GetPathFromObstacleCoordinates(loadedObstacle.rawCoordinates));
            }
            catch (SocketException sE)
            {

            }        
        }

        public void Move(String direction)
        {
            switch (direction)
            {
                case "forward":
                    socket.Send(forward);
                    break;
                case "backward":
                    socket.Send(backward);
                    break;
                case "right":
                    socket.Send(right);
                    break;
                case "left":
                    socket.Send(left);
                    break;
                case "stay":
                    socket.Send(stay);
                    break;
            }
        }

        //////////////////////////////////////////////////////////////////
        /////////////////////////////CONNEXION////////////////////////////
        //////////////////////////////////////////////////////////////////
        
        public int Connect(string ip, int port)
        {
            if (!socket.Connected)
            {
                IPAddress[] adresses = Dns.GetHostAddresses(ip);
                try
                {
                    socket.Connect(adresses, port);
                    if (socket.Connected)
                    {
                        return connexionMessagesCodes[1];
                    }
                }
                catch (Exception e)
                {
                    return connexionMessagesCodes[2];
                }
            }
            return connexionMessagesCodes[3];
        }

        public Boolean CheckIpAdressFormat(string ip)
        {
            IPAddress ipAddr;
            return IPAddress.TryParse(ip, out ipAddr);
        }

        public int TryConnect(string ip, string port)
        {
            if (socket.Connected)
                return Disconnect();
            else
                return Connect(ip, int.Parse(port));
        }

        public int Disconnect()
        {
            try
            {
                socket.Disconnect(true);
                if (!socket.Connected)
                    return connexionMessagesCodes[4];
                else
                    return connexionMessagesCodes[5];
            }
            catch (Exception e)
            {
                return connexionMessagesCodes[5];
            }
        }

        public bool IsAppConnected()
        {
            return socket.Connected;
        }

        public void SetConnexionDetails(string ip, string port)
        {
            this.ip = ip;
            this.port = port; 
        }

        public void OnScanClicked()
        {
            Scan();
        }

        public void OnSaveFileClicked()
        {
            commandActivity.ShowSavingDialog();
        }

        public void OnDrivePushed()
        {
            

        }

        public void OnDriveReleased()
        {
            
            

        }

        public void SetFile(string filePath)
        {
            this.openedFile = filePath;
        }

        public bool OnDeleteDrawClicked()
        {
            if (mode == "Review" && openedFile != null)
            {
                DeleteObstacle();
                this.openedFile = null;
            }
            DeletePath();
            return true;
        }

        public void SetFirstUpdate(long timestamp)
        {
            firstUpdateDone = true;
            this.timeFirstUpdate = timestamp;
        }

        public void OnSensorChanged(float X, float Y,long timestamp)
        {
            if (!firstUpdateDone)
            {
                lastUpdate = timestamp;
                firstUpdateDone = true;
                UpdateDirection(X, Y);
            }

            long timeNow = timestamp;

            if ((timeNow - lastUpdate) > 5000000)
            {
                UpdateDirection(X, Y);
                lastUpdate = timeNow;
            }
        }


        public void UpdateDirection(float X, float Y)
        {
            if (X <= -2)
                Console.WriteLine(forward.ToString());
            else if (X >= 2)
                Console.WriteLine(backward.ToString());
            else if (Y <= -2)
                Console.WriteLine(left.ToString());
            else if (Y >= 2)
                Console.WriteLine(right.ToString());
            else
                Console.WriteLine(stay.ToString());
        }

        public void SendCommand(byte[] data)
        {
            try
            {
                socket.Send(data);
            }
            catch (Exception e) { };
        }


        public void Stop()
        {
            Console.WriteLine(stay.ToString());
            // SendCommand(stay);
        }

        public void OnAveragePickerScroll(int average)
        {
            if (isFilterChecked)
            {
                MapAveragePickerToIntegerValue(average);
                float[] newCoordinates = MathUtil.ProcessNewCoordinates(loadedObstacle.rawDistances, selectedInterval);
                screenDrawer.LoadPath(MathUtil.GetPathFromObstacleCoordinates(newCoordinates));
            }
        }

        public void OnFilterClick()
        {
            isFilterChecked = !isFilterChecked;

            if (isFilterChecked && loadedObstacle != null)
            {
                commandActivity.FilterOn();
            }
            else if(!isFilterChecked && loadedObstacle != null)
            {
                commandActivity.FilterOff();
                commandActivity.LoadPath(MathUtil.GetPathFromObstacleCoordinates(loadedObstacle.rawCoordinates));
            }
        }
    }
}