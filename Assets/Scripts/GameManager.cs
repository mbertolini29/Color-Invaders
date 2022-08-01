using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;

    Player player;

    [SerializeField] int score = 0;
    [SerializeField] Text scoreText;

    [SerializeField] int highestScore;
    [SerializeField] Text highestScoreText;

    [SerializeField] Text healthText;
    [SerializeField] GameObject LifeImg2;
    [SerializeField] GameObject LifeImg1;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreText.text = score.ToString();
        }
    }

     public int HighestScore
    {
        get { return highestScore; }
        set
        {
            highestScore = value;
            highestScoreText.text = highestScore.ToString();
        }
    }

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
            //DontDestroyOnLoad(sharedInstance);
        }
        else{
            Destroy(gameObject);
        }


    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        Score = player.myStats.score;
        HighestScore = player.myStats.highestScoreght;
    }

    public void UpdateUIHealth(int newHealth)
    {
        healthText.text = newHealth.ToString();

        switch (newHealth)
        {
            case 2:
                LifeImg2.SetActive(false);
                break;
            case 1:
                LifeImg1.SetActive(false);
                break;
        }
    }

    public void GameOver()
    {
        HighestScore = player.myStats.highestScoreght;

        if (Score >= HighestScore)
        {
            HighestScore = Score;
            player.myStats.highestScoreght = HighestScore;
            SaveManager saveManager = FindObjectOfType<SaveManager>();
            saveManager.Save();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Win()
    {
        player = FindObjectOfType<Player>();
        player.myStats.life = player.Health;
        player.myStats.score = Score;
        GetComponent<SaveManager>().Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {
        if (UnityEditor.EditorApplication.isPlaying == true &&
           UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode == false)
        {
            Player player = FindObjectOfType<Player>();
            player.myStats.life = 3;
            player.myStats.score = 0;
            GetComponent<SaveManager>().Save();
        }
    }
}
