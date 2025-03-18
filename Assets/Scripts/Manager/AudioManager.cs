using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum EBGMType
{
    MainBGM,
}
public enum ESfxType
{
    ClearSound,
    PutSound,
    InkSound,
    LightSound,
    FolderSound,
}

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public List<AudioClip> BgmClips;
    public List<AudioClip> SfxClips;

    protected override void Awake()
    {
        base.Awake();

        if (bgmSource != null && sfxSource != null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        sfxSource.playOnAwake = false;
    }
    #region 배경음악
    public void PlayBGM(int bgm)
    {
        if (bgmSource.clip == BgmClips[bgm]) return; // 이미 재생중이면 무시
        bgmSource.clip = BgmClips[bgm];
        bgmSource.loop = true;
        bgmSource.Play();
    }
    public void PlayBGM(EBGMType bgm)
    {
        if (bgmSource.clip == BgmClips[(int)bgm]) return; // 이미 재생중이면 무시
        bgmSource.clip = BgmClips[(int)bgm];
        bgmSource.loop = true;
        bgmSource.Play();
    }
    #endregion
    #region 효과음
    public void PlaySFX(ESfxType type)
    {
        sfxSource.PlayOneShot(SfxClips[(int)type]);
    }
    public void PlaySFX(int typeNum)
    {
        sfxSource.PlayOneShot(SfxClips[typeNum]);
    }
    public void PlaySFXClip(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    #endregion
}
