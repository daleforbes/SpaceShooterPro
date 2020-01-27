using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Qutting Application");
            Application.Quit();
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
    }
    public bool GetGameOver()
    {
        return _isGameOver;
    }
}
