using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource1;
    public AudioSource musicSource2;
    public MusicTrackScriptable mainMusic;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    public static AudioManager audioManager;

    private AudioSource currentMusicSource;
    private AudioSource otherMusicSource;
    private float crossFadeSpeed = 1f;
    private static bool doCrossFade = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (audioManager != null) Destroy(this.gameObject);
        audioManager = this;

        currentMusicSource = musicSource1;
        otherMusicSource = musicSource2;
        PlayMusic(mainMusic);

        LoadVolume();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (doCrossFade)
        {
            currentMusicSource.volume += Time.deltaTime * crossFadeSpeed;
            otherMusicSource.volume -= Time.deltaTime * crossFadeSpeed;
            if(currentMusicSource.volume >= 1)
            {
                otherMusicSource.volume = 0;
                FlipMusicSources();
                doCrossFade = false;
            }

        }
    }

    private void FlipMusicSources()
    {
        AudioSource newOtherSource = currentMusicSource;
        currentMusicSource = otherMusicSource;
        otherMusicSource = newOtherSource;
    }

    public void PlayMusic(MusicTrackScriptable aTrack)
    {
        if (currentMusicSource != null)
        {
            currentMusicSource.clip = aTrack.mainMusicClip;
            currentMusicSource.loop = aTrack.loops;
            currentMusicSource.Play();
        }
    }

    public void adjustMusicVolume()
    {
        adjustVolume("musicVol", musicSlider);
        
    }
    public void adjustSFXVolume()
    {
        adjustVolume("sfxVol", sfxSlider);
    }

    private void adjustVolume(string variable, Slider slider)
    {
        float sliderVolume = slider.value;
        float mixerVolume = Mathf.Log(sliderVolume) * 15;
        mixer.SetFloat(variable, mixerVolume);

        if (slider == musicSlider)
        {
            SaveManager.SetMusicVol(sliderVolume);
        }
        else
        {
            SaveManager.SetSFXVol(sliderVolume);
        }
    }

    private void LoadVolume()
    {
        
        musicSlider.value = SaveManager.GetMusicVol();
        sfxSlider.value = SaveManager.GetSFXVol();
        adjustMusicVolume();
        adjustSFXVolume();
        
        
    }

    public static void SetCrossFade(bool crossFade)
    {
        doCrossFade = crossFade;
    }
}
