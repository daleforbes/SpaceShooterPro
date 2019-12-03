//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _enemySpeed = 4.0f;
    private Player _player;
    private Animator _animator;
    private bool _isDead = false;
    private AudioSource _explosionSound;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _explosionSound = GetComponent<AudioSource>();
        _animator = gameObject.GetComponent<Animator>();
        StartCoroutine(SpawnLaser());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if (_player == null)
            {
                Debug.LogError("Can't find Player object to add score!");
            }
            else
            {
                if (!_isDead)
                {
                    if (_player != null) _player.AddScore(10); // Player may have been killed after laser was fired so don't add score
                    KillEnemy();
                    Destroy(other.gameObject);
                }
                
            }
            
            
        }
        else if (other.tag == "Player")
        {
            Debug.Log("Damaged Player");
            _player = other.GetComponent<Player>();
            if (_player != null && !_isDead)
            {
                _player.Damage();
                KillEnemy();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        WrapEnemyPosition();
    }

    private void WrapEnemyPosition()
    {
        if (transform.position.y < -3)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 7, 0);
        }
    }
    private void KillEnemy()
    {
        _animator.SetTrigger("OnEnemyDeath");
        _explosionSound.Play();
        _isDead = true;
        Destroy(this.gameObject, 2.8f);
    }
    private void EnemyMovement()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _enemySpeed);
    }
    IEnumerator SpawnLaser()
    {
        while (!_isDead)
        {
            yield return new WaitForSeconds(Random.Range(.5f, 3f));
            
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Debug.Log("New enemy laser instantiated");
        }
    }
}
