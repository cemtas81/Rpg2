using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    Canvas pauseCanvas, igiCanvas;
    public static bool isPaused = false;
    public AudioSource audio;
    public Image heart1, heart2, heart3;
    int keyAmount = 0;
    public TextMeshProUGUI keyAmountText;

    public void setHearts(int num)
    {
        if (num <= 2)
        {
            heart3.color = new Color(heart3.color.r, heart3.color.g, heart3.color.b, 0f);
        }
        if (num <= 1)
        {
            heart2.color = new Color(heart2.color.r, heart2.color.g, heart2.color.b, 0f);
        }
        if (num == 0)
        {
            heart1.color = new Color(heart1.color.r, heart1.color.g, heart1.color.b, 0f);
        }
    }

    public void incrementKeys(int i)
    {
        Debug.Log("Changing key amount!");
        keyAmount += i;
        if (keyAmount < 0)
            keyAmount = 0;
        string amountTxt = "x" + keyAmount;
        keyAmountText.text = amountTxt;
    }

    public void TogglePauseMenu()
    {
        if (pauseCanvas.enabled)
        {
            isPaused = false;
            pauseCanvas.enabled = false;
            if (audio != null)
                audio.Play();
            Time.timeScale = 1.0f;
        }
        else
        {
            isPaused = true;
            pauseCanvas.enabled = true;
            if (audio != null)
                audio.Pause();
            Time.timeScale = 0.0f;
        }
    }

    public void RestartLevel()
    {
        isPaused = false;
        //Application.LoadLevel(Application.loadedLevel);
        Scene currentScene = SceneManager.GetActiveScene();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitToMainMenu()
    {
        isPaused = false;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        Canvas[] canvasses = GetComponentsInChildren<Canvas>(true);
        pauseCanvas = canvasses[0];
        pauseCanvas.enabled = false;
        igiCanvas = canvasses[1];
        igiCanvas.enabled = true;

        //key = igiCanvas.GetComponent<Image>();
        //key.color = new Color(key.color.r, key.color.g, key.color.b, 0f);

        //TogglePauseMenu(); //There was a bug where after restarting you had to pause and then unpause to get stuff working again, this is a shitty fix.
    }

    // Update is called once per frame
    void Update()
    {
        scanForPause();
    }

    void scanForPause()
    {
        if (Input.GetKeyDown("escape"))
            TogglePauseMenu();
    }
}