using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if (gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Ghost was hit!");
        }
    }
}
