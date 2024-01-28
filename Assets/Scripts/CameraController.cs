using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3[] positions;
    public Vector3[] rotations;
    private ObjectTeleporter objectTeleporter;
    public static CameraController cameraController;

    void Awake() {
        cameraController = this;
        objectTeleporter = new ObjectTeleporter(positions, rotations, this);
    }

    public void teleport(int idx) {
        objectTeleporter.Teleport(idx);
    }
}
