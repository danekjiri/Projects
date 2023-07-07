using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBoardController : MonoBehaviour
{
    //players score tables text
    public TMP_Text ObjectP1;    
    public TMP_Text ObjectP2;
    public TMP_Text ObjectP3;
    public TMP_Text ObjectP4;
    private TMP_Text[] dashboards;

    public GameObject[] Players;

    //to be accessed from another script
    public static ScoreBoardController instance;

    //help variables
    private bool wasIncremented = false; //to prevent multiple increment
    private int[] scores = new int[4];
    private float timeBetweenGames = 2f;

    //to be accessible from different location
    private void Awake()
    {
        instance = this;
    }

    //check if not collison - 2 last players die at once
    void Update(){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length < 1)
            Invoke(nameof(newGame),timeBetweenGames);

    }

    //resets scene
    private void newGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //init tables - write given score or X for non-players
    private void Start()
    {
        dashboards = new TMP_Text[4] {ObjectP1, ObjectP2, ObjectP3, ObjectP4};
        initScores(); //load stored previous score from PlayerPrefs

        for (int i = 0; i < dashboards.Length; i++)
        {
            if (Players[i].activeSelf)
                dashboards[i].text = scores[i].ToString(); //write score to text box
            else
                dashboards[i].text = "X";
        }
    }

    //load data
    private void initScores()
    {
        for(int i = 0; i < dashboards.Length; i++)
        {
            this.scores[i] = PlayerPrefs.GetInt(i.ToString()); //get stored data index 0 - Player1, index 1 - Player2 ...
        }
    }

    //increment score just once a round
    public void IncrementWinnerScore(int winnerIndex)
    {
        if (!wasIncremented)
        {
            scores[winnerIndex]++;
            this.wasIncremented = true;
        }
    }

    //in the end of turn save current score
    public void SaveLastScore()
    {
        for(int i = 0; i < dashboards.Length; i++)
        {
            PlayerPrefs.SetInt(i.ToString(), scores[i]);
        }
    }

}
