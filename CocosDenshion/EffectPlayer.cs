using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using cocos2d;
using System.Diagnostics;

namespace CocosDenshion
{
    public class EffectPlayer
    {
        public static ulong s_mciError;

        uint m_nSoundID;
        SoundEffect m_effect;

        public EffectPlayer()
        {
            m_nSoundID = 0;
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
            } while (false);
        }

        public void Play(bool bLoop)
        {
            if (null == m_effect)
            {
                return;
            }

            m_effect.Play();
           
        }

        public void Play()
        {
            Play(false);
        }

        public void Close()
        {
            Stop();

            m_effect = null;
        }

        public void Pause()
        {
            Debug.WriteLine("Pause is invalid for sound effect");
        }

        public void Resume()
        {
            Debug.WriteLine("Resume is invalid for sound effect");
        }

        public void Stop()
        {
            Debug.WriteLine("Stop is invalid for sound effect");
        }

        public void Rewind()
        {
            Debug.WriteLine("Rewind is invalid for sound effect");
        }

        public bool IsPlaying()
        {
            Debug.WriteLine("IsPlaying is invalid for sound effect");
            return false;
        }

        public uint GetSoundID()
        {
            return m_nSoundID;
        }

        // the volume is gloabal, it will affect other effects' volume
        public static float Volume
        {
            get
            {
                return SoundEffect.MasterVolume;
            }

            set
            {
                if (value >= 0.0f && value <= 1.0f)
                {
                    SoundEffect.MasterVolume = value;
                }
            }
        }

    }
}
