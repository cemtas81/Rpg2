using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Rendering;

public class MainMenu : MonoBehaviour
{

    public GameObject DefaultButtons;
    public GameObject Settings;
    public GameObject Credits;
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public Slider volumeSlider;
    public Toggle FullScreenToggle;
    public Toggle ButtonSwapToggle;

    Resolution[] resolutions;

    private void Start()
    {
        LoadSettings();

        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToList().ToArray();
        resolutionDropdown.ClearOptions();

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

        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !DefaultButtons.activeSelf)
        {
            Back();
        }
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
            LoadVolume(PlayerPrefs.GetFloat("Volume"));
        if (PlayerPrefs.HasKey("Fullscreen"))
            LoadFullscren(PlayerPrefs.GetInt("Fullscreen") == 1);
        if (PlayerPrefs.HasKey("resolutionWidth") && PlayerPrefs.HasKey("resolutionHeight"))
            SetResolution(PlayerPrefs.GetInt("resolutionWidth"), PlayerPrefs.GetInt("resolutionHeight"));
        if (PlayerPrefs.HasKey("swapButtons"))
            LoadSwapActionButtons(PlayerPrefs.GetInt("swapButtons") == 1);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        DefaultButtons.SetActive(false);
        Settings.SetActive(true);
    }

    public void OpenCredits()
    {
        DefaultButtons.SetActive(false);
        Credits.SetActive(true);
    }

    public void Back()
    {
        Settings.SetActive(false);
        Credits.SetActive(false);
        DefaultButtons.SetActive(true);
        PlayerPrefs.Save();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void LoadVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        volumeSlider.value = volume;
    }

    public void SetFullscren(bool IsFullscreen)
    {
        Screen.fullScreen = IsFullscreen;
        PlayerPrefs.SetInt("Fullscreen", IsFullscreen ? 1 : 0);
    }

    public void LoadFullscren(bool IsFullscreen)
    {
        Screen.fullScreen = IsFullscreen;
        FullScreenToggle.isOn = IsFullscreen;
    }

    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionWidth", resolution.width);
        PlayerPrefs.SetInt("resolutionHeight", resolution.height);
    }

    public void SetResolution(int width, int height)
    {
        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    public void SetSwapActionButtons(bool SwapButtons)
    {
        PlayerPrefs.SetInt("swapButtons", SwapButtons ? 1 : 0);
    }

    public void LoadSwapActionButtons(bool SwapButtons)
    {
        ButtonSwapToggle.isOn = SwapButtons;
    }
}
