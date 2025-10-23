public interface IAudioManager
{
    void PlayBGM(string name, bool isloop);
    void StopBGM();
    void PlaySE(string name);
}