using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BombTest : MonoBehaviour
{
    public Transform firePoint;
    public Camera mainCamera;
    public GameObject Bomb;

    private Vector3 destination;


    // Start is called before the first frame update
    void Start()
    {
        InstantiateBomb();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ThrowBomb();
        }
    }


    void ThrowBomb()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(1000);
        }

        InstantiateBomb();

    }

    void InstantiateBomb()
    {
        GameObject bombProjectile = Instantiate(Bomb, firePoint.position, Quaternion.identity); 
    }

}
