using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler2 : MonoBehaviour
{
    public void ChangeScene(int sceneIndex_p)
    { SceneManager.LoadScene(sceneIndex_p); }

    public void Quit()
    { Application.Quit();
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false; // stops playmode
    #endif
    }

    #region Audio

    public AudioMixer masterAudio;
    public string currentSlider;
    public Slider tempSlider;

    public void GetSlider(Slider slider_p)
    {
        tempSlider = slider_p;
    }

    public void MuteToggle(bool isMuted_p)
    {
        if (isMuted_p)
        {
            masterAudio.SetFloat(currentSlider, -80);
            tempSlider.interactable = false;
        }
        else
        {
            masterAudio.SetFloat(currentSlider, tempSlider.value);
            tempSlider.interactable = true;
        }
    }

    public void CurrentSlider(string sliderName_p)
    {
        currentSlider = sliderName_p;
    } 

    public void ChangeVolume(float volume_p)
    {
        masterAudio.SetFloat(currentSlider, volume_p);
    }
    #endregion

    #region Quality
    public void Quality(int qualityIndex_p)
    {
        QualitySettings.SetQualityLevel(qualityIndex_p);
    }

    #endregion

    #region Resolution
    public Resolution[] resolutions;
    public Dropdown resDropdown;

    public void FullscreenToggle(bool isFullscreen_p)
    {
        Screen.fullScreen = isFullscreen_p;
    }

    public void Start()
    {
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
            resDropdown.AddOptions(options);
            resDropdown.value = currentResolutionIndex;
            resDropdown.RefreshShownValue();
        }
    }

    public void SetResolution(int resolutionIndex_p)
    {
        Resolution res = resolutions[resolutionIndex_p];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
    #endregion
}
