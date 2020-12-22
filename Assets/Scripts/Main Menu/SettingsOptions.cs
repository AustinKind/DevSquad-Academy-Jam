using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

public class SettingsOptions : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;
    public Slider volumeSlider;
    public Dropdown qualityDropDown;
    List<string> options;
    Resolution[] resolutions;

    private int screenInt;

    private const string QUALITY_INDEX = "qualityIndex";
    private const string RESOLUTION_NAME = "resolution";

    void Awake() {
        screenInt = PlayerPrefs.GetInt("togglestate", 1);
        if (screenInt == 1) {
            fullScreenToggle.isOn = true;
        } else {
            fullScreenToggle.isOn = false;
        }

        resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(index =>
        {
            PlayerPrefs.SetInt(RESOLUTION_NAME, resolutionDropdown.value);
            PlayerPrefs.Save();
        }));
        qualityDropDown.onValueChanged.AddListener(new UnityAction<int>(index =>
        {
            PlayerPrefs.SetInt(QUALITY_INDEX, qualityDropDown.value);
            PlayerPrefs.Save();
        }));
    }

    void Start () {

        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 0f);
        audioMixer.SetFloat("masterVolume", PlayerPrefs.GetFloat("masterVolume"));
        qualityDropDown.value = PlayerPrefs.GetInt(QUALITY_INDEX, 0);


        resolutions = Screen.resolutions;
        options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + "x" + resolutions[i].height + " @ " + resolutions[i].refreshRate + "Hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt(RESOLUTION_NAME, currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution (int resolutionIndex) {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume (float volume) {
        PlayerPrefs.SetFloat("masterVolume", volume);
        audioMixer.SetFloat("masterVolume", volume);
    }

    public void SetQuality (int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
        if (isFullscreen)
            PlayerPrefs.SetInt("togglestate", 1);
        else
            PlayerPrefs.SetInt("togglestate", 0);
    }
}
