using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip mainSound;
    public AudioClip clearSound;
    public AudioClip putSound;
    public AudioClip inkSound;
    public AudioClip lightSound;
    public AudioClip folderSound;

    public void start()
    {
        mainSound = Resources.Load<AudioClip>("Sounds/MainSound");
        clearSound = Resources.Load<AudioClip>("Sounds/clearSound");
        putSound = Resources.Load<AudioClip>("Sounds/putSound");
        inkSound = Resources.Load<AudioClip>("Sounds/inkSounds");
        lightSound = Resources.Load<AudioClip>("Sounds/lightSourd");
        folderSound = Resources.Load<AudioClip>("Sounds/folderSound");

        if (bgmSource != null && sfxSource != null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

    }


}
