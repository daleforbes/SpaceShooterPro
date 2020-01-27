using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    private AudioSource _bgMusic, _bossMusic, _cannonSound;
    [SerializeField]
    private float _bossSpeed = 1.0f, _dodgeSpeed = .5f;
    [SerializeField]
    private Vector3 centerScreenPos = new Vector3(0f, 2.5f, 0f);
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireSpreadDelay = 7f, _fireFrequency = .3f;
    [SerializeField]
    private int _fireQty = 5;

    [SerializeField]
    private int _maxHealth = 10;
    private int _curHealth;

    private float _distanceToCenter;
    private bool _atCenter = false;
    private bool _bossDead = false;
    [SerializeField]
    private float _scatterAmt;
    private Vector3 _newPos;
    private float _distanceToTarget = 0f;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private Player _player;
    private Transform _bossCannon;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) Debug.LogError("Cannot find Player");

        _bgMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        if (_bgMusic == null) Debug.LogError("Cannot find Background AudioSource");
        _bgMusic.Stop();

        _bossMusic = GameObject.Find("BossMusic").GetComponent<AudioSource>();
        if (_bossMusic == null) Debug.LogError("Cannot find BossMusic AudioSource");
        _bossMusic.Play();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null) Debug.LogError("Cannot find UIManager");
        _uiManager.ActivateBossHealth();

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null) Debug.LogError("Cannot find Spawn Manager");

        _cannonSound = GetComponent<AudioSource>();
        
        _bossCannon = transform.Find("Cannon");

        _curHealth = _maxHealth;
        StartCoroutine(FireSpreadWave());
    }

    // Update is called once per frame
    void Update()
    {
        if (!_atCenter)
        {
            MoveToCenter();
        }
        else
        {
            DodgeMove();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser" && !_bossDead)
        {
            BossDamage();
            var explosion = Instantiate(_explosionPrefab, collision.transform.position, Quaternion.identity);
            Destroy(explosion.gameObject, 2.8f);
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Player" && !_bossDead)
        {
            _player.Damage();
        }
    }
    private void MoveToCenter()
    {
        _player.DisallowFire();
        _distanceToCenter = Vector3.Distance(transform.position, centerScreenPos);

        if (_distanceToCenter > .01f)
        {
            _atCenter = false;
            transform.position = Vector3.MoveTowards(transform.position, centerScreenPos, _bossSpeed * Time.deltaTime);
            _distanceToCenter = Vector3.Distance(transform.position, centerScreenPos);
        }
        else
        {
            _atCenter = true;
            _player.AllowFire();
        }
    }
    private void DodgeMove()
    {
        if(!_bossDead)
        {
            if (_distanceToTarget > .01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _newPos, _dodgeSpeed * Time.deltaTime);
                _distanceToTarget = Vector3.Distance(transform.position, _newPos);
            }
            else
            {
                _newPos = GetRandomPos();
                _distanceToTarget = Vector3.Distance(transform.position, _newPos);
            }
            
        }
    }
    private void BossDamage()
    {
        _curHealth--;
        if (_player != null) _player.AddScore(50); // Player may have been killed after laser was fired so don't add score

        if (_curHealth > 0)
        {
            float bossHealth = ((float)_curHealth) / _maxHealth;
            _uiManager.UpdateBossHealth(bossHealth);
        }
        else
        {
            KillBoss();
        }
        
    }
    private void KillBoss()
    {
        // Stop Dodging
        _bossDead = true;
        Destroy(_bossCannon.gameObject);
        // Disable HUD
        _uiManager.DeactivateBossHealth();
        
        _spawnManager.StopSpawning();
        // Explode in Children
        StartCoroutine(BossExplosion());
    }
    IEnumerator FireSpreadWave()
    {
        while(!_bossDead)
        {
            yield return new WaitForSeconds(_fireSpreadDelay);
            StartCoroutine(FireSpread());
        }
    }
    IEnumerator FireSpread()
    {
        for (int i = 0; i < _fireQty; i++)
        {
            yield return new WaitForSeconds(_fireFrequency);
            if (_bossDead || _player == null) break;
            var bossLaser = Instantiate(_laserPrefab, _bossCannon.transform.position, Quaternion.identity);
            _cannonSound.Play();
            EnemyLaser enemyLaser = bossLaser.GetComponent<EnemyLaser>();
            enemyLaser.BossShot();
        }
        
    }
    IEnumerator BossExplosion()
    {
        foreach (Transform child in transform)
        {
            yield return new WaitForSeconds(1f);
        
            var explosion = Instantiate(_explosionPrefab, child.transform.position, Quaternion.identity);
            Destroy(explosion.gameObject, 2.8f);
        }
        Destroy(this.gameObject,.2f);
        _uiManager.BossVictory();
    }

    private Vector3 GetRandomPos()
    {
        float x = Random.Range(-2.0f, 2.0f) * _scatterAmt;
        float y = Random.Range(2.0f, 4.0f) * _scatterAmt;
        var newPos = new Vector3(x, y, 0f);
        return newPos;
    }
}
