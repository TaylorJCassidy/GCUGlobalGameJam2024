using UnityEngine;

public class ObjectTeleporter
{
    private readonly Vector3[] positions;
    private readonly MonoBehaviour gameObject;
    private int currentIdx = 0;

    public ObjectTeleporter(Vector3[] positions, MonoBehaviour gameObject)
    {
        this.positions = positions;
        this.gameObject = gameObject;
    }

    public void Teleport(bool forward) 
    {
        if (forward) {
            gameObject.transform.position = positions[++currentIdx % positions.Length];
        }
        else {
            if (currentIdx - 1 < 0) currentIdx = positions.Length;
            gameObject.transform.position = positions[--currentIdx];
        }
    }
}
