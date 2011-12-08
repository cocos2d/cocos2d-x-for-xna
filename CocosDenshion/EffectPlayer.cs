using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using cocos2d;

namespace CocosDenshion
{
    public class EffectPlayer
    {
        public static ulong s_mciError;

        uint m_nSoundID;
        int m_nTimes;
        bool m_bPlaying;

        SoundEffect m_effect;

        public EffectPlayer()
        {
            m_nSoundID = 0;
            m_nTimes = 0;
            m_bPlaying = false;
        }

        ~EffectPlayer()
        {
            Close();
        }

        public void Open(string pFileName, uint uId)
        {
            do 
            {
                if (null == pFileName || pFileName.Length == 0)
                    break;

                Close();

                m_effect = CCApplication.sharedApplication().content.Load<SoundEffect>(pFileName);

                m_nSoundID = uId;
                m_bPlaying = false;
            } while (false);
        }

        public void Play(int nTimes /* = 1 */)
        {
            if (null == m_effect)
            {
                return;
            }

            m_effect.Play();
            m_bPlaying = true;
            m_nTimes = nTimes;
            
        }

        public void Play()
        {
            Play(1);
        }

        public void Close()
        {
            if (m_bPlaying)
            {
                Stop();
            }

            m_effect.Dispose();
            m_effect = null;

            m_bPlaying = false;
        }

        public void Pause()
        {
        }

        public void Resume()
        {
        }

        public void Stop()
        {
            m_bPlaying = false;
        }

        public void Rewind()
        {
        }

        public bool IsPlaying()
        {
            return m_bPlaying;
        }

        public uint GetSoundID()
        {
            return m_nSoundID;
        }

        ////////////////////////////////////////////////////////////////////////////
        //// private member
        ////////////////////////////////////////////////////////////////////////////

        public void _SendGenericCommand(int nCommand)
        {
            //if (! m_hDev)
            //{
            //    return;
            //}
            //mciSendCommand(m_hDev, nCommand, 0, 0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //// static function
        ////////////////////////////////////////////////////////////////////////////

        //LRESULT WINAPI _SoundPlayProc(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam)
        //{
        //    EffectPlayer * pPlayer = NULL;
        //    if (MM_MCINOTIFY == Msg 
        //        && MCI_NOTIFY_SUCCESSFUL == wParam
        //        &&(pPlayer = (EffectPlayer *)GetWindowLong(hWnd, GWL_USERDATA)))
        //    {
        //        if (pPlayer->m_nTimes)
        //        {
        //            --pPlayer->m_nTimes;
        //        }

        //        if (pPlayer->m_nTimes)
        //        {
        //            mciSendCommand(lParam, MCI_SEEK, MCI_SEEK_TO_START, 0);

        //            MCI_PLAY_PARMS mciPlay = {0};
        //            mciPlay.dwCallback = (DWORD)hWnd;
        //            mciSendCommand(lParam, MCI_PLAY, MCI_NOTIFY,(DWORD)&mciPlay);
        //        }
        //        else
        //        {
        //            pPlayer->m_bPlaying = false;
        //        }
        //        return 0;
        //    }
        //    return DefWindowProc(hWnd, Msg, wParam, lParam);
        //}

    }
}
