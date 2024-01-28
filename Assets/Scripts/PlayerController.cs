using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Min(1f)]
    public float speed;
    public Vector3[] positions;
    private int currentPosition = 1; //middle

    private bool canMove = true;

    public bool CanMove { get => canMove; set => canMove = value; }

    // Update is called once per frame
    void Update()
    {
        processUserInput();
    }

    void processUserInput() {
        if (canMove) {
            if (Input.GetKeyDown(KeyCode.A)) {
                if (currentPosition - 1 >= 0) --currentPosition;
            }
            else if (Input.GetKeyDown(KeyCode.D)) {
                if (currentPosition + 1 < positions.Length) ++currentPosition;
            }
        }
        move(positions[currentPosition]);
    }

    void move(Vector3 position) {
        Vector3 lerpPos = Vector3.Lerp(transform.position, position, Time.deltaTime * speed);
        transform.position = lerpPos;
    }

    public void Reset()
    {
        currentPosition = 1;
        move(positions[currentPosition]);
    }
    
}
