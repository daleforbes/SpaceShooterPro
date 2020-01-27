using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammingField : MonoBehaviour
{
    private Enemy _enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyDiver")
        {
            _enemy = collision.GetComponent<Enemy>();
            if (_enemy == null) Debug.LogError("Can't find enemy in Ramming Field on enter");
            _enemy.StartRamPlayer();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "EnemyDiver")
        {
            _enemy = collision.GetComponent<Enemy>();
            if (_enemy == null) Debug.LogError("Can't find enemy in Ramming Field on exit");
            _enemy.StopRamPlayer();
        }
    }
}
