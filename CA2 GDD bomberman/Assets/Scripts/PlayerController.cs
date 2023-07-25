using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CameraControl cameraControlRef;
    public int moveSpeed = 15;


    CharacterController cc;

    public bool holdingBomb;
    public Transform firePoint;
    public GameObject Bomb;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        StartCoroutine(InstantiateBomb());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (dir.magnitude >= 0.1f)
        {
            Vector3 displacement = transform.TransformDirection(dir.normalized);
            cc.Move(displacement * moveSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButton(1))
        {
            cameraControlRef.AdsMode(true);
        }
        else
        {
            cameraControlRef.AdsMode(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            ThrowBomb();
        }
        
    }

    void ThrowBomb()
    {
        if (holdingBomb)
        {
            Debug.Log("Throw Bomb");
            firePoint.GetChild(0).GetComponent<BombProjectile>().fireBomb();
            holdingBomb = false;
        }
        StartCoroutine(InstantiateBomb());
    }

    IEnumerator InstantiateBomb()
    {
        if (!holdingBomb)
        {
            holdingBomb = true;
            Debug.Log("Begin Bomb Animation");
            yield return new WaitForSeconds(0.5f);
            GameObject _bomb = Instantiate(Bomb, firePoint.position, Quaternion.identity);
            _bomb.transform.SetParent(firePoint);
        }
    }
}
