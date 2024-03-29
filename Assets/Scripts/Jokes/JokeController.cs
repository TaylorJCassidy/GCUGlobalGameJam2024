using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JokeController : MonoBehaviour
{
    public int leftoverCount = 2;
    public JokePiece jokePrefab;
    public FullJoke fullJokePrefab;

    private List<Joke> jokes;
    private Joke chosenJoke;
    private List<JokePiece> correctJokePieces = new();
    private List<JokePiece> selectedJokePieces = new();

    private float punchlineTimer = -1f;
    private FullJoke fullJoke;

    private static JokeController jokeController;

    public static JokeController GetJokeController() {
        return jokeController;
    }

    // Start is called before the first frame update
    void Awake()
    {
        jokeController = this;
        jokes = getAllJokes();
    }

    void Update()
    {
        //change fulljoke text to punchline after timer has passed
        if (punchlineTimer > 0f) {
            punchlineTimer -= Time.deltaTime;
            if (punchlineTimer <= 0f) {
                punchlineTimer = -1f;
                fullJoke.setJokeText(chosenJoke.punchline);
            }
        }
    }

    public bool addSelectedJoke(JokePiece jokePiece)
    {
        if (selectedJokePieces.Count == 0 ) {
            selectedJokePieces.Add(jokePiece);
            return true;
        }

        JokePiece previousPiece = selectedJokePieces[selectedJokePieces.Count - 1];
        if (previousPiece.getEndConnector().Equals(jokePiece.getStartConnector())) {
            selectedJokePieces.Add(jokePiece);
            previousPiece.connect(jokePiece);
            return true;
        }
        return false;
    }

    public bool removeSelectedJoke(JokePiece jokePiece)
    {
        JokePiece lastPiece = selectedJokePieces[selectedJokePieces.Count - 1];
        if (jokePiece == lastPiece) {
            selectedJokePieces.Remove(lastPiece);
            if (selectedJokePieces.Count != 0) selectedJokePieces[selectedJokePieces.Count - 1].disconnect();
            return true;
        }
        return false;
    }

    List<Joke> getAllJokes()
    {
        return Resources.LoadAll<Joke>("Jokes").ToList();
    }

    public bool isValidSelections() 
    {
        return selectedJokePieces.SequenceEqual(correctJokePieces);
    }

    public FullJoke spawnJokeAndPunchline(float punchlineTimer) 
    {
        fullJoke = Instantiate(fullJokePrefab);
        fullJoke.setJokeText(chosenJoke.joke);
        fullJoke.name = "Full Joke";
        this.punchlineTimer = punchlineTimer;
        return fullJoke;
    }

    public List<JokePiece> spawnJokePieces()
    {
        chosenJoke = jokes[Random.Range(0, jokes.Count)];
        List<JokePiece> jokePieces = createCorrectJokePieces(chosenJoke);
        correctJokePieces.AddRange(jokePieces);
        jokePieces.AddRange(createOtherRandomJokePieces(jokes.Where(joke => joke != chosenJoke).ToList()));
        return jokePieces;
    }

    List<JokePiece> createCorrectJokePieces(Joke chosenJoke) {
        List<JokePiece> jokePieces = chosenJoke.jokeKeywords.Select(joke => {
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
        
        return jokePieces;
    }

    List<JokePiece> createOtherRandomJokePieces(List<Joke> jokes) {
        if (jokes.Count < leftoverCount) throw new System.Exception("Leftover count must be greater than the number of jokes + 1");

        List<JokePiece> jokePieces = new(leftoverCount);
        for (int i = 0; i < leftoverCount; i++) {
            Joke chosenJoke = jokes[Random.Range(0, jokes.Count)];
            jokes.Remove(chosenJoke);

            JokePiece jokePiece = Instantiate(jokePrefab);
            jokePiece.name = "Fake Joke Compontent";
            jokePiece.setJokeText(chosenJoke.jokeKeywords[Random.Range(0, chosenJoke.jokeKeywords.Length)]);
            jokePiece.setStartConnector(JokeConnector.GetRandomJokeConnector());
            jokePiece.setEndConnector(JokeConnector.GetRandomJokeConnector());

            jokePieces.Add(jokePiece);
        }
        return jokePieces;
    }


    public void ResetJokes() //
    {
        correctJokePieces.Clear();
        selectedJokePieces.Clear();
        //Destroy(fullJoke.gameObject);
    }

}
