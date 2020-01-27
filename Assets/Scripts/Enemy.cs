//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int _enemyID;
    [SerializeField]
    private GameObject _laserPrefab;
    
    [SerializeField]
    private float _enemySpeed = 4.0f;
    
    private Player _player;
    private Animator _animator;
    
    [SerializeField]
    private bool _isDead = false;
    
    private AudioSource _explosionSound;
    private AudioSource _shieldSound;
    private Vector3 _enemyDirection;
    private Quaternion _prevRotation;
    
    [SerializeField]
    private float _angle = 2f;
    
    [SerializeField]
    private float _zigzagWaveSpeed = 10f, _zigzagWaveSize = 2.0f, _zigzagVertSpeed = 3.0f;
    
    private Vector3 _zigzagPos;
    private bool _zigzagLeft = false;
    private Transform _enemyShield;
    
    [SerializeField]
    private bool _shieldDestroyed = false;
    
    private bool _ramPlayer = false;
    private bool _dodging = false;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) Debug.LogError("Can't find Player");
        CalculateDirection();
        
        _explosionSound = GetComponent<AudioSource>();
        _shieldSound = GameObject.Find("ShieldsDown").GetComponent<AudioSource>();
        if (_shieldSound == null) Debug.LogError("Cannot find ShieldsDown AudioSource");
        _animator = gameObject.GetComponent<Animator>();
        _zigzagPos = transform.position;
        int i = Random.Range(1, 3);
        if (i == 1)
        {
            _zigzagLeft = true;
        }
        else
        {
            _zigzagLeft = false;
        }
        StartCoroutine(SpawnLaser());
    }
    
    void Update()
    {
        EnemyMovement();
        WrapEnemyPosition();
        CheckForPowerUp();
    }

    private void CheckForPowerUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up * 6);
        
        if (hit.collider != null && hit.transform.tag == "PowerUp")
        {
            StartCoroutine(FireAtPowerUp());
        }
    }
    IEnumerator FireAtPowerUp()
    {
        var enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if (_player == null) Debug.LogError("Can't find Player object to add score!");
            
            if (!_isDead)
            {
                if (this.transform.childCount > 0 && _enemyID == 3 && _shieldDestroyed == false)
                {
                    _shieldDestroyed = true;
                    _shieldSound.Play();
                    _enemyShield = this.transform.GetChild(0);
                    StartCoroutine(FlashShield());
                }
                else
                {
                    if (_player != null) _player.AddScore(10); // Player may have been killed after laser was fired so don't add score
                    KillEnemy();
                }
                Destroy(other.gameObject);
            }
        }
        else if (other.tag == "Player")
        {
            _player = other.GetComponent<Player>();
            if (_player != null && !_isDead)
            {
                _player.Damage();
                KillEnemy();
            }
        }
    }

    IEnumerator FlashShield()
    {
        int i = 0;
        while (i <= 3)
        {
            if (_enemyShield != null) _enemyShield.gameObject.SetActive(!_enemyShield.gameObject.activeSelf);
            yield return new WaitForSeconds(.04f);
            i++;
        }
        if (_enemyShield != null) Destroy(_enemyShield.gameObject);
    }
    public void StartRamPlayer()
    {
        _ramPlayer = true;
    }
    public void StopRamPlayer()
    {
        _ramPlayer = false;
    }

    private void CalculateDirection()
    {
        switch(_enemyID)
        {
            case 1:
            case 3:
            case 4:
            case 5:
            case 6:
                int angle = Random.Range(1, 4);
                switch (angle)
                {
                    case 1:
                        _angle *= -1;
                        break;
                    case 2:
                        _angle = 0;
                        break;
                    case 3:
                        _angle *= 1;
                        break;
                        
                }
                
                _enemyDirection = new Vector3(_angle, -1f, 0f);
                
                _prevRotation = this.transform.rotation;
                break;
        }
        
    }
    
    private void WrapEnemyPosition()
    {
        if (transform.position.y < -3)
        {
            _zigzagPos = new Vector3(Random.Range(-9f, 9f), 7, 0);
            transform.position = new Vector3(Random.Range(-9f, 9f), 7, 0);
        }
    }
    private void KillEnemy()
    {
        _animator.SetTrigger("OnEnemyDeath");
        _explosionSound.Play();
        _isDead = true;
        
        // Destroy shield if there is one
        if (this.gameObject.transform.childCount > 0)
        {
            _enemyShield = this.transform.GetChild(0);
            Destroy(_enemyShield.gameObject);
        }
        
        Destroy(this.gameObject, 2.8f);
    }
    private void EnemyMovement()
    {
        switch (_enemyID)
        {
            case 1:
            case 3:
            case 5:
                transform.Translate(_enemyDirection * Time.deltaTime * _enemySpeed);
                break;
            
            case 2:
                _zigzagPos -= transform.up * Time.deltaTime * _zigzagVertSpeed;
                
                // Start Zig Zag left/right randomly to break up uniformity
                if (_zigzagLeft)
                {
                    transform.position = _zigzagPos + transform.right * Mathf.Sin(Time.time * _zigzagWaveSpeed) * _zigzagWaveSize;
                }
                else
                {
                    transform.position = _zigzagPos - transform.right * Mathf.Sin(Time.time * _zigzagWaveSpeed) * _zigzagWaveSize;
                }
                break;
            case 4:
                
                // Try to ram player if within the "Ram Field" - Circle collider child of Player
                if (_ramPlayer)
                {
                    var dir = transform.position - _player.transform.position;
                    var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    
                    angle -= 90; // Angle nose of enemy toward player
                    transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0,0,1));

                    transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _enemySpeed * Time.deltaTime);
                }
                else
                {

                    transform.rotation = _prevRotation;
                    transform.Translate(_enemyDirection * Time.deltaTime * _enemySpeed);
                }
                break;
            case 6:
                if (_dodging)
                {
                    _enemyDirection.x = _angle;
                }
                transform.Translate(_enemyDirection * Time.deltaTime * _enemySpeed);
                break;
        }
        
    }
    IEnumerator SpawnLaser()
    {
        while (!_isDead)
        {
            yield return new WaitForSeconds(Random.Range(.5f, 3f));

            if (!_isDead)
            {
                var enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

                if (_enemyID == 2)
                {
                    enemyLaser.transform.SetParent(this.transform);
                }
                else if (_enemyID == 5)
                {
                    if (_player == null) yield break; // Dont' try to check Player pos if player is already dead
                    if (_player.transform.position.y > transform.position.y)
                    {
                        enemyLaser.GetComponent<EnemyLaser>().SetLaserUp();
                    }
                    
                }
            }
        }
    }
    public void  StartDodge(int dodgeDir)
    {
        _dodging = true;
        _angle = 0.4f * dodgeDir;
    }

    public void StopDodge()
    {
        _dodging = false;
    }
}
