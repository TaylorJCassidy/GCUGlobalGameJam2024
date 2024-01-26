using UnityEngine;

public class JokeShapeFactory {
    public static JokeShape GetRandomShape()
    {
        return (JokeShape)System.Enum.GetValues(typeof(JokeShape)).GetValue(Random.Range(0,3));
    }
}