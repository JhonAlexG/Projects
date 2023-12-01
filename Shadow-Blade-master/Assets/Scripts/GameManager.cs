using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ControlsCanvas;
    public GameObject AttackCanvas;
    public GameObject DashCanvas;
    public GameObject PauseCanvas;

    public GameObject settingsAnimator;
    public GameObject creditsAnimator;

    public Slider volumeSlider;
    public Slider maxFPSSlider;

    public Text volumeValue;
    public Text maxFPSValue;

    private int valueForOtherScenes;

    // Start is called before the first frame update
    void Start()
    {
        if (valueForOtherScenes != 0)
        {
            volumeSlider.value = valueForOtherScenes;
        }

        AudioListener.volume = volumeSlider.value / 100f;
        Application.targetFrameRate = (int)maxFPSSlider.value;

        maxFPSSlider.maxValue = Screen.currentResolution.refreshRate;
        maxFPSSlider.value = Screen.currentResolution.refreshRate;

        volumeValue.text = volumeSlider.value.ToString();
        maxFPSValue.text = maxFPSSlider.value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseCanvas != null)
        {
            if (Time.timeScale == 0f)
            {
                PauseCanvas.SetActive(true);
            }
            else {
                PauseCanvas.SetActive(false);
            }
        }
    }

    // Load Scenes
    public void Play()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
    }

    public void Settings()
    {
        if (settingsAnimator.activeSelf)
        {
            settingsAnimator.SetActive(false);
        }
        else {
            settingsAnimator.SetActive(true);
        }
    }

    public void Credits()
    {
        if (creditsAnimator.activeSelf)
        {
            creditsAnimator.SetActive(false);
        }
        else {
            creditsAnimator.SetActive(true);
        }
    }

    public void Exit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }

    public void GoBackToGame()
    {
        Time.timeScale = 1f;
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Settings
    public void Volume()
    {
        int value = (int)volumeSlider.value;
        volumeValue.text = value.ToString();
        AudioListener.volume = value / 100f;

        if (GameObject.Find("Player") != null)
        {
            valueForOtherScenes = value;
        }
    }

    public void MaxFPS()
    {
        int value = (int)maxFPSSlider.value;
        maxFPSValue.text = value.ToString();
        Application.targetFrameRate = value;
    }

    // Controls
    public void NextForControls()
    {
        ControlsCanvas.SetActive(false);
        AttackCanvas.SetActive(true);
    }

    public void NextForAttack()
    {
        AttackCanvas.SetActive(false);
        DashCanvas.SetActive(true);
    }

    public void BackForAttack()
    {
        AttackCanvas.SetActive(false);
        ControlsCanvas.SetActive(true);
    }

    public void BackForDash()
    {
        DashCanvas.SetActive(false);
        AttackCanvas.SetActive(true);
    }
}
