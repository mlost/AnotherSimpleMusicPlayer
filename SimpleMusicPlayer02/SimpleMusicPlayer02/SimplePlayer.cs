using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace SimpleMusicPlayer02
{
    class SimplePlayer
    {
        [DllImport("winmm.dll")]
        private static extern int mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        private StringBuilder returnData = new StringBuilder(128);
        private string command;
        public bool isPaused = false;

        public void open(string sFileName)
        {
            command = "open \"" + sFileName + "\" type mpegvideo alias MyMusic";
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public void close()
        {
            command = "close MyMusic";
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public void play()
        {
            string command = "play MyMusic";
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public void pause()
        {
            if(isPaused)
            {
                resume();
                isPaused = false;
            }
            else if(isPlaying())
            {
                command = "pause MyMusic";
                mciSendString(command, null, 0, IntPtr.Zero);
                isPaused = true;
            }
        }

        public void resume()
        {
            command = "resume MyMusic";
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public void stop()
        {
            command = "stop MyMusic";
            mciSendString(command, null, 0, IntPtr.Zero);
            isPaused = false;
            close();
        }

        public bool isPlaying()
        {
            command = "status MyMusic mode";
            mciSendString(command, returnData, returnData.Capacity, IntPtr.Zero);
            if (returnData.Length == 7 && returnData.ToString().Substring(0,7) == "playing")
            {
                return true;
            }
            else { return false; }
        }

        public int getCurrentMilisecond()
        {
            command = "status MyMusic position";
            mciSendString(command, returnData, returnData.Capacity, IntPtr.Zero);
            return int.Parse(returnData.ToString());
        }

        public void SetPosition(int miliseconds)
        {
            if(isPlaying())
            {
                command = "play MyMusic from " + miliseconds.ToString();
                mciSendString(command, null, 0, IntPtr.Zero);
            }
            else
            {
                command = "seek MyMusic to " + miliseconds.ToString();
                mciSendString(command, null, 0, IntPtr.Zero);
            }
        }

        public int getSongLength()
        {
            if(isPlaying())
            {
                command = "status MyMusic length";
                mciSendString(command, returnData, returnData.Capacity, IntPtr.Zero);
                return int.Parse(returnData.ToString());
            } else { return 0; }
        }

        public bool setVolume(int volume)
        {
            if(volume >= 0 && volume <= 1000)
            {
                command = "setaudio MyMusic volume to " + volume.ToString();
                mciSendString(command, null, 0, IntPtr.Zero);
                return true;
            } else { return false; }
        }
    }
}
