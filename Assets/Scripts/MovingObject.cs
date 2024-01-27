using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // Setup Values
    Vector3 startPosition;
    Vector3 middlePosition;
    Vector3 endPosition = Vector3.negativeInfinity;
    float speed;
    bool active = false;
    
    // In case we want to end object movement
    public void Deactivate()
    {
        active = false;
    }

    // Activate Object Movement
    public void Activate(Vector3 position, float speed, float curveHeight)
    {
        startPosition = transform.position;
        this.endPosition = position;
        this.speed = speed;

        middlePosition = endPosition - startPosition + (Vector3.up * curveHeight);

        StartCoroutine(MoveTo());
    }

    private void FixedUpdate()
    {
        // Not Active
        if (!active)
            return;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                // Damage Player
                break;
            default:
                break;
        }
        Debug.Log(gameObject.name + " Hit: " + other.name);
        Destroy(gameObject);
    }

    IEnumerator MoveTo()
    {
        float time = 1 / speed;
        for (float currTime = 0; currTime < time; currTime += Time.deltaTime)
        {
            Debug.Log("Start is: " + startPosition + ", Middle is: " + middlePosition + " Final Point is: " + endPosition);
            float t = currTime / time;
            float y0 = Mathf.Lerp(startPosition.y, middlePosition.y, t);
            float y1 = Mathf.Lerp(middlePosition.y, endPosition.y, t);
            float y2 = Mathf.Lerp(y0, y1, t);

            Vector3 currPosition = Vector3.Lerp(startPosition, endPosition, t);
            currPosition.y = y2;

            transform.position = currPosition;
            yield return new WaitForFixedUpdate();
        }

        transform.position = endPosition;
    }
}
