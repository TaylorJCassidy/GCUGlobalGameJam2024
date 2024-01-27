using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JokePiece : MonoBehaviour
{
    private JokeConnector startConnector;
    private JokeConnector endConnector;
    private string jokeText;

    private Sprite[] sprites;
    private bool selected = false;

    public void Awake() 
    {
        sprites = Resources.LoadAll<Sprite>("Jokes/Connectors");
    }

    public void setStartConnector(JokeConnector jokeConnector) 
    {
        startConnector = jokeConnector;
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sprites[jokeConnector.spriteIndex];
    }

    public void setEndConnector(JokeConnector jokeConnector)
    {
        endConnector = jokeConnector;
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = sprites[jokeConnector.spriteIndex];
    }

    public JokeConnector getStartConnector() {
        return startConnector;
    }

    public JokeConnector getEndConnector() {
        return endConnector;
    }

    public void setJokeText(string jokeText) 
    {
        this.jokeText = jokeText;
        transform.GetChild(2).gameObject.GetComponent<TextMeshPro>().text = jokeText;
    }

    public void connect(JokePiece jokePiece) {
        GameObject endConnector = transform.GetChild(1).gameObject;
        Vector3[] positions = {
            endConnector.transform.position + new Vector3(0f, 0f, 1f), 
            jokePiece.transform.GetChild(0).transform.position + new Vector3(0f, 0f, 1f)
        };
        endConnector.GetComponent<LineRenderer>().SetPositions(positions);
    }

    public void disconnect() {
        transform.GetChild(1).gameObject.GetComponent<LineRenderer>().SetPositions(new Vector3[]{Vector3.zero, Vector3.zero});
    }

    void OnMouseDown()
    {
        JokeController jokeController = JokeController.GetJokeController();
        if (selected) {
            if (jokeController.removeSelectedJoke(this)) {
                selected = false;
                transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        else {
            if (jokeController.addSelectedJoke(this)) {
                selected = true;
                transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
        
    }
}