using NAudio.Wave;
using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsClient.Sound
{
    public class NAudioSound : ISoundImplementation
    {
        /*
        private ISound _sound;
        private bool loop = true;

        public NAudioSound(ISound sound)
        {
            _sound = sound;

            if (_sound .Loop)
            {
                loop = true;
            }

            Start();
        }
      
        private void Start()
        {
            Task.Run(() =>
            {
                using (var audioFile = new AudioFileReader(_sound.SoundName))
                {
                    using (var outputDevice = new WaveOutEvent())
                    {
                        outputDevice.Init(audioFile);
                        outputDevice.Play();
                        while (outputDevice.PlaybackState == PlaybackState.Playing)
                        {
                            Thread.Sleep(1);
                        }
                    }
                }
            });
        }

        public void Stop()
        {

        }
        */

        private Dictionary<string, NAudioSoundSound> activeSounds = new Dictionary<string, NAudioSoundSound>();

        public NAudioSound()
        {

        }


        public void PlaySound(ISound sound)
        {
            PruneOldSounds();

            if (activeSounds.ContainsKey(sound.SoundName))
            {
                activeSounds.Remove(sound.SoundName);
            }

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            Task t = Task.Run(() =>
            {
                using (var audioFile = new AudioFileReader(Path.Combine("Sounds", sound.SoundName)))
                {
                    using (var outputDevice = new WaveOutEvent())
                    {
                        outputDevice.Init(audioFile);
                        do
                        {
                            audioFile.Position = 0;
                            outputDevice.Play();
                            while (outputDevice.PlaybackState == PlaybackState.Playing && !token.IsCancellationRequested)
                            {
                                Thread.Sleep(1);
                            }
                        } while (sound.Loop && !token.IsCancellationRequested);
                    }
                }
            });

            NAudioSoundSound nAudioSoundSound = new NAudioSoundSound() { Task = t, TokenSource = cancellationTokenSource };

            activeSounds.Add(sound.SoundName, nAudioSoundSound);
        }


        public void StopSound(ISound sound)
        {
            StopSound(sound.SoundName);
        }


        public void StopSound(string soundName, bool prune = true)
        {
            if (prune)
            {
                PruneOldSounds();
            }

            if (activeSounds.TryGetValue(soundName, out NAudioSoundSound activeSound))
            {
                activeSound.TokenSource.Cancel();
                activeSound.Task.Wait();
                activeSounds.Remove(soundName);
            }
        }

        public void StopAllSounds()
        {
            PruneOldSounds();

            foreach (var sound in activeSounds.Keys)
            {
                StopSound(sound, false);
            }
        }


        private void PruneOldSounds()
        {
            foreach (var item in activeSounds.Keys)
            {
                if (activeSounds[item].Task.IsCompleted)
                {
                    activeSounds.Remove(item);
                }
            }
        }

        public class NAudioSoundSound
        {
            public Task Task { get; set; }
            public CancellationTokenSource TokenSource { get; set; }
        }
    }
}
