using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public SoundChannelSO SFXChannel;
    public SoundChannelSO MusicChannel;
    private AudioSource _sfxSource;
    private AudioSource _musicSource;
    private void Awake()
    {
        SetupAudioSources();
        SFXChannel.SetListener(this);
        MusicChannel.SetListener(this);
    }

    /// <summary>
    /// AudioSource 초기화
    /// </summary>
    private void SetupAudioSources()
    {
        _sfxSource = gameObject.AddComponent<AudioSource>();
        _musicSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.playOnAwake = false;
        _musicSource.playOnAwake = false;
        _sfxSource.loop = false;
        _musicSource.loop = true;
        _musicSource.volume = .15f;
    }

    /// <summary>
    /// SFX 재생
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="source"></param>
    public void PlaySFX(SoundSO sound, AudioSource source = null)
    {
        sound.PlaySoundOneShot(source ? source : _sfxSource);
    }

    /// <summary>
    /// BGM 재생
    /// </summary>
    /// <param name="sound"></param>
    public void PlayMusic(SoundSO sound)
    {
        StopMusic();
        sound.PlayAsClip(_musicSource);
    }

    /// <summary>
    /// BGM 중지
    /// </summary>
    public void StopMusic()
    {
        _musicSource.Stop();
    }
}