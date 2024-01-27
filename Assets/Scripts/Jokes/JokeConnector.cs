using UnityEngine;

public class JokeConnector
{
    public readonly int spriteIndex;

    public JokeConnector(int spriteIndex)
    {
        this.spriteIndex = spriteIndex;
    }

    public static JokeConnector GetRandomJokeConnector() {
        return new JokeConnector(Random.Range(0, 9));
    }
}