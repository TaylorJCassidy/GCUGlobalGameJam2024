using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    Vector3 dir = Vector3.zero;
    public float speed = 1.0f;
    public void Activate(Vector3 dir, float speed)
    {
        this.dir = dir;
        this.speed = speed;
    }
    private void FixedUpdate()
    {
        if (dir == Vector3.zero)
            return;
        transform.position += dir * speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tongue"))
        {
           ReverseDir();
        }
        else Destroy(gameObject);
    }

    public void ReverseDir()
    {
        //dir = -dir;
       dir = Vector3.zero;
       GetComponent<Rigidbody>().isKinematic = true;
    }
}
