using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using cocos2d;

namespace CocosDenshion
{
    /// <summary>
    /// This interface controls the media player on the device. For Microsoft mobile devices
    /// that play music, e.g. Zune and phone, you must not intefere with the background music
    /// unless the user has allowed it.
    /// </summary>
    public class MusicPlayer
    {
        public static ulong s_mciError;

        private uint m_nSoundID;
        private Song m_music;
        /// <summary>
        /// Track if we did play our own game song, otherwise the media player is owned
        /// by the user of the device and that user is listening to background music.
        /// </summary>
        private bool m_didPlayGameSong = false;
        private Song m_SongToPlayAfterClose;
        private float m_VolumeAfterClose = 1f;
        private TimeSpan m_PlayPositionAfterClose = TimeSpan.Zero;
        private MediaQueue m_QueueAfterClose;
        private bool m_IsRepeatingAfterClose = false;
        private bool m_IsShuffleAfterClose = false;

        public MusicPlayer()
        {
            m_nSoundID = 0;
            if (MediaPlayer.State == MediaState.Playing)
            {
                SaveMediaState();
            }
        }

        public void SaveMediaState()
        {
            // User is playing a song, so remember the song state.
            m_SongToPlayAfterClose = MediaPlayer.Queue.ActiveSong;
            m_VolumeAfterClose = MediaPlayer.Volume;
            m_PlayPositionAfterClose = MediaPlayer.PlayPosition;
            m_IsRepeatingAfterClose = MediaPlayer.IsRepeating;
            m_IsShuffleAfterClose = MediaPlayer.IsShuffled;
        }

        public void RestoreMediaState()
        {
            if (m_SongToPlayAfterClose != null && m_didPlayGameSong)
            {
                MediaPlayer.IsShuffled = m_IsShuffleAfterClose;
                MediaPlayer.IsRepeating = m_IsRepeatingAfterClose;
                MediaPlayer.Volume = m_VolumeAfterClose;
                MediaPlayer.Play(m_SongToPlayAfterClose);
            }
        }

        ~MusicPlayer()
        {
            Close();
            RestoreMediaState();
        }

        public void Open(string pFileName, uint uId)
        {
            if (null == pFileName || pFileName.Length == 0)
                return;

            Close();

            m_music = CCApplication.sharedApplication().content.Load<Song>(pFileName);

            m_nSoundID = uId;
        }

        public void Play(bool bLoop)
        {
            if (null != m_music)
            {
                MediaPlayer.IsRepeating = bLoop;
                MediaPlayer.Play(m_music);
                m_didPlayGameSong = true;
            }
        }

        public void Play()
        {
            Play(false);
        }

        public void Close()
        {
            if (IsPlaying() && m_didPlayGameSong)
            {
                Stop();
            }

            if (m_music != null)
            {
                m_music = null;
            }
        }

        /// <summary>
        /// Pauses the current song being played. 
        /// </summary>
        public void Pause()
        {
            MediaPlayer.Pause();
        }

        /// <summary>
        /// Resumes playback of the current song.
        /// </summary>
        public void Resume()
        {
            MediaPlayer.Resume();
        }

        /// <summary>
        /// Stops playback of the current song and resets the playback position to zero.
        /// </summary>
        public void Stop()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// resets the playback of the current song to its beginning.
        /// </summary>
        public void Rewind()
        {
            Song s = MediaPlayer.Queue.ActiveSong;

            Stop();

            if (null != m_music)
            {
                MediaPlayer.Play(m_music);
            }
            else if (s != null)
            {
                MediaPlayer.Play(s);
            }
        }

        /// <summary>
        /// Returns true if one of the game songs is playing.
        /// </summary>
        /// <returns></returns>
        public bool IsPlayingMySong()
        {
            if (!m_didPlayGameSong)
            {
                return (false);
            }
            if (MediaState.Playing == MediaPlayer.State)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if any song is playing in the media player, even if it is not one
        /// of the songs in the game.
        /// </summary>
        /// <returns></returns>
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
