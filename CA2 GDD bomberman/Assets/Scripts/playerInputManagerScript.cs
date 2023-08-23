using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputManagerScript : MonoBehaviour
{
    private PlayerInputManager playerInputManager;

    public List<string> camLayer;
    public int stringInt;
    public LayerMask player1Mask;
    public LayerMask player2Mask;

    // Start is called before the first frame update
    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }


    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += HandlePlayerJoin;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= HandlePlayerJoin;
    }

    public void HandlePlayerJoin(PlayerInput playerInput)
    {
        Debug.Log("Ran");
        // Get the GameObject representing the player
        Transform playerObject = playerInput.gameObject.transform.parent;
        Debug.Log(playerObject);

        //
        for (int i= 0; i<playerObject.childCount; i++)
        {
            Transform childTransform = playerObject.transform.GetChild(i);
            if (childTransform.tag == "Camera")
            {
                int layerInt = LayerMask.NameToLayer(camLayer[stringInt]);
                
       
                childTransform.gameObject.layer = layerInt;
            }
        }

        Camera playerCam = playerObject.GetComponentInChildren<Camera>();

        if (stringInt == 0)
        {
            playerCam.cullingMask = ~player2Mask;
        }
        else
        {
            playerCam.cullingMask = ~player1Mask;
        }


        stringInt++;
    }

}
