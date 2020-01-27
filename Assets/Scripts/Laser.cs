using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float _laserSpeed;
    private bool _homing = false;
    private Transform _closestEnemy = null;
    private bool _isBossWave = false;
    private SpawnManager _spawnManager;
    private Transform _bossPos;

    // Update is called once per frame
    void Update()
    {
        if (_homing)
        {
            HomingLaser();
        }
        else
        {
            transform.Translate(Vector3.up * Time.deltaTime * _laserSpeed);
            
            // Destroy laser if it travels outside boundaries of the screen
            if (transform.position.y > 8.0f || transform.position.y < -4.0f || transform.position.x < -11f || transform.position.x > 11f)
            {
                if (transform.parent != null) Destroy(transform.parent.gameObject, 1f);
                Destroy(this.gameObject);
            }
        }
        
    }
    public void HomeIt()
    {
        _homing = true;

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null) Debug.LogError("Cannot find SpawnManager");

        _isBossWave = _spawnManager.IsBossWave();
        
        if (_isBossWave)
        {
            var bossObject = GameObject.Find("Boss(Clone)"); ;
            if (bossObject == null)
            {
                Debug.Log("Cannot find Boss in HomeIt!");
                _homing = false;
                return;
            }
            else
            {
                _bossPos = bossObject.GetComponent<Transform>();
            }
            _closestEnemy = _bossPos;
        }
        else
        {
            GetClosestEnemy();
        }
    }

    private void GetClosestEnemy()
    {

        float distanceToClosestEnemy = Mathf.Infinity;
        _closestEnemy = null;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
        if (allEnemies.Length == 0)
        {
            _homing = false;
            return;
        }

        foreach (Enemy curEnemy in allEnemies)
        {
            float distanceToEnemy = (curEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                _closestEnemy = curEnemy.transform;
            }
        }
        
    }

    void HomingLaser()
    {
        if (_isBossWave)
        {
            if (_bossPos != null) _closestEnemy.transform.position = _bossPos.position;
        }
        else
        {
            if (_closestEnemy == null)
            {
                GetClosestEnemy(); //Enemy was destroyed after this laser locked on so get another
                return;
            }
        }
        
        if (_closestEnemy == null)
        {
            _homing = false;
            return;
        }

        var dir = transform.position - _closestEnemy.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        angle -= 90; // Angle nose of laser toward enemy
        transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));

        transform.position = Vector3.MoveTowards(transform.position, _closestEnemy.transform.position, _laserSpeed * Time.deltaTime);
    }
}
