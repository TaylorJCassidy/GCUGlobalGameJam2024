using UnityEngine;

public class ObjectTeleporter
{
    private readonly Vector3[] positions;
    private readonly Vector3[] rotations;
    private readonly MonoBehaviour gameObject;
    private int currentIdx = 0;

    public ObjectTeleporter(Vector3[] positions, Vector3[] rotations, MonoBehaviour gameObject)
    {
        this.positions = positions;
        this.rotations = rotations;
        this.gameObject = gameObject;
        if (positions.Length != rotations.Length) throw new System.Exception("The number of positions and rotations must match");
    }

    public void Teleport(int idx) {
        if (idx < 0 || idx > positions.Length) throw new System.Exception("The index provided is greater than the number of positions");
        gameObject.transform.position = positions[idx];
        gameObject.transform.rotation = new Quaternion(){
            eulerAngles = rotations[idx]
        };
    }

    public void Teleport(bool forward) 
    {
        if (forward) {
            int idx = ++currentIdx % positions.Length;
            gameObject.transform.position = positions[idx];
            gameObject.transform.rotation = new Quaternion(){
                eulerAngles = rotations[idx]
            };
        }
        else {
            if (currentIdx - 1 < 0) currentIdx = positions.Length;
            gameObject.transform.position = positions[--currentIdx];
            gameObject.transform.rotation = new Quaternion(){
                eulerAngles = rotations[currentIdx]
            };
        }
    }
}
