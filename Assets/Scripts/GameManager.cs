using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;


    private List<JokePiece> jokes = new(); // current joke pieces on screen
    private FullJoke fullJoke; // current full joke on screen

    private bool isJokeValid = false;

    [SerializeField] private Animator[] audienceAnimator;

    public enum GameState
    {
        Joke,
        TellingJoke,
        Dodge,
        Catch,
        End
    }

    public GameState gameState = GameState.Joke;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        playerMove.CanMove = false;
        tongue.CanUse = false;
        playerMove.Reset();

        timerDisplay.maxValue = timeForJoke;
        audienceDisplay.value = audienceMeter;

        audioSource = GetComponent<AudioSource>();

        audienceAnimator = GameObject.Find("Audience").GetComponentsInChildren<Animator>();
    }

    void Start()
    {
        StartJokeSequence();
    }

    [SerializeField]
    GameObject dodgeUI;
    [SerializeField]
    GameObject catchUI;
    [SerializeField]
    GameObject heart1;
    [SerializeField]
    GameObject heart2;
    [SerializeField]
    GameObject heart3;
    [SerializeField]
    PlayerHealth health;
    void Update()
    {
        dodgeUI.SetActive(false);
        catchUI.SetActive(false);
        heart1.SetActive(false); heart2.SetActive(false); heart3.SetActive(false);
       
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
                if (health.GetHealth() > 2)
                {
                    heart1.SetActive(true); heart2.SetActive(true); heart3.SetActive(true);
                }
                else if (health.GetHealth() > 1)
                {
                    heart1.SetActive(true); heart2.SetActive(true);
                }
                else if (health.GetHealth() > 0)
                {
                    heart1.SetActive(true);
                }
                dodgeUI.SetActive(true);
                catchUI.SetActive(false);
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0)
                {
                    gameState = GameState.Joke;
                    StartJokeSequence();
                }
                break;
            case GameState.Catch:
                dodgeUI.SetActive(false);
                catchUI.SetActive(true);
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0)
                {
                    gameState = GameState.Joke;
                    StartJokeSequence();
                    //EndGame();
                }
                break;
            case GameState.End:
                EndGame();
                break;
        }

        // update timer
        timerDisplay.value = timeLeft;

    }

#region Joke Sequence
    void StartJokeSequence()
    {
        audioSource.PlayOneShot(audioClips[3]); //paper shuffle
        CameraController.cameraController.teleport(1);
        foreach (Animator anim in audienceAnimator)
        {
            anim.SetBool("Clap", false);
        }
        timeLeft = timeForJoke;
        playerMove.CanMove = false;
        tongue.CanUse = false;
        playerMove.Reset();

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
        fullJoke.gameObject.transform.SetParent(finalJokePosition);
        fullJoke.gameObject.transform.position = finalJokePosition.position;
        StartCoroutine(DelayedAudienceReaction());
    }
    private IEnumerator DelayedAudienceReaction()
    {
        CameraController.cameraController.teleport(2);
        audioSource.PlayOneShot(audioClips[4]); //tell joke
        yield return new WaitForSeconds(7.0f);
        if (isJokeValid)
        {
            // clap
            yield return new WaitForSeconds(punchLineTimer);
            audioSource.PlayOneShot(audioClips[2]); //drum
            yield return new WaitForSeconds(0.5f);
            foreach (Animator anim in audienceAnimator)
            {
                anim.SetBool("Clap", true);
            }
            audioSource.PlayOneShot(audioClips[1]); //clap
            audienceMeter += 10f;
            AddScore(40);
        }
        else
        {
            // boo
            audioSource.PlayOneShot(audioClips[2]); //drum
            yield return new WaitForSeconds(0.5f);
            foreach (Animator anim in audienceAnimator)
            {
                anim.SetBool("Clap", true);
            }
            audioSource.PlayOneShot(audioClips[0]); //boo
            yield return new WaitForSeconds(1f);
            audienceMeter -= 40f;
        }
        audienceDisplay.value = audienceMeter;
        yield return new WaitForSeconds(2f);
        if (fullJoke != null) Destroy(fullJoke.gameObject);
        if (isJokeValid)
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
        isJokeValid = false;
        yield break;
    }
#endregion

    void StartDodgeSequence()
    {
        CameraController.cameraController.teleport(0);
        timeLeft = timeForDodge;
        // Enable movement
        playerMove.CanMove = true;
        //repeat throwing action
        StartCoroutine(DelayedThrow());
    }

    void StartCatchSequence()
    {
        CameraController.cameraController.teleport(0);
        timeLeft = timeForCatch;
        tongue.CanUse = true;
        throwObjects.ThrowGoodObject(); // Change to good objects only
        StartCoroutine(DelayedThrow());
    }

    private IEnumerator DelayedThrow()
    {
        while (gameState == GameState.Catch || gameState == GameState.Dodge)
        {
            Debug.Log("Waiting To throw");
            yield return new WaitForSeconds(throwDelay);
            Debug.Log("Throwing");
            if (gameState == GameState.Catch)
                throwObjects.ThrowGoodObject();
            else if (gameState == GameState.Dodge) 
                throwObjects.ThrowBadObject();
        }

        Debug.Log("Throwing stopped");
        tongue.CanUse = false;
        playerMove.CanMove = false;
        yield break;   
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public void EndGame()
    {
        // Load Main Menu
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
