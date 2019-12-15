using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBurst : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _laserSpeed);
        if (transform.position.y > 8.0f)
        {
            if (transform.parent != null) Destroy(transform.parent.gameObject, 1f);
            Destroy(this.gameObject);
        }
    }
}
