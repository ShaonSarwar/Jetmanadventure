using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseSprite;
    [SerializeField] private GameObject playSprite;
    private bool isPaused; 
    // Start is called before the first frame update
    void Start()
    {
        pauseSprite.SetActive(true);
        playSprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseButton()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseSprite.SetActive(false);
            playSprite.SetActive(true); 
            Time.timeScale = 0.0f;
        }
        else
        {
            pauseSprite.SetActive(true);
            playSprite.SetActive(false);
            Time.timeScale = 1.0f; 
        }
    }

    //private void OnApplicationPause(bool pause)
    //{
    //    isPaused = true;
    //    Time.timeScale = 0.0f;
    //    Debug.Log("Paused");
    //}
}
