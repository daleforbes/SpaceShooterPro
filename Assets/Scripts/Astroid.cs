using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 2.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private Text _cToCollectText;
    private SpawnManager _spawnManager;
    private Player _player;

    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
            Debug.LogError("Can't find Player from Astroid");
        _cToCollectText.gameObject.SetActive(true);
        StartCoroutine(DisplayCtoCollect());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            _player.RefillAmmo();
            _spawnManager.StartEnemyWave();
            _cToCollectText.gameObject.SetActive(false);
            Destroy(this.gameObject, 0.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateAstroid();
    }

    private void RotateAstroid()
    {
        transform.Rotate(0f, 0f, _rotateSpeed * Time.deltaTime);
    }
    IEnumerator DisplayCtoCollect()
    {
        yield return new WaitForSeconds(5.0f);
        _cToCollectText.gameObject.SetActive(false);
    }
}
