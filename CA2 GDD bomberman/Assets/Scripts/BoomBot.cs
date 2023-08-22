using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoomBot : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 player;

    public GameObject explosionEffect;
    public float blastRadius = 4f;
    public float knockbackForce = 2f;
    GameObject targetPlayer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    void Update()
    {
        player = targetPlayer.transform.position;
        agent.destination = player;

        float playerDist = Vector3.Distance(targetPlayer.transform.position, gameObject.transform.position);

        if(playerDist <= 3)
        {
            Explode();
        }

    }

   
    public void GetOtherPlayer(string playerNumber)
    {
        
        targetPlayer = GameObject.FindGameObjectWithTag(playerNumber);
      


    }

    void Explode()
    {
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
