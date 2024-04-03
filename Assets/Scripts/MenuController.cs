using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject highScoreObject; 
    [SerializeField] private TextMeshProUGUI highScoreText;
    private int playerHighScore; 
    // Start is called before the first frame update
    void Start()
    {
        highScoreObject.SetActive(false); 

        if (PlayerPrefs.HasKey("HighScore"))
        {
            playerHighScore = PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            playerHighScore = 0;
        }

        highScoreText.text = playerHighScore.ToString();

        if (playerHighScore > 0)
        {
            highScoreObject.SetActive(true); 
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(1); 
    }
}
