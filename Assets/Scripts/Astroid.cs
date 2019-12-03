﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 2.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            _spawnManager.StartEnemyWave();
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
}