using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float _laserSpeed;
    [SerializeField]
    private GameObject _explosionPrefab;
    private bool _laserUp = false;
    private bool _bossShot = false;
    private Vector3 _laserDir;
    private AudioSource _powerUpSound;
    private Transform _playerPos;


    // Update is called once per frame
    void Update()
    {
        if (_bossShot)
        {
            transform.position += transform.forward * _laserSpeed * Time.deltaTime;
        }
        else
        {
            if (_laserUp)
            {
                _laserDir = Vector3.up;
            }
            else
            {
                _laserDir = Vector3.down;
            }
            transform.Translate(_laserDir * Time.deltaTime * _laserSpeed);
        }

        if (transform.position.y > 9.0f || transform.position.y < -3.0f)
        {
            if (transform.parent != null) Destroy(transform.parent.gameObject, 1f);
            Destroy(this.gameObject);
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player _player = collision.GetComponent<Player>();
            _player.Damage();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if (collision.tag == "PowerUp")
        {
            StartCoroutine(FlashPowerUp(collision));
            _powerUpSound = GameObject.Find("ShieldsDown").GetComponent<AudioSource>();
            if (_powerUpSound == null) Debug.LogError("Cannot find ShieldsDown AudioSource");
            _powerUpSound.Play();
        }
    }
    IEnumerator FlashPowerUp(Collider2D powerUp)
    {
        int i = 0;
        while (i <= 3)
        {
            if (powerUp != null) powerUp.gameObject.SetActive(!powerUp.gameObject.activeSelf);
            yield return new WaitForSeconds(.04f);
            i++;
        }
        if (powerUp != null) Destroy(powerUp.gameObject);
        Destroy(this.gameObject);
    }
    public void SetLaserUp()
    {
        _laserUp = true;
    }
    public void BossShot()
    {
        _bossShot = true;
        var _playerObject = GameObject.Find("Player");
        if (_playerObject == null) return;

        _playerPos = _playerObject.GetComponent<Transform>();

        // Aim toward player
        transform.rotation = Quaternion.LookRotation(_playerPos.position);
        
    }
}
