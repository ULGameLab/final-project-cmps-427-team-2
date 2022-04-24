using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHadEnded = false;
    public float restartDelay = 3f;
   public void EndGame()
    {
        if (gameHadEnded == false)
        {
            gameHadEnded = true;
            Debug.Log("GameOver");
            Invoke("Restart", restartDelay);
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
