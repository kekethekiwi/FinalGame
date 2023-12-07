using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource1;
    public MusicTrackScriptable menuMusic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(MusicTrackScriptable aTrack)
    {
        if (musicSource1 != null)
        {
            musicSource1.clip = aTrack.mainMusicClip;
            musicSource1.loop = aTrack.loops;
        }
    }
}
