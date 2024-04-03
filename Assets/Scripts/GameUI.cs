using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Gravitime.Timer; 

public class GameUI : MonoBehaviour
{
    [SerializeField] private PlayerCollision playerCollision;
    [SerializeField] private PlatformController platformController;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject finalTextObj;
    [SerializeField] private TextMeshProUGUI finalText;
    [SerializeField] private GameObject playAgainObj;
    [SerializeField] private GameObject fuelFinishedTextObj;
    [SerializeField] private AudioSource gameOverSound; 
    private int playerScore; 

    private void Awake()
    {
        playerScore = 0;
        scoreText.text = playerScore.ToString();
        finalText.text = playerScore.ToString(); 
        playerCollision.OnPlayerScoreChanged += PlayerCollision_OnPlayerScoreChanged;
        platformController.OnGameScoreChanged += PlatformController_OnGameScoreChanged;
        playerCollision.OnPlayerDie += PlayerCollision_OnPlayerDie;
        playerCollision.OnFuelFinished += PlayerCollision_OnFuelFinished;
    }

    private void PlayerCollision_OnFuelFinished(object sender, System.EventArgs e)
    {
        fuelFinishedTextObj.SetActive(true);
        FunctionTimer.Create(() =>
        {
            fuelFinishedTextObj.SetActive(false);
        }, 1.5f); 
    }

    private void PlayerCollision_OnPlayerDie(object sender, System.EventArgs e)
    {
        finalText.text = playerScore.ToString();
        finalTextObj.SetActive(true);
        UpdateHighScore(); 
        playAgainObj.SetActive(true);
        gameOverSound.Play(); 
    }

    private void PlatformController_OnGameScoreChanged(object sender, System.EventArgs e)
    {
        playerScore += 10;
       
        UpdateText(); 
    }

    private void PlayerCollision_OnPlayerScoreChanged(object sender, System.EventArgs e)
    {
        playerScore += 20;
        UpdateText(); 
    }   

    private void UpdateText()
    {
        scoreText.text = playerScore.ToString(); 
    }

    private void UpdateHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            int previousHiScore = PlayerPrefs.GetInt("HighScore");
            if (playerScore > previousHiScore)
            {
                PlayerPrefs.SetInt("HighScore", playerScore);            
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", playerScore);
        }
       
    }

    public void BackToMenuScene()
    {
        SceneManager.LoadScene(0); 
    }
}
