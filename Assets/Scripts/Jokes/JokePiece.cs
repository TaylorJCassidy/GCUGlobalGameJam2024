using System.Collections.Generic;
using UnityEngine;

public class JokePiece : MonoBehaviour
{
    private JokeConnector startConnector;
    private JokeConnector endConnector;
    private string jokeText;

    private Sprite[] sprites;

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

    public void setJokeText(string jokeText) 
    {
        this.jokeText = jokeText;
        transform.GetChild(2).gameObject.GetComponent<TextMesh>().text = jokeText;
    }
}