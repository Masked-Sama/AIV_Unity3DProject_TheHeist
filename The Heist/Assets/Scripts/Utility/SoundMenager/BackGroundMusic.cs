using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip backGroundMusicFX;

    [SerializeField]
    private AudioClip[] backgroundMusicClips;

    [SerializeField] 
    float volume;
    void Start()
    {
        if (backgroundMusicClips == null)
        {
            SoundMenager.instance.PlaySoundFXClip(backGroundMusicFX, transform, volume);
            return;
        }
        SoundMenager.instance.PlayBackgroundMusicLoop(backgroundMusicClips, transform, volume);
    }

}
