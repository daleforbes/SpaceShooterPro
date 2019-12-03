using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Image _livesUIImage;
    [SerializeField]
    private Transform _gameOverText;
    [SerializeField]
    private float _flickerSpeed = .25f;
    [SerializeField]
    private Text _restartText;
    
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("UIManager cannot find GameManager!");
        }
    }
    public void UpdateScore(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }
    public void UpdateLives(int lives)
    {
        _livesUIImage.sprite = _livesSprite[lives];
        if (lives <= 0)
        {
            _restartText.gameObject.SetActive(true);
            _gameManager.GameOver();
            StartCoroutine(FlickerText());
        }
        
    }
    IEnumerator FlickerText()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(!_gameOverText.gameObject.activeSelf);
            yield return new WaitForSeconds(_flickerSpeed);
        }
        
    }
}
