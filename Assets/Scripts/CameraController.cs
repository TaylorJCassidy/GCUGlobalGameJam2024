using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3[] positions;
    public Vector3[] rotations;
    private ObjectTeleporter objectTeleporter;

    void Awake() {
        objectTeleporter = new ObjectTeleporter(positions, rotations, this);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetButtonDown("Fire1")) { //TODO change button to change camera
        //    objectTeleporter.Teleport(1);
        // }
    }
}
