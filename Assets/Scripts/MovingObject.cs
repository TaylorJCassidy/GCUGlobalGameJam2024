using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class MovingObject : MonoBehaviour
{
    // Setup Values
    Vector3 startPosition;
    Vector3 middlePosition;
    Vector3 endPosition = Vector3.negativeInfinity;
    float speed;
    bool active = false;
    bool falling = false;
    Vector3 finalDirection;
    [SerializeField]
    VisualEffect vfx;
    
    // In case we want to end object movement
    public void Deactivate()
    {
        active = false;
    }

    // Activate Object Movement
    public void Activate(Vector3 position, float speed, float curveHeight)
    {
        active = true;
        startPosition = transform.position;
        this.endPosition = position;
        this.speed = speed;

        middlePosition = endPosition - startPosition + (Vector3.up * curveHeight);

        StartCoroutine(MoveTo());
    }
    private void Update()
    {
        //DrawBezierCurve(.0001f);
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

        if (falling)
            transform.position += finalDirection * speed;
    }


    private void OnTriggerEnter(Collider other)
    {     
        switch (other.tag)
        {
            case "Player":
                // Damage Player
                if (this.tag == "DamagingThrowable")
                {
                    other.GetComponent<PlayerHealth>().TakeDamage(1);
                    Destroy(gameObject);
                }
                break;
            case "Throwable":
                break;
            case "DamagingThrowable":

                break;
            case "Tongue":
                break;
            default:
                CleanUp();
                Destroy(gameObject);
                break;
        }
        Debug.Log(gameObject.name + " Hit: " + other.name);
    }
    GameObject decal;
    void CleanUp()
    {
        Destroy(decal);
        Destroy(Instantiate(vfx, transform.position, Quaternion.identity, null), 2.0f);
    }
    IEnumerator MoveTo()
    {
        decal = SpawnDecal(endPosition);

        float time = 1 / speed;
        for (float currTime = 0; currTime < time; currTime += Time.deltaTime)
        {
            if (!active) yield break;

            //Debug.Log("Start is: " + startPosition + ", Middle is: " + middlePosition + " Final Point is: " + endPosition);
            float t = currTime / time;

            transform.position = CalculatePointOnCurve(t);      
            yield return new WaitForFixedUpdate();

        }

        transform.position = endPosition;

        falling = true;
        finalDirection = Vector3.Normalize(endPosition - CalculatePointOnCurve(0.9f));
    }

    [SerializeField]
    GameObject projectileDecalPrefab;
    [SerializeField]
    LayerMask layerMask;
    GameObject SpawnDecal(Vector3 pos)
    {
        // Shoot ray from camera to position (Ignore player)
        RaycastHit hit;
        Vector3 origin = Camera.main.transform.position;
        Vector3 dir = Vector3.Normalize(pos - origin);
        if (Physics.Raycast(origin, dir, out hit, 10000.0f, layerMask))
        {
            Debug.Log("Hit " + hit.transform.gameObject.name);
            // Place object perpindicular to normal
            // Get rotation from normal
            return Instantiate(projectileDecalPrefab, hit.point, Quaternion.identity, null);
        }
        return null;
    }
}
