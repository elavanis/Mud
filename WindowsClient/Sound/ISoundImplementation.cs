using Shared.Sound.Interface;

namespace WindowsClient.Sound
{
    public interface ISoundImplementation
    {
        public void PlaySound(ISound sound);
        public void StopSound(ISound sound);

        public void StopAllSounds();
    }
}