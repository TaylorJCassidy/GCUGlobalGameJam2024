using UnityEngine;

public class JokeColorFactory
{
    public enum Colors {
        Red,
        Green,
        Blue
    }
    public static Color GetColor(Colors color) {
        return color switch
        {
            Colors.Red => Color.red,
            Colors.Green => Color.green,
            Colors.Blue => Color.blue,
            _ => throw new System.Exception("Not a valid colour!")
        };
    }

    public static Color GetRandomColor() {
        return GetColor((Colors)Random.Range(0,3));
    }
}