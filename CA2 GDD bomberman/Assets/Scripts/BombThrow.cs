using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrow : MonoBehaviour
{
    public float explosionDelay = 3f;
    public float blastRadius = 5f;
    public float knockbackForce = 2f;


    float explosionCountdown;
    public bool startCD;
    bool hasExploded = false;

    public GameObject explosionEffect; //particle for explosion

    Rigidbody rigidBody;


    // Start is called before the first frame update
    void Start()
    {
        explosionCountdown = explosionDelay;
        rigidBody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (startCD)
        {
            explosionCountdown -= Time.deltaTime;

            if(explosionCountdown < 0.01 && !hasExploded)
            {
                Explode(); 
                hasExploded = true;
            }
        }

    
      
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            //start countdown for explosion only upon hitting ground or wall (tentative)
            startCD = true;
            rigidBody.velocity = Vector3.zero;
        }
        else startCD = false;
    }

    public void Explode()
    {
        //bomb explosion visual
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //obtain all objects hit by grenade explosion
        Collider[] hit = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObjects in hit)
        {
            //bomb knockback
            Rigidbody rb = nearbyObjects.GetComponent<Rigidbody>();
            if(rb != null) // only if the object hit has a rigidbody component
            {
                rb.AddExplosionForce(knockbackForce, transform.position, blastRadius);
            }
            if (nearbyObjects.GetComponent<PlayerController>())
            {
                //apply damage
            }

           
        }

        //removes bomb after exploded
        Destroy(gameObject);
        

    }
}
