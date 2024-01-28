using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    //joke controller

    [SerializeField] private JokeController jokeController;
    [SerializeField] private ThrowObjects throwObjects;
    [SerializeField] private Tounge tongue;

    [Header("")]
    private int score = 0;
    private float audienceMeter = 50f;
    private float timeLeft = 60f;

    [Header("Sequence Lengths")]
    [SerializeField] private float timeForDodge = 60f;
    [SerializeField] private float timeForCatch = 60f;
    [SerializeField] private float timeForJoke = 60f;

    [Header("Joke Positions")]
    [SerializeField] private List<Transform> jokePositions = new();
    [SerializeField] private Transform finalJokePosition;

    enum GameState
    {
        Joke,
        Dodge,
        Catch,
        End
    }

    GameState gameState = GameState.Joke;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        tongue.SetActive(false);
    }

    void Update()
    {
      switch (gameState)
        {
            case GameState.Joke:
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0) // this occurs when the player fails to create a joke in time
                {
                    gameState = GameState.Dodge;
                    StartDodgeSequence();
                }

                if (true)
                {
                    //dont forget to stop timer above when this is going on
                    ShowFullJoke();
                }

                break;
            case GameState.Dodge:
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0)
                {
                    gameState = GameState.Joke;
                    StartJokeSequence();
                }
                break;
            case GameState.Catch:
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0)
                {
                    gameState = GameState.Joke;
                    StartJokeSequence();
                    //EndGame();
                }
                break;
            case GameState.End:
                break;
        }
    }

    void StartJokeSequence()
    {
        timeLeft = timeForJoke;
        List <JokePiece> jokes = jokeController.spawnJokes();
        for (int i = 0; i < jokes.Count; i++)
        {
            jokes[i].gameObject.transform.position = jokePositions[i].position;
        }

        while (gameState == GameState.Joke)
        {
            //bruteforce check using isValidSelections or something
        }

    }

    void StartDodgeSequence()
    {
        timeLeft = timeForDodge;
        //repeat throwing action

    }

    void StartCatchSequence()
    {
        timeLeft = timeForCatch;
        tongue.SetActive(true); // Allow the player to use the tongue
        throwObjects.ThrowObject(); // Change to good objects only

    }

    void ShowFullJoke()
    {
        FullJoke joke = jokeController.spawnFullJoke();
        joke.gameObject.transform.position = finalJokePosition.position;

        //laugh or something idk

        //Change meter somehow

        // delay the below items
        if (audienceMeter > 50f)
        {
            gameState = GameState.Catch;
        }
        else
        {
            gameState = GameState.Dodge;
        }
    }

    public void AddScore()
    {
        score += 1;
    }

    void EndGame()
    {

    }

}
