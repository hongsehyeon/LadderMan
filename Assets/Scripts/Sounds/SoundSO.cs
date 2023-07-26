using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오디오를 유용하게 사용하기 위한 스크립터블 오브젝트
/// 클립, 실행 함수 포함
/// </summary>
[CreateAssetMenu(menuName = "Sound/SoundClip", fileName = "SoundSO")]
public class SoundSO : ScriptableObject
{
    public List<AudioClip> SoundVariations;

    /// <summary>
    /// 클립들 중 하나를 한 번 재생
    /// </summary>
    /// <param name="source"></param>
    public void PlaySoundOneShot(AudioSource source)
    {
        if (SoundVariations.Count <= 0) { return; }
        source.PlayOneShot(SoundVariations[Random.Range(0, SoundVariations.Count)]);
    }

    /// <summary>
    /// 클립들 중 하나 재생
    /// </summary>
    /// <param name="source"></param>
    public void PlayAsClip(AudioSource source)
    {
        if (SoundVariations.Count <= 0) { return; }
        source.clip = SoundVariations[Random.Range(0, SoundVariations.Count)];
        source.Play();
    }
}