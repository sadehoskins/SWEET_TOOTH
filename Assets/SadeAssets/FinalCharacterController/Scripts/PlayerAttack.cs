using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform cam;
    public Transform attackPoint;
    public GameObject cake;

    public int totalThrows;
    public float throwCoolDown;

    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;

    private void Start()
    {
        readyToThrow = true;
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;
        
        // Instantiate Object to throw
        GameObject projectile = Instantiate(cake, attackPoint.position, cam.rotation);

        // Get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // Add force
        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce; // Throw in looking direction

        //projectile.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        // Throw cooldown
        Invoke(nameof(ResetThrow), throwCoolDown);

    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
