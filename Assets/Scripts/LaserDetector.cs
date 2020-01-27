using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetector : MonoBehaviour
{
    private Enemy _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = this.transform.parent.GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            int dodgeDir;
            if (collision.transform.position.x <= this.transform.position.x)
            {
                dodgeDir = 1;
            }
            else
            {
                dodgeDir = -1;
            }
            _enemy.StartDodge(dodgeDir);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            _enemy.StopDodge();
        }
    }
}
