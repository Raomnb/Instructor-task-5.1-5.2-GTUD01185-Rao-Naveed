using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverText;
    public GameObject restart;
    public TextMeshProUGUI highScoreDisp;
    public TextMeshProUGUI livesDisp;
    public TextMeshProUGUI scoreDisp;
    public GameObject barrel;
    public int score;
    private  int highScore=0;
    public int lives = 3;
    public bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ThrowBarrel()); // spawn barrels
        highScore = PlayerPrefs.GetInt("High Score"); // get saved high score
    }

    // Update is called once per frame
    void Update()
    {
       
        if(score > highScore)
        {
            highScore = score; // if score is greater than high score then make score the new high score
            PlayerPrefs.SetInt("High Score", highScore);  // save high score of user
        } 
        scoreDisp.text = "Score : " + score; // shows current score on ui
        highScoreDisp.text = "High Score : " + highScore; // shows high screen on ui
        livesDisp.text = "Lives : " + lives; // shows lives on ui
        if (lives <= 0)
        {
            gameOver = true; // set gameover boolean true;
            StopAllCoroutines(); // Stop spawning barrels
            gameOverText.gameObject.SetActive(true); // Enables Game Over Text on Screen
            restart.gameObject.SetActive(true); // Enables Restart Button
        }

    }
    IEnumerator ThrowBarrel()
    {
        while(!gameOver)
        {
            Instantiate(barrel, barrel.transform.position, barrel.transform.rotation); // spawn 1 instance of barrel in game
            yield return new WaitForSeconds(Random.Range(3,7)); // gives delay in spawning barrels from 3 to 6 seconds
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // on getting restart button pressed reload the current scene
    }
}
