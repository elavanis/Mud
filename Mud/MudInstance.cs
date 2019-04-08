using Mud.Interface;
using Objects.Command.World;
using Objects.Global;
using System;
using System.Diagnostics;
using System.Threading;
using static Objects.Global.Logging.LogSettings;

namespace Mud
{
    public class MudInstance
    {
        private int lastTimeRan = -1;
        private int lastUpdateLength = 0;

        private Thread _heartBeatThread { get; set; }

        private Stopwatch sw = new Stopwatch();

        private IHeartBeat _heartBeat { get; set; }

        public MudInstance()
        {
            GlobalReference.GlobalValues.Initilize();
        }

        public void StartMud()
        {
            LoadWorld();
            GlobalReference.GlobalValues.Logger.Log(LogLevel.INFO, "Starting Heartbeat");
            StartHeartBeat();
        }

        #region StartMudMethods
        private void StartHeartBeat()
        {
            _heartBeat = new HeartBeat();
            _heartBeat.Tick += HeartBeat_Tick;

            _heartBeatThread = new Thread(() =>
            {
                Thread.CurrentThread.Name = "HeartBeat";
                Thread.CurrentThread.IsBackground = true;
                _heartBeat.StartHeartBeat();
            });
            _heartBeatThread.Start();
        }

        private void LoadWorld()
        {
            GlobalReference.GlobalValues.Logger.Log(LogLevel.INFO, "Loading World");
            GlobalReference.GlobalValues.World.LoadWorld();
        }
        #endregion StartMudMethods

        public void StopMud(string zoneFileLocation)
        {
            _heartBeat.StopHeartBeat();

            //wait for the heartbeat to stop then flush the logs 1 more time to get everything out
            _heartBeatThread.Join();
            GlobalReference.GlobalValues.Logger.FlushLogs();

            //World.SaveWorld(zoneFileLocation);
        }

        public void HeartBeat_Tick(object sender, EventArgs e)
        {
            sw.Restart();
            GlobalReference.GlobalValues.TickCounter++;
            GlobalReference.GlobalValues.World.PerformTick();
            GlobalReference.GlobalValues.TickTimes.Enqueue(sw.ElapsedTicks);

            if (DateTime.Now.Second % 5 == 0 && lastTimeRan != DateTime.Now.Second)
            {
                lastTimeRan = DateTime.Now.Second;
                GameStats gs = new GameStats();
                string s = gs.GenerateGameStats();
                Console.SetCursorPosition(0, 0);
                if (s.Length != lastUpdateLength)
                {
                    lastUpdateLength = s.Length;
                    Console.Clear();
                }
                Console.Write(s);
            }
        }
    }
}
