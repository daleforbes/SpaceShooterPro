using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _powerUpSpeed = 3f;
    
    [SerializeField]
    private int powerUpID;

    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _powerUpSpeed);
        if (transform.position.y < -3)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                switch (powerUpID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedUp();
                        break;
                    case 2:
                        player.ActivateShield();
                        break;
                    case 3:
                        player.ActivateAmmo();
                        break;
                    case 4:
                        player.ActivateHealth();
                        break;
                    case 5:
                        player.ActivateLaserBurst();
                        break;
                }
                Destroy(this.gameObject);
            }
        }
    }
}
