using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class JokeController : MonoBehaviour
{
    public int leftoverCount = 2;
    public JokePiece jokePrefab;

    // Start is called before the first frame update
    //TODO fix duplicate fakes
    void Start()
    {
        List<Joke> jokes = getAllJokes();
        Joke chosenJoke = jokes[Random.Range(0, jokes.Count)];
        createCorrectJokePieces(chosenJoke);
        createOtherRandomJokePieces(jokes.Where(joke => joke != chosenJoke).ToList());
    }

    List<Joke> getAllJokes()
    {
        return Resources.LoadAll<Joke>("Jokes").ToList();
    }

    void createCorrectJokePieces(Joke chosenJoke) {
        List<JokePiece> jokePieces = chosenJoke.jokeContent.Select(joke => {
            JokePiece jokePiece = Instantiate(jokePrefab);
            jokePiece.name = "Real Joke Compontent";
            jokePiece.jokeSection = joke;
            return jokePiece;
        }).ToList();

        jokePieces[0].startColour = JokeColorFactory.GetRandomColor();
        jokePieces[0].startShape = JokeShapeFactory.GetRandomShape();

        jokePieces[jokePieces.Count - 1].endColour = JokeColorFactory.GetRandomColor();
        jokePieces[jokePieces.Count - 1].endShape = JokeShapeFactory.GetRandomShape();

        for (int i = 0; i < jokePieces.Count - 1; i++) {
            Color sharedColour = JokeColorFactory.GetRandomColor();
            JokeShape sharedShape = JokeShapeFactory.GetRandomShape();

            jokePieces[i].endColour = sharedColour;
            jokePieces[i].endShape = sharedShape;

            jokePieces[i + 1].startColour = sharedColour;
            jokePieces[i + 1].startShape = sharedShape;
        }
    }

    void createOtherRandomJokePieces(List<Joke> jokes) {
        for (int i = 0; i < leftoverCount; i++) {
            Joke chosenJoke = jokes[Random.Range(0, jokes.Count)];
            JokePiece jokePiece = Instantiate(jokePrefab);
            jokePiece.name = "Fake Joke Compontent";
            jokePiece.jokeSection = chosenJoke.jokeContent[Random.Range(0, chosenJoke.jokeContent.Length)];
            jokePiece.startColour = JokeColorFactory.GetRandomColor();
            jokePiece.endColour = JokeColorFactory.GetRandomColor();
            jokePiece.startShape = JokeShapeFactory.GetRandomShape();
            jokePiece.endShape = JokeShapeFactory.GetRandomShape();
        }
    }


}
