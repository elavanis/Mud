using Mud.Interface;
using Objects.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Objects.Global.Logging.LogSettings;

namespace Mud
{
    public class HeartBeat : IHeartBeat
    {
        private int _heartBeatDelay;
        private bool _continueHeartBeat;
        public event EventHandler Tick;
        public HeartBeat(int heartBeatDelay = 500)
        {
            _heartBeatDelay = heartBeatDelay;
        }

        public void StartHeartBeat()
        {
            _continueHeartBeat = true;
            Stopwatch sw = new Stopwatch();
            while (_continueHeartBeat)
            {
                sw.Restart();
                try
                {
                    //IAsyncResult result = Tick.BeginInvoke(null, null, null, null);
                    Tick.Invoke(null, null);
                    //Thread.Sleep(_heartBeatDelay);
                    //Tick.EndInvoke(result);
                }
                catch (Exception ex)
                {
                    string message = ex.Message + Environment.NewLine + ex.StackTrace.ToString();
                    GlobalReference.GlobalValues.Logger.Log(LogLevel.ERROR, message);
                }
                finally
                {
                    if (GlobalReference.GlobalValues.TickCounter % 60 == 0)  //every 30 seconds
                    {
                        GC.Collect(); //force a garbabce collection now while nothing is happening
                    }
                    int sleepTime = Math.Max(0, (int)(_heartBeatDelay - sw.ElapsedMilliseconds));
                    Thread.Sleep(sleepTime);
                }
            }

            GlobalReference.GlobalValues.Logger.FlushLogs();
        }

        public void StopHeartBeat()
        {
            _continueHeartBeat = false;
        }
    }
}
