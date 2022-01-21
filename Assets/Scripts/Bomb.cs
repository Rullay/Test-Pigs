using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float TimeToActivate;
    [SerializeField] private GameObject explosion;
    private bool isActive = true;
    void Start()
    {
        if (isActive == true)
        {
            Invoke("BombActivate", TimeToActivate);
            isActive = false;
        }
    }

   void BombActivate()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
