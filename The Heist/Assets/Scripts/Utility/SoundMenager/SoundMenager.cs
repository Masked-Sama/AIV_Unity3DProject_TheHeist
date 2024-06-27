using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundMenager : MonoBehaviour
{
    public static SoundMenager instance;

    [SerializeField]
    private AudioSource soundFXObject;

    private AudioSource currentBackGroundAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #region PublicMethods
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, audioClip.Length);

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip[rand];

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    #region BackGround Methods
    public void PlayBackgroundMusicLoop(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        if (audioClip.Count() == 0)
        {
            Debug.LogWarning("No background music clips found in SoundManager!");
            return;
        }
        if (currentBackGroundAudioSource == null)
        {
            currentBackGroundAudioSource = gameObject.AddComponent<AudioSource>();
        }


        // Randomly choose a background music clip
        int randomIndex = Random.Range(0, audioClip.Count());
        AudioClip chosenClip = audioClip[randomIndex];
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        currentBackGroundAudioSource.clip = chosenClip;
        currentBackGroundAudioSource.volume = 1f; // Adjust volume as needed
        currentBackGroundAudioSource.loop = true;  // Enable looping
        currentBackGroundAudioSource.Play();

        // Set up a coroutine to handle seamless looping
        StartCoroutine(HandleBackgroundMusicLoop(audioClip));
    }

    private IEnumerator HandleBackgroundMusicLoop(AudioClip[] audioClip)
    {
        while (true)
        {
            // Wait for the current clip to finish
            yield return new WaitForSeconds(currentBackGroundAudioSource.clip.length);

            // Randomly choose a new clip and seamlessly switch
            int randomIndex = Random.Range(0, audioClip.Count());
            AudioClip newClip = audioClip[randomIndex];

            currentBackGroundAudioSource.clip = newClip;
            currentBackGroundAudioSource.PlayScheduled(AudioSettings.dspTime); // Seamless transition
        }
    }
    #endregion

    #endregion
}
