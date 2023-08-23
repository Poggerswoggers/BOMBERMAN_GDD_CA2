using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastPack : MonoBehaviour
{
    public GameObject explosionEffect;
    public float blastRadius = 4f;
    public float knockbackForce = 4f;

    [Header("AduioClips")]
    public AudioClip bombExplode;
    //public AudioClip ticking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(bombExplode, transform.position);

        //obtain all objects hit by grenade explosion
        Collider[] hit = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObjects in hit)
        {
            //bomb knockback
            Rigidbody rb = nearbyObjects.GetComponent<Rigidbody>();
            if (rb != null) // only if the object hit has a rigidbody component
            {
                rb.AddExplosionForce(knockbackForce, transform.position, blastRadius);
            }
            if (nearbyObjects.GetComponent<PlayerController>())
            {
               

                

            }
            
            if (nearbyObjects.GetComponent<BoomBot>())
            {
                Destroy(nearbyObjects.gameObject);
            }

        }

        //removes bomb after exploded
        Destroy(gameObject);


    }
}
