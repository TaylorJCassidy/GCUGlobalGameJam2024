using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JokeController : MonoBehaviour
{
    public int leftoverCount = 2;
    public JokePiece jokePrefab;

    // Start is called before the first frame update
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
            jokePiece.setJokeText(joke);
            return jokePiece;
        }).ToList();

        jokePieces[0].setStartConnector(JokeConnector.GetRandomJokeConnector());
        jokePieces[jokePieces.Count - 1].setEndConnector(JokeConnector.GetRandomJokeConnector());

        for (int i = 0; i < jokePieces.Count - 1; i++) {
            JokeConnector sharedConnector = JokeConnector.GetRandomJokeConnector();

            jokePieces[i].setEndConnector(sharedConnector);
            jokePieces[i + 1].setStartConnector(sharedConnector);
        }
    }

    void createOtherRandomJokePieces(List<Joke> jokes) {
        if (jokes.Count < leftoverCount) throw new System.Exception("Leftover count must be greater than the number of jokes + 1");
        int i = 0;
        do {
            Joke chosenJoke = jokes[Random.Range(0, jokes.Count)];
            Debug.Log(chosenJoke);
            jokes.Remove(chosenJoke);

            JokePiece jokePiece = Instantiate(jokePrefab);
            jokePiece.name = "Fake Joke Compontent";
            jokePiece.setJokeText(chosenJoke.jokeContent[Random.Range(0, chosenJoke.jokeContent.Length)]);
            jokePiece.setStartConnector(JokeConnector.GetRandomJokeConnector());
            jokePiece.setEndConnector(JokeConnector.GetRandomJokeConnector());
        }
        while (++i < leftoverCount);
    }


}
