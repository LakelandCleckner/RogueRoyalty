using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    public GameObject player;
    public Camera camera;
    public Transform cameraTransform;
    public GameObject playerMarker;

    private void Update()
    {
        // Follow the player's position
        Vector3 newPosition = new Vector3(player.transform.position.x, camera.transform.position.y, player.transform.position.z);
        camera.transform.position = newPosition;

        // Correctly rotate the playerMarker to match the player's Y-axis rotation
        // This aligns the marker's Z rotation with the player's Y rotation, suitable for a 2D minimap representation
        float playerYRotation = player.transform.eulerAngles.y;
        playerMarker.transform.localRotation = Quaternion.Euler(0, 0, -playerYRotation);
    }
}
