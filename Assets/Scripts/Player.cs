using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _normalSpeed = 6f, _powerSpeed = 12f;
    private float _speed;
    [SerializeField]
    private float _fireRate = .5f;
    private float _nextFire;
    [SerializeField]
    private float _laserOffset = .8f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;

    [SerializeField]
    private AudioSource _powerUpSound, _laserSound;

    // Prefabs
    [SerializeField]
    private GameObject _laserPrefab, _tripleShotPrefab, _explosionPrefab;

    [SerializeField]
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private Transform _rightEngine, _leftEngine;
    private Transform _playerShield;

    // Conditions
    [SerializeField]
    private bool isTripleShotActive = false, isSpeedPowerUpActive = false, _shieldsActive = false;

    public float playerInput;

    // Components
    private SpriteRenderer _playerSprite;
    private BoxCollider2D _playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL!");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Mananger not found by Player!");
        }

        _speed = _normalSpeed;

        CheckPlayerChildren();
        CheckPlayerComponents();
    }

    private void CheckPlayerComponents()
    {
        _playerSprite = GetComponent<SpriteRenderer>();
        if (_playerSprite == null)
        {
            Debug.LogError("Player Sprite Renderer not Found!");
        }
        _playerCollider = GetComponent<BoxCollider2D>();
        if (_playerCollider == null)
        {
            Debug.LogError("Player Box Collider 2D not found!");
        }
    }

    private void CheckPlayerChildren()
    {
        _playerShield = this.gameObject.transform.GetChild(0);
        if (_playerShield == null)
        {
            Debug.LogError("No Player Shield Child Found!");
        }
        _rightEngine = this.gameObject.transform.GetChild(2);
        if (_playerShield == null)
        {
            Debug.LogError("No Right Engine Child Found!");
        }
        _leftEngine = this.gameObject.transform.GetChild(3);
        if (_playerShield == null)
        {
            Debug.LogError("No Left Engine Child Found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        WrapAroundScreen();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }
            
    }

    private void FireLaser()
    {
        _nextFire = Time.time + _fireRate;

        if (isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
        }
        _laserSound.Play();
    }

    private void WrapAroundScreen()
    {
        if (transform.position.x < -9)
        {
            transform.position = new Vector3(9, transform.position.y, 0);
        }
        else if (transform.position.x > 9)
        {
            transform.position = new Vector3(-9, transform.position.y, 0);
        }

        if (transform.position.y < -3)
        {
            transform.position = new Vector3(transform.position.x, 7, 0);
        }
        else if (transform.position.y > 7)
        {
            transform.position = new Vector3(transform.position.x, -3, 0);
        }
    }

    private void MovePlayer()
    {
        playerInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * playerInput * Time.deltaTime * _speed);

        playerInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * playerInput * Time.deltaTime * _speed);
    }
    public void Damage()
    {
        if (_shieldsActive)
        {
            DeactivateShield();
            return;
        }
        
        _lives--;

        if (_lives <= 0)
        {
            _spawnManager.StopSpawning();
            _playerCollider.gameObject.SetActive(false);
            _playerSprite.gameObject.SetActive(false);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 2.8f);
        }

        if (_lives == 2)
        {
            _rightEngine.gameObject.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.gameObject.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);
    }
    public void ActivateTripleShot()
    {
        isTripleShotActive = true;
        _powerUpSound.Play();
        StartCoroutine(StartTripleShot());
    }
    public void  ActivateSpeedUp()
    {
        isSpeedPowerUpActive = true;
        _powerUpSound.Play();
        StartCoroutine(StartSpeedPowerUp());
    }

    public void ActivateShield()
    {
        _shieldsActive = true;
        _powerUpSound.Play();
        _playerShield.gameObject.SetActive(true);
    }
    private void DeactivateShield()
    {
        _shieldsActive = false;
        _playerShield.gameObject.SetActive(false);
    }

    IEnumerator StartTripleShot()
    {
        yield return new WaitForSeconds(5f);
        isTripleShotActive = false;
    }
    IEnumerator StartSpeedPowerUp()
    {
        _speed = _powerSpeed;
        yield return new WaitForSeconds(5f);
        _speed = _normalSpeed;
        isSpeedPowerUpActive = false;
    }
    public void AddScore(int points)
    {
        _score = _score + points;
        _uiManager.UpdateScore(_score);
    }
}
