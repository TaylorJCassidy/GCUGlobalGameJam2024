using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("Controllers")]
    [SerializeField] private JokeController jokeController;
    [SerializeField] private ThrowObjects throwObjects;
    [SerializeField] private Tounge tongue;
    [SerializeField] private PlayerController playerMove;


    private int score = 0;
    private float audienceMeter = 50f;
    private float timeLeft = 60f;

    [Header("Sequence Lengths")]
    [SerializeField] private float timeForDodge = 60f;
    [SerializeField] private float timeForCatch = 60f;
    [SerializeField] private float timeForJoke = 60f;

    [SerializeField] private float punchLineTimer = 3f;
    [SerializeField] private float throwDelay = 1.5f;

    [Header("Joke Positions")]
    [SerializeField] private List<Transform> jokePositions = new();
    [SerializeField] private Transform finalJokePosition;

    [Header("UI")]
    [SerializeField] private Slider timerDisplay;
    [SerializeField] private Slider audienceDisplay;


    [SerializeField] private List<JokePiece> jokes = new(); // current joke pieces on screen
    private FullJoke fullJoke; // current full joke on screen

    private bool isJokeValid = false;

    enum GameState
    {
        Joke,
        TellingJoke,
        Dodge,
        Catch,
        End
    }

    [SerializeField] GameState gameState = GameState.Joke;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        playerMove.CanMove = false;
        tongue.SetActive(false);

        timerDisplay.maxValue = timeForJoke;
        audienceDisplay.value = audienceMeter;
    }

    void Start()
    {
        StartJokeSequence();
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.Joke:
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0) // this occurs when the player fails to create a joke in time
                {
                    gameState = GameState.TellingJoke;
                    ClearJokes();
                    StartCoroutine(DelayedAudienceReaction());
                }
                else
                {
                    CheckJokes();
                }
                break;
            case GameState.TellingJoke:
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

        // update timer
        timerDisplay.value = timeLeft;

    }

#region Joke Sequence
    void StartJokeSequence()
    {
        timeLeft = timeForJoke;
        playerMove.CanMove = false;
        tongue.SetActive(false);

        jokes = jokeController.spawnJokePieces();

        for (int i = 0; i < jokes.Count; i++)
        {
            // Randomize the order of the jokes
            int rand = Random.Range(0, jokes.Count);
            bool isOccupied = true;
            while (isOccupied)
            {
                if (jokePositions[rand].childCount > 0)
                {
                    rand = Random.Range(0, jokes.Count);
                    Debug.Log("Occupied" + rand);
                }
                else
                {
                    isOccupied = false;
                }
            }
            jokes[i].transform.position = jokePositions[rand].position;
            jokes[i].transform.SetParent(jokePositions[rand]);
        }
    }
    void CheckJokes()
    {
        if (jokeController.isValidSelections())
        {
            Debug.Log("Joke is valid:");
            gameState = GameState.TellingJoke;
            isJokeValid = true;
            ShowFullJoke();
        }
    }
    private void ClearJokes()
    {
        for (int i = 0; i < jokes.Count; i++)
        {
            Destroy(jokes[i].gameObject);
        }
        jokes.Clear();
        jokeController.ResetJokes();
    }

    void ShowFullJoke()
    {
        ClearJokes();

        fullJoke = jokeController.spawnJokeAndPunchline(punchLineTimer);
        fullJoke.gameObject.transform.position = finalJokePosition.position;
        StartCoroutine(DelayedAudienceReaction());
    }
    private IEnumerator DelayedAudienceReaction()
    {
        yield return new WaitForSeconds(punchLineTimer);
        //audienceMeter += 10f;
        if (isJokeValid)
        {
            // clap
            audienceMeter += 10f;
        }
        else
        {
            // boo
            audienceMeter -= 40f;
        }
        audienceDisplay.value = audienceMeter;
        isJokeValid = false;
        yield return new WaitForSeconds(1f);
        if (fullJoke != null) Destroy(fullJoke.gameObject);
        if (audienceMeter > 50f)
        {
            // clap
            gameState = GameState.Catch;
            StartCatchSequence();
        }
        else
        {
            // boo
            gameState = GameState.Dodge;
            StartDodgeSequence();
        }
        yield break;
    }
#endregion

    void StartDodgeSequence()
    {
        timeLeft = timeForDodge;
        // Enable movement
        playerMove.CanMove = true;
        //repeat throwing action
        StartCoroutine(DelayedThrow());
    }

    void StartCatchSequence()
    {
        timeLeft = timeForCatch;
        tongue.SetActive(true); // Allow the player to use the tongue
        throwObjects.ThrowObject(); // Change to good objects only
        StartCoroutine(DelayedThrow());
    }

    private IEnumerator DelayedThrow()
    {
        while (gameState == GameState.Catch || gameState == GameState.Dodge)
        {
            Debug.Log("Waiting To throw");
            yield return new WaitForSeconds(throwDelay);
            Debug.Log("Throwing");
            throwObjects.ThrowObject();
        }

        Debug.Log("Throwing stopped");
        tongue.SetActive(false);
        playerMove.CanMove = false;
        yield break;   
    }

    public void AddScore()
    {
        score += 1;
    }

    void EndGame()
    {

    }

}
