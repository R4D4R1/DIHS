using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class AudioPoolExample : MonoBehaviour
{
    public int InitialAudioSourcePoolSize = 3;
    public int TargetConcurrentSoundCap = 3;
    public int FadeOutSpeed = 10;
    private List<AudioSource> audioSourcePool = new();
 
    void Start()
    {
        for (int i = 0; i < InitialAudioSourcePoolSize; i++)
        {
            _ = AddNewSourceToPool();
        }
    }
 
 
    private AudioSource AddNewSourceToPool()
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        audioSourcePool.Add(newSource);
        return newSource;
    }
 
    private AudioSource GetAvailablePoolSource()
    {
        //Fetch the first source in the pool that is not currently playing anything
        foreach (var source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
 
        //No unused sources. Create and fetch a new source
        return AddNewSourceToPool();
    }
 
 
    public int PlayingPoolSourcesCount
    {
        get
        {
            int playingSourcesCount = 0;
 
            foreach (var source in audioSourcePool)
            {
                if (source.isPlaying)
                {
                    playingSourcesCount++;
                }
            }
 
            return playingSourcesCount;
        }
    }
 
    public void PlayClip(AudioClip clip)
    {
        AudioSource availableSource = GetAvailablePoolSource();
        availableSource.clip = clip;
        availableSource.Play();
 
 
        //Do we need to fade out any sources?
        if (PlayingPoolSourcesCount > TargetConcurrentSoundCap)
        {
            //Find the source that is closest to finishing
            float maxSourceTime = 0;
            AudioSource sourceClosestToFinishing = null;
            foreach (var source in audioSourcePool)
            {
                if (source.time > maxSourceTime)
                {
                    maxSourceTime = source.time;
                    sourceClosestToFinishing = source;
                }
            }
 
            StartCoroutine(FadeStopResetAudioSource(sourceClosestToFinishing));
        }
    }
 
    public IEnumerator FadeStopResetAudioSource(AudioSource source)
    {
        //temporarily remove it from the pool so that we don't pick it up in a subsequent check
        //and inadvertently start another fade coroutine
        audioSourcePool.Remove(source);
 
        //fade out
        while (!(Mathf.Approximately(source.volume, 0)))
        {
            source.volume -= FadeOutSpeed * Time.deltaTime;
            yield return null;
        }
 
        //stop and reset the volume
        source.Stop();
        source.volume = 1;
 
        //throw it back into the pool
        audioSourcePool.Add(source);
    }
}