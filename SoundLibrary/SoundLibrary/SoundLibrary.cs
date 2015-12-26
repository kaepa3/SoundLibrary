using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Controls;

namespace SoundLibrary
{
    public delegate void SoundEvent(int a);

    public class SoundUtill
    {
        public SoundEvent PlayEvent;
        string ariasName = "mySound";
        public SoundUtill()
        {
        }
        public SoundUtill(string text)
        {
            ariasName = text;
        }
        [DllImport("winmm.dll")]
        extern static int mciSendString(string s1, StringBuilder s2, int i1, IntPtr i2);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern MMRESULT waveOutWrite(IntPtr hwo, IntPtr pwh, uint cbwh);


        /// <summary>
        /// コールバックのデリゲート
        /// </summary>
        /// <param name="hdrvr"></param>
        /// <param name="uMsg"></param>
        /// <param name="dwUser"></param>
        /// <param name="wavhdr"></param>
        /// <param name="dwParam2"></param>
        public delegate void WaveOutProc(IntPtr hdrvr, WaveOutMessage uMsg, int dwUser, IntPtr wavhdr, int dwParam2);

        /// <summary>
        /// ファイルオープン
        /// </summary>
        /// <param name="FileName"></param>
        bool OpenFile(string fileName)
        {
            return 0 == mciSendString("open " + fileName + " alias " + ariasName, null, 0, IntPtr.Zero);
        }

        /// <summary>
        /// クローズ処理
        /// </summary>
        bool CloseFile()
        {
            return 0 == mciSendString("close  " + ariasName, null, 0, IntPtr.Zero);
        }
        /// <summary>
        /// 再生
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Play(string fileName, Window wnd = null) 
        {
            if (OpenFile(fileName))
            {
                HwndSource source = wnd != null ? (HwndSource)HwndSource.FromVisual(wnd) : null;
                if (mciSendString("play   " + ariasName, null, 0, source != null ? source.Handle : IntPtr.Zero) == 0)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 再生中か？
        /// </summary>
        /// <returns></returns>
        public bool IsPlay()
        {
            StringBuilder sb = new StringBuilder(32);
            if(mciSendString("status " + ariasName + " mode", sb, sb.Capacity, IntPtr.Zero) == 0)
            {
                return sb.ToString() == "playing";
            }
            return false;
        }

        /// <summary>
        /// 再生停止
        /// </summary>
        public bool PlayStop()
        {
            if (mciSendString("stop   " + ariasName, null, 0, IntPtr.Zero) == 0)
            {
                if (CloseFile())
                {
                    return true;
                }
            }
            return false;
        }        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Rec()
        {
            bool flg = false;
            mciSendString("open new type waveaudio alias Rec", null, 0, IntPtr.Zero);
            
            if (mciSendString("record Rec", null, 0, IntPtr.Zero) == 0)
            {
                flg =  true;
            }
            return flg;
        }
        /// <summary>
        /// 録音終了
        /// </summary>
        /// <returns></returns>
        public bool RecStop(string fileName)
        {
            try
            {
                mciSendString("stop Rec" , null, 0, IntPtr.Zero);
                mciSendString("save Rec " + fileName, null, 0, IntPtr.Zero);
                mciSendString("close Rec", null, 0, IntPtr.Zero);
                return true;
            }
            catch{}
            return false;

        }
    }
}
