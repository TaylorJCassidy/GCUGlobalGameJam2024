using UnityEngine;

[CreateAssetMenu(fileName = "NewJoke", menuName = "Jokes/New Joke", order = 1)]
public class Joke : ScriptableObject
{
    public string joke;
    public string punchline;
    public string[] jokeKeywords;
}
