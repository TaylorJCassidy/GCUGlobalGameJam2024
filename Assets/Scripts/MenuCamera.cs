using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Transform followTransform;
    public float rotationSpeed = 2.0f;
    public float rotationTime = 30f ;


    private Vector3 cameraOffset;
    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion camTurnAngle = Quaternion.AngleAxis(10 * rotationSpeed, Vector3.up);

        cameraOffset = camTurnAngle * cameraOffset;
       
        Vector3 newPos = followTransform.position + cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
        transform.LookAt(followTransform);

    }
}
