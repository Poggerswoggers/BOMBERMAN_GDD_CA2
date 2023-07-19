using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cam;
    public int moveSpeed = 15;
    public float sensX;

    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (dir.magnitude >= 0.1f)
        {
            //float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 displacement = transform.TransformDirection(dir.normalized);
            cc.Move(displacement * moveSpeed * Time.deltaTime);

            transform.Rotate(0, Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime, 0);

        }

    }
}
