using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MusicTrack", menuName = "MusicTrack")]
public class MusicTrackScriptable : ScriptableObject
{
    public AudioClip mainMusicClip;
    public bool loops = true;
}
