using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using UnityEditor;
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
    private void Update()
    {
        DrawBezierCurve(.0001f);
    }
    void DrawBezierCurve(float resolution)
    {
        int numPoints = Mathf.CeilToInt(1f / resolution);
        GetComponent<LineRenderer>().positionCount = numPoints;
        for (int i = 0; i < numPoints; i++)
        {
            float t = i * resolution;
            Vector3 point = CalculatePointOnCurve(t);
            GetComponent<LineRenderer>().SetPosition(i, point);
        }
    }
    Vector3 CalculatePointOnCurve(float t)
    {
        float y0 = Mathf.Lerp(startPosition.y, middlePosition.y, t);
        float y1 = Mathf.Lerp(middlePosition.y, endPosition.y, t);
        float y2 = Mathf.Lerp(y0, y1, t);

        Vector3 currPosition = Vector3.Lerp(startPosition, endPosition, t);
        currPosition.y = y2;
        return currPosition;
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
            case "Throwable":
                break;
            default:
                Destroy(gameObject);
                break;
        }
        Debug.Log(gameObject.name + " Hit: " + other.name);
    }

    IEnumerator MoveTo()
    {
        GameObject landingPos = GameObject.CreatePrimitive(PrimitiveType.Cube);
        landingPos.transform.position = endPosition;

        float time = 1 / speed;
        Destroy(landingPos, time);

        for (float currTime = 0; currTime < time; currTime += Time.deltaTime)
        {
            Debug.Log("Start is: " + startPosition + ", Middle is: " + middlePosition + " Final Point is: " + endPosition);
            float t = currTime / time;


            transform.position = CalculatePointOnCurve(t);
            yield return new WaitForFixedUpdate();
        }

        transform.position = endPosition;
    }
}
