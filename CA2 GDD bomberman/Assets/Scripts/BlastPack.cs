using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastPack : MonoBehaviour
{
    Rigidbody rigidBody;


    public GameObject explosionEffect;
    public float blastRadius = 4f;
    public float knockbackForce = 1000f;
    public float upwardsModifier = 3f;

    //[Header("AduioClips")]
    //public AudioClip bombExplode;
    //public AudioClip ticking;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
        rigidBody.velocity = Vector3.zero;
//Debug.Log("BlastPack");
    }


    void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        //AudioSource.PlayClipAtPoint(bombExplode, transform.position);

        //obtain all objects hit by grenade explosion
        Collider[] hit = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObjects in hit)
        {
            //bomb knockback
            Rigidbody rb = nearbyObjects.GetComponent<Rigidbody>();
            if (rb != null) // only if the object hit has a rigidbody component
            {
                rb.AddExplosionForce(knockbackForce, transform.position, blastRadius, upwardsModifier);
            }
            if (nearbyObjects.GetComponent<PlayerController>())
            {
               

                

            }
            
            

        }

        //removes bomb after exploded
        Destroy(gameObject);


    }
}
