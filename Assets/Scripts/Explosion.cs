using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    void Start()
    {
        Invoke("explosion", 1);
    }

    void explosion()
    {
        Destroy(gameObject);
    }


}
