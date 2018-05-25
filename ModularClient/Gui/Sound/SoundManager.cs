using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Sound
{
    public static class SoundManager
    {
        private static IrrKlang.ISoundEngine engine { get; } = new IrrKlang.ISoundEngine();

        private static List<ISound> playingSounds { get; } = new List<ISound>();

        public static void PlaySounds(List<ISound> sounds)
        {
            if (sounds.Count == 0)
            {
                engine.StopAllSounds();
                playingSounds.Clear();
                return;
            }

            List<ISound> soundsToRemove = GetExclusive(playingSounds, sounds);
            List<ISound> soundsToAdd = GetExclusive(sounds, playingSounds);


            //only remove sounds if there are any sounds that loop, ie change of background noise
            //if there is no looping sounds we just have a sound effect
            if (sounds.Any(i => i.Loop))
            {
                foreach (ISound sound in soundsToRemove)
                {
                    for (int i = 0; i < playingSounds.Count; i++)
                    {
                        if (playingSounds[i].SoundName == sound.SoundName)
                        {
                            engine.RemoveSoundSource(sound.SoundName);
                            playingSounds.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            foreach (ISound sound in soundsToAdd)
            {
                string soundFileName = Path.Combine("Sounds", sound.SoundName);
                if (File.Exists(soundFileName))
                {
                    if (sound.Loop)
                    {
                        engine.Play2D(soundFileName, true);
                        playingSounds.Add(sound);
                    }
                    else
                    {
                        engine.Play2D(soundFileName);
                    }
                }
            }
        }

        private static List<ISound> GetExclusive(List<ISound> list1, List<ISound> list2)
        {
            List<ISound> exclusive = new List<ISound>();

            foreach (ISound sound1 in list1)
            {
                bool found = false;
                foreach (ISound sound2 in list2)
                {
                    if (sound1.SoundName == sound2.SoundName)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    exclusive.Add(sound1);
                }
            }

            return exclusive;
        }
    }
}
