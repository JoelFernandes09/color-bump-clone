using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject levelStartUI, gameOverUI, levelCompleteUI, gameCompleteUI, pauseGameUI, ingameUI, rewardedAdText;
    [SerializeField] private TMPro.TextMeshProUGUI levelValueText, countdownValueText, ingameLevelText;
    [SerializeField] private AudioSource buttonClickSound, countdownSound, levelCompleteSound, gameCompleteSound, backgroundMusic, gameOverSound;
    private GoogleAdsManager googleAdsManager;
    private void Awake() {
        googleAdsManager = FindObjectOfType<GoogleAdsManager>();
    }
    public void StartGame()
    {
        StartCoroutine(StartGameTriggered());
    }

    IEnumerator StartGameTriggered()
    {
        buttonClickSound.Play();
        yield return new WaitForSeconds(buttonClickSound.clip.length);
        SceneManager.LoadScene("Main Game");
    }

    IEnumerator StartLevelCountdown()
    {
        
        countdownSound.Play();
        countdownValueText.text = "3";
        Debug.Log("3");
        yield return new WaitForSeconds(1f);
        countdownValueText.text = "2";
        Debug.Log("2");
        yield return new WaitForSeconds(1f);
        countdownValueText.text = "1";
        Debug.Log("1");
        yield return new WaitForSeconds(1f);
        Debug.Log("GO!");
        levelStartUI.SetActive(false);
        PlayerInfo.isGamePaused = false;
        backgroundMusic.Play();
    }

    public void ShowLevelStartUI()
    {
        levelStartUI.SetActive(true);
        levelValueText.text = "Level " + (PlayerInfo.currentLevel + 1);
        ingameLevelText.text = "Level " + (PlayerInfo.currentLevel + 1);
        StartCoroutine(StartLevelCountdown());
    }

    public void ShowGameOverUI()
    {
        backgroundMusic.Stop();
        gameOverSound.Play();
        gameOverUI.SetActive(true);
    }

    public void ShowLevelCompleteUI()
    {
        levelCompleteUI.SetActive(true);
        StartCoroutine(LevelCompleted());
    }

    IEnumerator LevelCompleted()
    {
        levelCompleteSound.Play();
        yield return new WaitForSeconds(levelCompleteSound.clip.length);
        SceneManager.LoadScene("Main Game");
    }

    public void ShowGameCompleteUI(bool rewardedAdLoaded)
    {
        if(!rewardedAdLoaded) rewardedAdText.SetActive(true);
        gameCompleteSound.Play();
        gameCompleteUI.SetActive(true);
    }

    public void PauseGame()
    {
        buttonClickSound.Play();
        ingameUI.SetActive(false);
        pauseGameUI.SetActive(true);
        PlayerInfo.isGamePaused = true;
    }

    public void ResumeGame()
    {
        buttonClickSound.Play();
        ingameUI.SetActive(true);
        pauseGameUI.SetActive(false);
        PlayerInfo.isGamePaused = false;
    }

    public void QuitGame()
    {
        buttonClickSound.Play();
        Application.Quit();
    }
}
