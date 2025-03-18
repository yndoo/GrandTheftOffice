using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum EBGMType
{
    MainBGM,
    Clear,
}
public enum ESfxType
{
    ButtonSound,
    InkSound,
    LightSound,
    FolderSound,
    PrinterSound,
    InkBottle,
    PickUp,
    Drop,
    Click,
    Message,
    MouseClick,
    Buzzer,
    NodeSwitch,
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
        bgmSource.loop = true;
        bgmSource.volume = 0.5f;
        bgmSource.playOnAwake = false;
        sfxSource.volume = 0.6f;
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
