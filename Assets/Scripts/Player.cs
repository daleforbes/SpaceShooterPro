using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private CameraShake _cameraShake;

    [SerializeField]
    private float _normalSpeed = 6f, _powerSpeed = 12f, _turboSpeed = 15f, _thrustChargeSpeed = .01f, _thrustBurnSpeed = .0001f;
    private float _currSpeed, _currThrustLevel = 1f;
    [SerializeField]
    private float _fireRate = .5f;
    private float _nextFire;
    private bool _laserBurstActive = false;
    
    [SerializeField]
    private int _maxAmmo = 15, _currAmmo = 99;

    private Coroutine _thrustRoutine = null;

    [SerializeField]
    private float _laserOffset = .8f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;

    [SerializeField]
    private AudioSource _powerUpSound, _laserSound, _gunEmptySound;

    // Prefabs
    [SerializeField]
    private GameObject _laserPrefab, _tripleShotPrefab, _explosionPrefab, _laserBurstPrefab;

    [SerializeField]
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private Transform _rightEngine, _leftEngine;
    private Transform _playerShield;
    private int _shieldCount = 3;
    private SpriteRenderer _shieldSpriteRender;
    private Image _thrustHUD;

    // Conditions
    [SerializeField]
    private bool isTripleShotActive = false, isSpeedPowerUpActive = false, _shieldsActive = false;

    public float _playerInputH, _playerInputV;

    // Components
    private SpriteRenderer _playerSprite;
    private Transform _thruster;
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
            Debug.LogError("UI Mananger not found by Player!");
        _thrustHUD = GameObject.Find("ThrusterChargeHUD").GetComponent<Image>();
        if (_thrustHUD == null)
            Debug.LogError("Can't find ThrustHUD object");
        _thruster = GameObject.Find("Thruster").GetComponent<Transform>();
        if (_thruster == null)
            Debug.LogError("Can't find Thruster");
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_cameraShake == null)
            Debug.LogError("Can't find Camera Shake");

        _currSpeed = _normalSpeed;
        _shieldCount = 0;

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
        else
        {
            _shieldSpriteRender = _playerShield.gameObject.transform.GetComponent<SpriteRenderer>();
            if (_shieldSpriteRender == null)
                Debug.LogError("Can't find Sprite Renderer for Shield");

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

        if (_currAmmo > 0)
        {
            if (isTripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
            }
            else
            {
                // LASER BURST
                if (_laserBurstActive)
                {
                    Instantiate(_laserBurstPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.Euler(0, 0, 25f));
                    Instantiate(_laserBurstPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.Euler(0, 0, -25f));

                    Instantiate(_laserBurstPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.Euler(0, 0, 50f));
                    Instantiate(_laserBurstPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.Euler(0, 0, -50f));

                    Instantiate(_laserBurstPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.Euler(0, 0, 75f));
                    Instantiate(_laserBurstPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.Euler(0, 0, -75f));

                    Instantiate(_laserBurstPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
                }
                
            }
            _laserSound.Play();
            _currAmmo--;
           
            // Flash LOW or NO AMMO if that was the last laser
            if (_currAmmo< 5 && _currAmmo > 0 && _uiManager.LowAmmoActive() == false)
            {
                _uiManager.TurnOnLowAmmo();
            }
            
            
            if (_currAmmo < 1)
            {
                if (_uiManager.LowAmmoActive())
                    _uiManager.TurnOffLowAmmo();
                _uiManager.TurnOnNoAmmo();
            }
        }
        else
        {
            _gunEmptySound.Play();
        }
        
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
        // Turbo speed when LEFT SHIFT is held down 
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _currSpeed = _turboSpeed;
        }
        else
        {
            _currSpeed = _normalSpeed;
        }
        
        _playerInputH = Input.GetAxis("Horizontal");
        _playerInputV = Input.GetAxis("Vertical");
        
        if (_thruster.gameObject.activeSelf)
        {
            transform.Translate(Vector3.right * _playerInputH * Time.deltaTime * _currSpeed);
            transform.Translate(Vector3.up * _playerInputV * Time.deltaTime * _currSpeed);
        }

        if ((_playerInputH != 0 || _playerInputV != 0) && _thrustHUD.fillAmount > 0)
        {
            if (_thrustRoutine != null)
            {
                StopCoroutine(_thrustRoutine);
                _thrustRoutine = null;
            }
            float burnRate = (_thrustBurnSpeed * (_currSpeed / 6)) * Time.deltaTime;
            _thrustHUD.fillAmount -= burnRate;
            if (_thrustHUD.fillAmount <= 0)
            {
                _thruster.gameObject.SetActive(false);
            }
        }
        else
        {
            if (_thrustRoutine == null)
            {
                _thrustRoutine = StartCoroutine(RechargeThrusters());
            }

        }


    }
    public void Damage()
    {
        StartCoroutine(_cameraShake.Shake(.15f, .4f));
        
        // If Shield Count is > 0, then shields are active
        if (_shieldCount > 0)
        {
            ShieldDamage();
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

        UpdateUILives();
    }

    private void UpdateUILives()
    {
        switch(_lives)
        {
            case 3:
                _rightEngine.gameObject.SetActive(false);
                _leftEngine.gameObject.SetActive(false);
                break;
            case 2:
                _rightEngine.gameObject.SetActive(true);
                _leftEngine.gameObject.SetActive(false);
                break;
            case 1:
                _leftEngine.gameObject.SetActive(true);
                break;

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
        _shieldCount = 3;
        _powerUpSound.Play();
        _playerShield.gameObject.SetActive(true);
        _shieldSpriteRender.color = Color.green;
    }
    public void ActivateAmmo()
    {
        _powerUpSound.Play();
        RefillAmmo();
    }
    public void ActivateLaserBurst()
    {
        _laserBurstActive = true;
        _powerUpSound.Play();
        StartCoroutine(StartLaserBurstPowerUp());
    }
    IEnumerator StartLaserBurstPowerUp()
    {
        yield return new WaitForSeconds(5f);
        _laserBurstActive = false;
    }
    private void ShieldDamage()
    {
        _shieldCount--;
        
        switch (_shieldCount)
        {
            case 2:
                _shieldSpriteRender.color = new Color(1f, .35f, 0f, 1f);
                break;
            case 1:
                _shieldSpriteRender.color = Color.red;
                break;
            case 0:
                _playerShield.gameObject.SetActive(false);
                break;
        }
        
    }
    public void ActivateHealth()
    {
        _lives++;
        if (_lives > 3)
            _lives = 3;
        
        _powerUpSound.Play();
        UpdateUILives();
    }

    IEnumerator StartTripleShot()
    {
        yield return new WaitForSeconds(5f);
        isTripleShotActive = false;
    }
    IEnumerator StartSpeedPowerUp()
    {
        _currSpeed = _powerSpeed;
        yield return new WaitForSeconds(5f);
        _currSpeed = _normalSpeed;
        isSpeedPowerUpActive = false;
    }
    public void AddScore(int points)
    {
        _score = _score + points;
        _uiManager.UpdateScore(_score);
    }
    public void RefillAmmo()
    {
        _uiManager.TurnOffLowAmmo();
        _uiManager.TurnOffNoAmmo();
        _currAmmo = _maxAmmo;
    }
    IEnumerator RechargeThrusters()
    {
        // Cool down before charge
        yield return new WaitForSeconds(1.5f);

        float chargeRate = _thrustChargeSpeed * Time.deltaTime;
        _thruster.gameObject.SetActive(true);

        while (_thrustHUD.fillAmount < 1f) 
        {
            yield return null;
            _thrustHUD.fillAmount += chargeRate;
        }
    }
}
