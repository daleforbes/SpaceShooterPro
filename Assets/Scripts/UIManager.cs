using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoText;

    [SerializeField]
    private Text _waveText;

    [SerializeField]
    private Sprite[] _livesSprite;

    [SerializeField]
    private Image _livesUIImage;

    [SerializeField]
    private GameObject _bossCointainer;
    private Image _bossHealthImage;
    
    [SerializeField]
    private Transform _noAmmoText, _lowAmmoText;
    
    [SerializeField]
    private float _flickerSpeed = .25f, _flickerSpeedFast = .15f;
    [SerializeField]
    private Text _restartText, _gameOverText;
    [SerializeField]
    private Transform _slowPanel;

    private bool _lowAmmoActive = false, _slowPanelActive = false;

    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    Coroutine _noAmmoRoutine = null, _lowAmmoRoutine = null, _slowPanelRoutine;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("UIManager cannot find GameManager!");
        }
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null) Debug.LogError("Cannot find Spawnmanager");

    }
    public void UpdateScore(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }
    public void UpdateAmmo(int curAmmo, int maxAmmo)
    {
        _ammoText.text = "Ammo: " + curAmmo.ToString() + "/" + maxAmmo.ToString();
    }
    public void UpdateLives(int lives)
    {

        _livesUIImage.sprite = _livesSprite[lives];
        if (lives <= 0)
        {
            DisplayGameOver("Game Over");
        }
        
    }
    private void DisplayGameOver(string gameOverText)
    {
        _restartText.gameObject.SetActive(true);
        _gameOverText.text = gameOverText;
        _gameManager.GameOver();
        StartCoroutine(FlickerGameOverText());
    }
    IEnumerator FlickerGameOverText()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(!_gameOverText.gameObject.activeSelf);
            yield return new WaitForSeconds(_flickerSpeed);
        }
        
    }
    public IEnumerator ShowWave(int wave)
    {
        if (wave == _spawnManager.bossWave)
        {
            _waveText.text = "Boss Wave";
        }
        else
        {
            _waveText.text = "Wave " + wave.ToString();
        }
        
        _waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        _waveText.gameObject.SetActive(false);
    }
    public void TurnOnLowAmmo()
    {
        if (!LowAmmoActive())
        {
            _lowAmmoRoutine = StartCoroutine(FlickerLowAmmoText());
            _lowAmmoActive = true;
        }
    }
    public void TurnOffLowAmmo()
    {
        if (LowAmmoActive())
            StopCoroutine(_lowAmmoRoutine);
            _lowAmmoText.gameObject.SetActive(false);
            _lowAmmoActive = false;
    }
    public void TurnOnSlowPanel()
    {
        if (!SlowPanelActive())
        {
            _slowPanelRoutine = StartCoroutine(FlickerSlowPanel());
            _slowPanelActive = true;
        }
    }
    public void TurnOffSlowPanel()
    {
        if (SlowPanelActive())
        {
            StopCoroutine(_slowPanelRoutine);
            _slowPanel.gameObject.SetActive(false);
            _slowPanelActive = false;
        }
    }
    IEnumerator FlickerLowAmmoText()
    {
        while (true)
        {
            _lowAmmoText.gameObject.SetActive(!_lowAmmoText.gameObject.activeSelf);
            yield return new WaitForSeconds(_flickerSpeed);
        }
    }
    IEnumerator FlickerSlowPanel()
    {
        while (true)
        {
            _slowPanel.gameObject.SetActive(!_slowPanel.gameObject.activeSelf);
            yield return new WaitForSeconds(_flickerSpeed);
        }
    }
    public bool LowAmmoActive()
    {
        return _lowAmmoActive;
    }
    public bool SlowPanelActive()
    {
        return _slowPanelActive;
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
    public void UpdateBossHealth(float bossLife)
    {
        _bossHealthImage.fillAmount = bossLife;
    }
    public void ActivateBossHealth()
    {
        _bossCointainer.SetActive(true);
        _bossHealthImage = _bossCointainer.transform.GetChild(2).GetComponent<Image>();
    }
    public void DeactivateBossHealth()
    {
        _bossCointainer.SetActive(false);
    }
    public void BossVictory()
    {
        // Display Victory screen
        DisplayGameOver("Victory!");
    }
}
