using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputManagerScript : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
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
        GameObject playerObject = playerInput.gameObject;

        // Access the Camera component of the player
        Component[] componentsWithTag = playerObject.GetComponents<Component>();

        // Get the layer index for the "p2" layer
        int p2LayerIndex = LayerMask.NameToLayer("P2 Cam");

        foreach (Component component in componentsWithTag)
        {
            if (component.CompareTag("Cameras"))
            {
                // Set the layer of the component to the "p2" layer
                if (component.gameObject != null)
                {
                    component.gameObject.layer = p2LayerIndex;
                }
            }
        }
    }
}
