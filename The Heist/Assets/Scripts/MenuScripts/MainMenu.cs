using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera settingsCamera;
    [SerializeField]
    private CinemachineVirtualCamera cameraBase;
    [SerializeField]
    private TMP_Text volumeTextValue = null;
    [SerializeField]
    private Slider volumeSlider = null;
    [SerializeField]
    private GameObject confirmationPrompt = null;

    //Graphic Settings
    private int _qualityLevel;
    private bool _isFullScreen;


    //resolution Dropdown
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [SerializeField]
    private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

   

    public void Setvolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0");
    }

    public void Apply()
    {
        PlayerPrefs.SetFloat("mastersettings", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
        PlayerPrefs.SetInt("masterquality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullScreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return ConfirmationBox();
        confirmationPrompt.SetActive(false);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    
}
