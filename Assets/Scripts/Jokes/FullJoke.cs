using TMPro;
using UnityEngine;

public class FullJoke : MonoBehaviour
{
    public void setJokeText(string jokeText)
    {
        GetComponentInChildren<TextMeshPro>().text = jokeText;
    }
}