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

    [SerializeField]
    [Range(0.1f, 20.0f)]
    float throwSpeed;
    
    [SerializeField]
    [Range(2.0f, 10.0f)]
    float rotationModifier;

    [SerializeField]
    [Range(1.0f, 30.0f)]
    float curveHeight;

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

    // Throw Multiple Objects
    void ThrowObject(int count)
    {
        for (int i = 0; i < count; i++)
        {
            ThrowObject();
        }
    }
    void ThrowObject()
    {
        // Get Camera Corner Positions
        Vector3 randomPos = GetRandomPositionWithinFrustum(Camera.main);

        // Create object behind camera
        GameObject newObj = Instantiate(PickRandomObject(), Camera.main.transform.position - Camera.main.transform.forward, Quaternion.identity, null);

        // Throw object in direction
        Vector3 dir = randomPos;
        MovingObject objScript = newObj.AddComponent<MovingObject>();
        objScript.Activate(dir, throwSpeed, curveHeight);

        // Random Rotation Speed
        float speedModifier = Random.Range(40.0f, 80.0f);
        if (Random.Range(0, 10) < 5) speedModifier *= -1;

        newObj.GetComponent<Rigidbody>().AddTorque(speedModifier * new Vector3(rotationModifier, rotationModifier, rotationModifier) * (throwSpeed));
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
        
        // Generate a random position within the viewport bounds
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        float randomZ = 30.0f;

        // Convert screen coordinates to world coordinates
        Vector3 randomPosition = camera.ScreenToWorldPoint(new Vector3(randomX, randomY, randomZ));

        return randomPosition;
    }
}
