using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource BGMAudioSource;
    [SerializeField] GameObject SFXAudioSourceParent;

    private List<AudioSource> listSFXAudioSource = new();
    private List<int> listSFXAudioSourceReady = new();
    private List<int> listSFXAudioSourceBusy = new();

    [SerializeField] List<SFXAudioClip> listSFXAudioClip = new();
    [SerializeField] List<BGMAudioClip> listBGMAudioClip = new();
    private Dictionary<SFXType, AudioClip> dictSFXAudioClip = new();
    private Dictionary<BGMType, AudioClip> dictBGMAudioClip = new();

    private void Awake()
    {
        InitDict();
    }

    private void InitDict()
    {
        for (int i = 0; i < listSFXAudioClip.Count; i++)
        {
            dictSFXAudioClip.Add(listSFXAudioClip[i].type, listSFXAudioClip[i].clip);
        }

        for (int i = 0; i < listBGMAudioClip.Count; i++)
        {
            dictBGMAudioClip.Add(listBGMAudioClip[i].type, listBGMAudioClip[i].clip);
        }
    }

    public void PlayBGM(BGMType type)
    {
        BGMAudioSource.clip = GetBGMClip(type);
        BGMAudioSource.loop = true;
        BGMAudioSource.Play();
    }    

    public void PlaySFX(SFXType type)
    {
        int audioSourceIndex = GetAudioSourceReady();

        listSFXAudioSource[audioSourceIndex].clip = GetSFXClip(type);
        listSFXAudioSource[audioSourceIndex].Play();

        StartCoroutine(ReturnAudioSourceReady(audioSourceIndex, listSFXAudioSource[audioSourceIndex]));
    }

    private int GetAudioSourceReady()
    {
        if (listSFXAudioSourceReady.Count == 0)
        {
            AudioSource newAudioSource = SFXAudioSourceParent.AddComponent<AudioSource>();
            listSFXAudioSourceReady.Add(listSFXAudioSource.Count);
            listSFXAudioSource.Add(newAudioSource);
        }

        int audioSourceIndex = listSFXAudioSourceReady[0];
        listSFXAudioSourceBusy.Add(audioSourceIndex);
        listSFXAudioSourceReady.RemoveAt(0);

        return audioSourceIndex;
    }

    private IEnumerator ReturnAudioSourceReady(int audioSourceIndex, AudioSource audioSource)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        listSFXAudioSourceBusy.Remove(audioSourceIndex);
        listSFXAudioSourceReady.Add(audioSourceIndex);
    }

    private AudioClip GetSFXClip(SFXType type)
    {
        return dictSFXAudioClip.TryGetValue(type, out var clip) ? clip : null;
    }

    private AudioClip GetBGMClip(BGMType type)
    {
        return dictBGMAudioClip.TryGetValue(type, out var clip) ? clip : null;
    }
}


[System.Serializable]
public class SFXAudioClip
{
    public SFXType type;
    public AudioClip clip;
}

[System.Serializable]
public class BGMAudioClip
{
    public BGMType type;
    public AudioClip clip;
}