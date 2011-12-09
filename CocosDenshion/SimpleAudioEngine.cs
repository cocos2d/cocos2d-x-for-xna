using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace CocosDenshion
{
    public class SimpleAudioEngine
    {
        static string s_szRootPath;
        static ulong s_dwRootLen;
        static string s_szFullPath;

        static SimpleAudioEngine s_SharedEngine;
        static Dictionary<uint, EffectPlayer> s_List;
        static MusicPlayer s_Music;

        public static string _FullPath(string szPath)
        {
            // todo: return self now
            return szPath;

            // return null;
        }
        
        public static uint _Hash(string key)
        {
            uint hash = 0;
            char[] arrKey = key.ToUpper().ToCharArray();
            for (int i = 0; i < key.Length; i++)
            {
                hash *= 16777619;
                hash ^= (uint)(arrKey[i]);
            }
            return hash;
        }

        public static Dictionary<uint, EffectPlayer> sharedList()
        {
            if (null == s_List)
            {
                s_List = new Dictionary<uint, EffectPlayer>();
            }

            return s_List;
        }

        static MusicPlayer sharedMusic()
        {
            if (null == s_Music)
            {
                s_Music = new MusicPlayer();
            }

            return s_Music;
        }


        public SimpleAudioEngine()
        {         
        }

        ~SimpleAudioEngine()
        { 
        }

        /**
        @brief Get the shared Engine object,it will new one when first time be called
        */
        public static SimpleAudioEngine sharedEngine()
        {
            if (null == s_SharedEngine)
            {
                s_SharedEngine = new SimpleAudioEngine();
            }

            return s_SharedEngine;
        }

        /**
        @brief Release the shared Engine object
        @warning It must be called before the application exit, or a memroy leak will be casued.
        */
        public void end()
        { 
            sharedMusic().Close();

            foreach (KeyValuePair<uint, EffectPlayer> kvp in sharedList())
            {
                kvp.Value.Close(); 
            }
   
            sharedList().Clear();
        }

        /**
        @brief  Set the zip file name
        @param pszZipFileName The relative path of the .zip file
        */
        public static void setResource(string pszZipFileName)
        { 
        }

        /**
         @brief Preload background music
         @param pszFilePath The path of the background music file,or the FileName of T_SoundResInfo
         */
        public void preloadBackgroundMusic(string pszFilePath)
        {
            sharedMusic().Open(_FullPath(pszFilePath), _Hash(pszFilePath));
        }
    
        /**
        @brief Play background music
        @param pszFilePath The path of the background music file,or the FileName of T_SoundResInfo
        @param bLoop Whether the background music loop or not
        */
        public void playBackgroundMusic(string pszFilePath, bool bLoop)
        {
            if (null == pszFilePath)
            {
                return;
            }

            sharedMusic().Open(_FullPath(pszFilePath), _Hash(pszFilePath));
            sharedMusic().Play((bLoop) ? -1 : 1);
    
        }

        /**
        @brief Play background music
        @param pszFilePath The path of the background music file,or the FileName of T_SoundResInfo
        */
        public void playBackgroundMusic(string pszFilePath)
        {
            playBackgroundMusic(pszFilePath, false);
        }

        /**
        @brief Stop playing background music
        @param bReleaseData If release the background music data or not.As default value is false
        */
        public void stopBackgroundMusic(bool bReleaseData)
        {
            if (bReleaseData)
            {
                sharedMusic().Close();
            }
            else
            {
                sharedMusic().Stop();
            }
        }

        /**
        @brief Stop playing background music
        */
        public void stopBackgroundMusic()
        {
            stopBackgroundMusic(false);
        }

        /**
        @brief Pause playing background music
        */
        public void pauseBackgroundMusic()
        {
            sharedMusic().Pause();
        }

        /**
        @brief Resume playing background music
        */
        public void resumeBackgroundMusic()
        {
            sharedMusic().Resume();
        }

        /**
        @brief Rewind playing background music
        */
        public void rewindBackgroundMusic()
        {
            sharedMusic().Rewind();
        }

        public bool willPlayBackgroundMusic()
        {
            return false;
        }

        /**
        @brief Whether the background music is playing
        @return If is playing return true,or return false
        */
        public bool isBackgroundMusicPlaying()
        {
            return sharedMusic().IsPlaying();
        }

        // properties
        /**
        @brief the volume of the background music max value is 1.0,the min value is 0.0
        */
        public float getBackgroundMusicVolume()
        {
            return sharedMusic().Volume;
        }

        /**
        @brief set the volume of background music
        @param volume must be in 0.0~1.0
        */
        public void setBackgroundMusicVolume(float volume)
        {
            sharedMusic().Volume = volume;
        }

        /**
        @brief The volume of the effects max value is 1.0,the min value is 0.0
        */
        public float getEffectsVolume()
        {
            return EffectPlayer.Volume;
        }

        /**
        @brief set the volume of sound effecs
        @param volume must be in 0.0~1.0
        */
        public void setEffectsVolume(float volume)
        {
            EffectPlayer.Volume = volume;
        }

        // for sound effects
        /**
        @brief Play sound effect
        @param pszFilePath The path of the effect file,or the FileName of T_SoundResInfo
        @bLoop Whether to loop the effect playing, default value is false
        */
        public uint playEffect(string pszFilePath, bool bLoop)
        { 
            uint nRet = _Hash(pszFilePath);

            preloadEffect(pszFilePath);

            foreach (KeyValuePair<uint, EffectPlayer> kvp in sharedList())
            {
                if (nRet == kvp.Key)
                {
                    kvp.Value.Play((bLoop) ? -1 : 1);
                }
            }
   
            return nRet;
        }

        /**
        @brief Play sound effect
        @param pszFilePath The path of the effect file,or the FileName of T_SoundResInfo
        */
        public uint playEffect(string pszFilePath)
        {
            return playEffect(pszFilePath, false);
        }

        /**
        @brief Stop playing sound effect
        @param nSoundId The return value of function playEffect
        */
        public void stopEffect(uint nSoundId)
        {
            foreach (KeyValuePair<uint, EffectPlayer> kvp in sharedList())
            {
                if (nSoundId == kvp.Key)
                {
                    kvp.Value.Stop();
                }
            }
        }

        /**
        @brief  		preload a compressed audio file
        @details	    the compressed audio will be decode to wave, then write into an 
        internal buffer in SimpleaudioEngine
        */
        public void preloadEffect(string pszFilePath)
        {
            do 
            {
                if (pszFilePath.Length <= 0)
                    break;
                
                uint nID = _Hash(pszFilePath);

                if (sharedList().ContainsKey(nID))
                    break;

                EffectPlayer eff = new EffectPlayer();
                eff.Open(_FullPath(pszFilePath), nID);
                sharedList().Add(nID, eff);

            } while (false);
        }

        /**
        @brief  		unload the preloaded effect from internal buffer
        @param[in]		pszFilePath		The path of the effect file,or the FileName of T_SoundResInfo
        */
        public void unloadEffect(string pszFilePath)
        { 
            uint nID = _Hash(pszFilePath);

            if (sharedList().ContainsKey(nID))
            {
                sharedList().Remove(nID);
            }
        }

    }
}
