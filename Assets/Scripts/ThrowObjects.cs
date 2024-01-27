using System.Collections;
using UnityEngine;

public class ThrowObjects : MonoBehaviour
{
    // Object Prefabs
    [SerializeField]
    GameObject[] possibleThrownObjects;

    // Offset from edges
    [SerializeField]
    [Range(0f, .5f)]
    float offset;

    private void Awake()
    {
        // Error Checking
        if (possibleThrownObjects.Length == 0)
        {
            throw new System.Exception(gameObject.name + ": Setup ObjectToThrow on ThrowObjects Script");
        }
    }
    private void Update()
    {
        // Temp
        if (Input.GetKeyDown(KeyCode.F))
            ThrowObject();
    }
    GameObject PickRandomObject() { return possibleThrownObjects[Random.Range(0, possibleThrownObjects.Length)]; }

    void ThrowObject()
    {
        // Get Camera Corner Positions
        Vector3 randomPos = GetRandomPositionWithinFrustum(Camera.main);

        // Create object behind camera
        GameObject newObj = Instantiate(PickRandomObject(), Camera.main.transform.position - Camera.main.transform.forward, Quaternion.identity, null);

        // Throw object in direction
        Vector3 dir = Vector3.Normalize(randomPos - newObj.transform.position);
        MovingObject objScript = newObj.AddComponent<MovingObject>();
        objScript.Activate(dir, 1.0f);
    }


    Vector3 GetRandomPositionWithinFrustum(Camera camera)
    {
        // Get the camera's viewport
        Rect viewportRect = camera.rect;
  
        // Calculate the bounds in screen space
        float minX = Screen.width * (viewportRect.x + offset);
        float maxX = Screen.width * (viewportRect.x + viewportRect.width - offset);
        float minY = Screen.height * (viewportRect.y + offset);
        float maxY = Screen.height * (viewportRect.y + viewportRect.height - offset);
        Debug.Log(minX + " " + maxX);
        // Generate a random position within the viewport bounds
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        float randomZ = 10.0f;

        // Convert screen coordinates to world coordinates
        Vector3 randomPosition = camera.ScreenToWorldPoint(new Vector3(randomX, randomY, randomZ));

        return randomPosition;
    }
}
