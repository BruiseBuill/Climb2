using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Single<AudioManager>
{
    [SerializeField] List<string> voiceNames;
    [SerializeField] List<AudioClip> voiceClips;
    [SerializeField] float voiceVolume;
    Dictionary<string, AudioClip> voiceDic = new Dictionary<string, AudioClip>();
    //
    [SerializeField] List<string> musicName;
    [SerializeField] List<AudioClip> musicClip;
    [SerializeField] float musicVolume;
    Dictionary<string, AudioClip> musicDic = new Dictionary<string, AudioClip>();
    //
    [SerializeField] List<string> vfxName;
    [SerializeField] List<AudioClip> vfxClip;
    [SerializeField] float vfxVolume;
    Dictionary<string, AudioClip> vfxDic = new Dictionary<string, AudioClip>();
    //
    AudioSource voiceSource;
    AudioSource musicSource;
    AudioSource vfxSource;

    private void Awake()
    {
        voiceSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        vfxSource = gameObject.AddComponent<AudioSource>();
        //
        musicSource.loop = true;
        //
        for (int i = 0; i < musicClip.Count; i++)
        {
            musicDic.Add(musicName[i], musicClip[i]);
        }
        for (int i = 0; i < vfxClip.Count; i++)
        {
            vfxDic.Add(vfxName[i], vfxClip[i]);
        }
    }
    private void Start()
    {
        PlayMusic();
    }
    //
    public void PlayVoice(string name)
    {
        voiceSource.clip = voiceDic[name];
        voiceSource.volume = voiceVolume;
        voiceSource.Play();
    }
    public void PlayVoice(string name,float volume)
    {
        voiceSource.clip = voiceDic[name];
        voiceSource.volume = voiceVolume * volume;
        voiceSource.Play();
    }
    public void PauseVoice()
    {
        voiceSource.Stop();
    }
    //
    public void PlayMusic()
    {
        musicSource.clip = musicClip[0];
        musicSource.volume = musicVolume;
        musicSource.Play();
    }
    public void PlayMusic(string name)
    {
        musicSource.clip = musicDic[name];
        musicSource.Play();
    }
    public void PauseMusic()
    {
        musicSource.Stop();
    }
    //
    public void PlayVFX(string name)
    {
        vfxSource.volume = vfxVolume;
        vfxSource.PlayOneShot(vfxDic[name]);
    }
    public void PauseVFX()
    {
        vfxSource.Stop();
    }
    //
    private void OnDisable()
    {
        PauseMusic();
        PauseVFX();
    }
}
