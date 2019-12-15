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
    private Transform _gameOverText, _noAmmoText, _lowAmmoText;
    [SerializeField]
    private float _flickerSpeed = .25f, _flickerSpeedFast = .15f;
    [SerializeField]
    private Text _restartText;

    private bool _lowAmmoActive = false;
    
    private GameManager _gameManager;
    Coroutine _noAmmoRoutine = null, _lowAmmoRoutine = null;

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
            StartCoroutine(FlickerGameOverText());
        }
        
    }
    IEnumerator FlickerGameOverText()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(!_gameOverText.gameObject.activeSelf);
            yield return new WaitForSeconds(_flickerSpeed);
        }
        
    }
    public void TurnOnLowAmmo()
    {
        if (_lowAmmoRoutine == null)
        {
            _lowAmmoRoutine = StartCoroutine(FlickerLowAmmoText());
            _lowAmmoActive = true;
        }
    }
    public void TurnOffLowAmmo()
    {
        if (_lowAmmoRoutine != null)
            StopCoroutine(_lowAmmoRoutine);
        _lowAmmoText.gameObject.SetActive(false);
        _lowAmmoActive = false;
    }
    IEnumerator FlickerLowAmmoText()
    {
        while (true)
        {
            _lowAmmoText.gameObject.SetActive(!_lowAmmoText.gameObject.activeSelf);
            yield return new WaitForSeconds(_flickerSpeed);
        }
    }
    public bool LowAmmoActive()
    {
        return _lowAmmoActive;
    }
    public void TurnOnNoAmmo()
    {
        _noAmmoRoutine = StartCoroutine(FlickerNoAmmoText());
    }
    public void TurnOffNoAmmo()
    {
        if (_noAmmoRoutine != null)
            StopCoroutine(_noAmmoRoutine);
        _noAmmoText.gameObject.SetActive(false);
    }
    IEnumerator FlickerNoAmmoText()
    {
        while (true)
        {
            _noAmmoText.gameObject.SetActive(!_noAmmoText.gameObject.activeSelf);
            yield return new WaitForSeconds(_flickerSpeedFast);
        }
    }
}
