using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _beamObject;
    [SerializeField]
    private Transform _explosionPos;
    void Start()
    {
        _beamObject.gameObject.SetActive(false);
        StartCoroutine(FlashBeam());
    }

    // Update is called once per frame
    IEnumerator FlashBeam()
    {
        int i = 0;
        while (i <= 3)
        {
            _beamObject.gameObject.SetActive(!_beamObject.gameObject.activeSelf);
            yield return new WaitForSeconds(.1f);
            i++;
        }
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player _player = collision.GetComponent<Player>();
            _player.Damage();
            Instantiate(_explosionPrefab, _explosionPos.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
