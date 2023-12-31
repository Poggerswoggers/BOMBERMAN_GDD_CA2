using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrow : MonoBehaviour
{
    public float explosionDelay = 3f;
    public float blastRadius = 4f;
    public float knockbackForce = 500f;

    private Material bombDefault;
    public Color flashingColour;
    private Color currentColour;
    private Color defaultColour;

    [SerializeField] private float colorChangeInterval = 1.5f;


    [SerializeField] float explosionCountdown;
    public bool startCD;
    bool hasExploded = false;

    public GameObject explosionEffect; //particle for explosion

    Rigidbody rigidBody;

    [Header("AduioClips")]    
    public AudioClip bombExplode;
    public AudioClip ticking;

    // Start is called before the first frame update
    void Start()
    {
        explosionCountdown = explosionDelay;
        rigidBody = GetComponent<Rigidbody>();
        bombDefault = GetComponent<Renderer>().material;
        currentColour = bombDefault.color;
        defaultColour = currentColour;
    }

    // Update is called once per frame
    void Update()
    {

        if (startCD)
        {
            explosionCountdown -= Time.deltaTime;

            if(explosionCountdown <= colorChangeInterval)
            {   
                
                currentColour = (currentColour == defaultColour) ? flashingColour : defaultColour;
                bombDefault.color = currentColour;

                if(bombDefault.color == flashingColour)
                {
                    GetComponent<AudioSource>().PlayOneShot(ticking);
                }

                colorChangeInterval *= 0.9f;
            }

            if(explosionCountdown < 0.01 && !hasExploded)
            {
                Explode(); 
                hasExploded = true;
            }
        }

    
      
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("ground"))
    //    {
    //        //start countdown for explosion only upon hitting ground or wall (tentative)
    //        startCD = true;
    //        rigidBody.velocity = Vector3.zero;
    //    }
    //    else startCD = false;
    // }

    private void OnCollisionEnter(Collision collision)
    {
        startCD = true;
        rigidBody.velocity = Vector3.zero; // stops it from moving after landing
    }

    public void Explode()
    {
        AudioSource.PlayClipAtPoint(bombExplode,transform.position);
        //bomb explosion visual
        Instantiate(explosionEffect, transform.position, transform.rotation);

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
                //apply damage
                float bombDistance = Vector3.Distance(nearbyObjects.transform.position, gameObject.transform.position);
                if(bombDistance < 1)
                {
                    nearbyObjects.GetComponent<PlayerController>().TakeDamage(40);

                }
                else if (bombDistance < 2)
                {
                    nearbyObjects.GetComponent<PlayerController>().TakeDamage(30);

                }
                else if (bombDistance < 3)
                {
                    nearbyObjects.GetComponent<PlayerController>().TakeDamage(20);

                }
                else if (bombDistance <= 4)
                {
                    nearbyObjects.GetComponent<PlayerController>().TakeDamage(10);

                }



            }
            if (nearbyObjects.GetComponent<IceWall>())
            {
                nearbyObjects.GetComponent<IceWall>().health=0;
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
