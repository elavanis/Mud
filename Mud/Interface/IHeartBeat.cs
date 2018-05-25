using System;

namespace Mud.Interface
{
    public interface IHeartBeat
    {
        event EventHandler Tick;

        void StartHeartBeat();
        void StopHeartBeat();
    }
}