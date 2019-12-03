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

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _laserSpeed);
        if (transform.position.y < -3.0f)
        {
            if (transform.parent != null) Destroy(transform.parent.gameObject, 1f);
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Damaged Player");
            Player _player = collision.GetComponent<Player>();
            _player.Damage();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
