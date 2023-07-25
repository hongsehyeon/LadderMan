using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������� �����ϰ� ����ϱ� ���� ��ũ���ͺ� ������Ʈ
/// Ŭ��, ���� �Լ� ����
/// </summary>
[CreateAssetMenu(menuName = "Sound/SoundClip", fileName = "SoundSO")]
public class SoundSO : ScriptableObject
{
    public List<AudioClip> SoundVariations;

    /// <summary>
    /// Ŭ���� �� �ϳ��� �� �� ���
    /// </summary>
    /// <param name="source"></param>
    public void PlaySoundOneShot(AudioSource source)
    {
        if (SoundVariations.Count <= 0) { return; }
        source.PlayOneShot(SoundVariations[Random.Range(0, SoundVariations.Count)]);
    }

    /// <summary>
    /// Ŭ���� �� �ϳ� ���
    /// </summary>
    /// <param name="source"></param>
    public void PlayAsClip(AudioSource source)
    {
        if (SoundVariations.Count <= 0) { return; }
        source.clip = SoundVariations[Random.Range(0, SoundVariations.Count)];
        source.Play();
    }
}