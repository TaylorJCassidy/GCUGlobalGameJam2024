using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    ObjectTeleporter objectTeleporter;
    void Awake() {
        Vector3[] positions = {
            new Vector3(0f, 3.2f, -5.53f),
            new Vector3(5f, 7f, 0f),
            new Vector3(0f, 5.5f, 8f)
        };
        Vector3[] rotations = {
            new Vector3(0f, 0f ,0f),
            new Vector3(30f, 315f, 0f),
            new Vector3(20f, 180f, 0f)
        };
        objectTeleporter = new ObjectTeleporter(positions, rotations, this);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Fire1")) { //TODO change button to change camera
        //    objectTeleporter.Teleport(true);
        //}
    }
}
