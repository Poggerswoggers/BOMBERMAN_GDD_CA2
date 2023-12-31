using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoomBot : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 player;

    public GameObject explosionEffect;
    public float maxExplosionCD = 2f;
    public float explosionCD = 2f;
    public float blastRadius = 4f;
    public float knockbackForce = 2f;
    public GameObject targetPlayer; //do not set

    bool startPriming = false; // start to explode countdown
    [Header("Color Stuff")]
    private Material bombDefault;
    public Color flashingColour;
    private Color currentColour;
    private Color defaultColour;
    [SerializeField] private float colorChangeInterval = 1.5f;

    [Header("AudioClips")]
    public AudioClip bombExplode;
    public AudioClip ticking;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        explosionCD = maxExplosionCD;
        bombDefault = GetComponent<Renderer>().material;
        currentColour = bombDefault.color;
        defaultColour = currentColour;
    }

    void Update()
    {
        AudioSource movingaudio = GetComponent<AudioSource>();
        if (!movingaudio.isPlaying)
        {
            movingaudio.Play();
        }
        player = targetPlayer.transform.position;
        agent.destination = player;

        float playerDist = Vector3.Distance(targetPlayer.transform.position, gameObject.transform.position);

        if(playerDist <= 3)
        {
            startPriming = true;
            //Explode();
            //GetComponent<AudioSource>().PlayOneShot(ticking);
        }
        if (startPriming)
        {
            explosionCD -= Time.deltaTime;
            if (explosionCD <= colorChangeInterval)
            {

                currentColour = (currentColour == defaultColour) ? flashingColour : defaultColour;
                bombDefault.color = currentColour;

                if (bombDefault.color == flashingColour)
                {
                    GetComponent<AudioSource>().PlayOneShot(ticking);
                }

                colorChangeInterval *= 0.9f;
            }


        }
        if (explosionCD <= 0)
        {
            Explode();
        }

    }
        
   
  //  public void GetOtherPlayer(string playerNumber)
  //  {
  //      Debug.Log(playerNumber);
  //      PlayerController[] players = FindObjectsOfType<PlayerController>();
  //
    //    foreach (PlayerController player in players)
      //  {
        //    if(player.currentPlayer.ToString() == playerNumber)
          //  {
          //      targetPlayer = player.gameObject;
           // }
       // }
        //Debug.Log(targetPlayer);


    //}

    public void GetOtherPlayer(string playerNumber)
    {
        //GameObject targetTag;
      

        Debug.Log(playerNumber);
        //FindObjectOfType<PlayerController>().tag = playerNumber;
       // targetPlayer = GameObject.FindGameObjectWithTag(playerNumber);
        targetPlayer = GameObject.FindWithTag(playerNumber);

        Debug.Log(targetPlayer);
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
                //apply damage
                float bombDistance = Vector3.Distance(nearbyObjects.transform.position, gameObject.transform.position);
                if (bombDistance < 1)
                {
                    nearbyObjects.GetComponent<PlayerController>().TakeDamage(30);

                }
                else if (bombDistance < 2)
                {
                    nearbyObjects.GetComponent<PlayerController>().TakeDamage(25);

                }
                else if (bombDistance < 3)
                {
                    nearbyObjects.GetComponent<PlayerController>().TakeDamage(20);

                }
                else if (bombDistance <= 4)
                {
                    nearbyObjects.GetComponent<PlayerController>().TakeDamage(15);

                }



            }
            if (nearbyObjects.GetComponent<IceWall>())
            {
                nearbyObjects.GetComponent<IceWall>().health = 0;
            }
            if(nearbyObjects.GetComponent<BoomBot>())
            {
                Destroy(nearbyObjects.gameObject);
            }

        }

        //removes bomb after exploded
        Destroy(gameObject);


    }
}

 //    GameObject FindClosestPlayer()
 //   {
 //       // Find all game objects tagged as Player
 //       GameObject[] targets;
 //       targets = GameObject.FindGameObjectsWithTag("Player");
 //       GameObject closestPlayer = null;
 //       var distance = Mathf.Infinity;
 //       Vector3 position = transform.position;
//
//        // Iterate through them and find the closest one
//        foreach (GameObject target in targets)
 //       {
 //           var difference = (target.transform.position - position);
 //           var curDistance = difference.sqrMagnitude;
   //         if (curDistance < distance)
 //       {
  //              closestPlayer = target;
   //             distance = curDistance;
    //        }
     //   }

        //return closestPlayer;
    //}
