using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using cocos2d;

namespace CocosDenshion
{
    public class MusicPlayer
    {
        public static ulong s_mciError;

        uint m_nSoundID;
        Song m_music;

        public MusicPlayer()
        {
            m_nSoundID = 0;
        }

        ~MusicPlayer()
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

                m_music = CCApplication.sharedApplication().content.Load<Song>(pFileName);
                
                m_nSoundID = uId;
            } while (false);
        }

        public void Play(bool bLoop)
        {
            if (null != m_music)
            {
                MediaPlayer.IsRepeating = bLoop;
                MediaPlayer.Play(m_music);
            }
}

        public void Play()
        {
            Play(false);
        }

        public void Close()
        {
#if !ZUNE
            // Pedro Kayatt - On the Zune, you must restore the play state of background music to the Zune's current music.
            // This code will prevent your game from being authorized on the windows marketplace.
            if (IsPlaying())
            {
                Stop();
            }
#endif
            if (m_music != null)
            {
                m_music = null;
            }
        }

        public void Pause()
        {
            MediaPlayer.Pause();
        }

        public void Resume()
        {
            MediaPlayer.Resume();
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }

        public void Rewind()
        {
            Stop();

            if (null != m_music)
            {
                MediaPlayer.Play(m_music);
            }
        }

        public bool IsPlaying()
        {
            if (MediaState.Playing == MediaPlayer.State)
            {
                return true;
            }

            return false;
        }

        public uint GetSoundID()
        {
            return m_nSoundID;
        }

        public float Volume
        {
            get
            {
                return MediaPlayer.Volume;
            }

            set 
            {
                if (value >= 0.0f && value <= 1.0f)
                {
                    MediaPlayer.Volume = value;
                }
            }
        }
    }
}
