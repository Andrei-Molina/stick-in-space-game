public interface IAudioService
{
    void PlayMusic(string musicId, bool loop = true);
    void StopMusic();

    void PlaySFX(string sfxId);
    void StopSFX(string sfxId);
}
