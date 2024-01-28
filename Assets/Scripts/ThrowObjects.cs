using System.Collections;
using UnityEngine;

public class ThrowObjects : MonoBehaviour
{
    // Object Prefabs
    [SerializeField]
    GameObject[] possibleBadThrownObjects;
    [SerializeField]
    GameObject[] possibleGoodThrownObjects;

    [SerializeField]
    [Range(0.1f, 20.0f)]
    float throwSpeed;
    
    [SerializeField]
    [Range(2.0f, 10.0f)]
    float rotationModifier;

    [SerializeField]
    [Range(1.0f, 30.0f)]
    float curveHeight;

    // Can Spawn anywhere in Collider
    [SerializeField]
    Collider possiblePositions;

    // Save Bounds so we can Disable Collider
    Bounds bounds;

    private void Awake()
    {
        // Error Checking
        if (possibleBadThrownObjects.Length == 0 || possibleGoodThrownObjects.Length == 0)
            throw new System.Exception(gameObject.name + ": Setup ObjectToThrow on ThrowObjects Script");
        if (possiblePositions == null)
            throw new System.Exception(gameObject.name + ": Setup PossiblePositions on ThrowObjects Script");

        bounds = possiblePositions.bounds;
        possiblePositions.enabled = false;
    }
    private void Update()
    {
        // Temp
        if (Input.GetKeyDown(KeyCode.F))
            ThrowGoodObject();
    }
    GameObject PickRandomObject(GameObject[] gameObjects) { return gameObjects[Random.Range(0, gameObjects.Length)]; }
    public void ThrowBadObject()
    {
        ThrowObject(true);
    }
    public void ThrowGoodObject()
    {
        ThrowObject(false);
    }

    private void ThrowObject(bool badObject)
    {
        // Get Camera Corner Positions
        Vector3 randomPos = GetRandomPointInBounds(bounds);
        GameObject obj;
        string tag;
        if (badObject)
        {
            obj = PickRandomObject(possibleBadThrownObjects);
            tag = "DamagingThrowable";
        }
        else
        {
            obj = PickRandomObject(possibleGoodThrownObjects);
            tag = "Throwable";
        }

        // Create object behind camera
        GameObject newObj = Instantiate(obj, Camera.main.transform.position - Camera.main.transform.forward, Quaternion.identity, null);
        newObj.tag = tag;

        MovingObject objScript = newObj.GetComponent<MovingObject>();
        objScript.Activate(randomPos, throwSpeed, curveHeight);

        // Random Rotation Speed
        float speedModifier = Random.Range(40.0f, 80.0f);
        if (Random.Range(0, 10) < 5) speedModifier *= -1;

        newObj.GetComponent<Rigidbody>().AddTorque(speedModifier * new Vector3(rotationModifier, rotationModifier, rotationModifier) * (throwSpeed));
    }



    public static Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    /* Not in Use but keeping incase need */
/*    Vector3 GetRandomPositionWithinFrustum(Camera camera)
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
    }*/
}
