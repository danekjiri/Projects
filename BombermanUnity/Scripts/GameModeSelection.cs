using UnityEngine;
using UnityEngine.SceneManagement;

public class PregameScene : MonoBehaviour
{
    //scoreboard object reference
    public GameObject Scoreboard;
    public GameObject ReturnKeyText;

    //stored data in PlayPrefs keys
    private const string storedLastGamemode = "LastGamemode";

    //check if the game is running and if so, pass the gamemode selection
    private void Awake()
    {
        if (isGameRunning())
        {
            int gamemode = PlayerPrefs.GetInt(storedLastGamemode); //load previous game round mode from PlayerPrefs

            if (gamemode == 2)
                Multiplayer2();
            else if (gamemode == 3)
                Multiplayer3();
            else if (gamemode == 4)
                Multiplayer4();
        }
    }

    //check if game running by score - if at least one game played then SUM(score) > 0
    private bool isGameRunning()
    {
        int sum = 0;
        int MAXPLAYERS = 4;

        for (int i = 0; i < MAXPLAYERS; i++)
            sum += PlayerPrefs.GetInt(i.ToString()); //stored scores

        if (sum > 0)
            return true;
        return false;
    }

    //activate scoreboard texts
    private void showScoreboards()
    {
        Scoreboard.SetActive(true);
    }

    //show exit key window for 3 seconds
    private void showQuitKeyText()
    {
        int windowShowTime = 3;

        ReturnKeyText.SetActive(true); 
        Destroy(ReturnKeyText, windowShowTime);
    }

    //deactivate players according to gamemode
    private void hideXPlayers(int playersToHide)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); //player position is P1---P3
                                                                            //                   |     |
                                                                            //                   P4---P2
        int playersCount = players.Length;
        for(int i = 1; i < playersToHide+1; i++) //little messy indexing to get P1,P2,P3...
        {
            int playerIndex = playersCount-i-1;
            players[playerIndex].SetActive(false);
        }
    }

    //deactivate gamemode selection
    private void hideGamemodesScene()
    {
        GameObject gamemode = GameObject.FindGameObjectWithTag("Gamemode");
        gamemode.SetActive(false);
    }


    public void Multiplayer2()
    {   
        int selectedGamemode = 2;
        PlayerPrefs.SetInt(storedLastGamemode, selectedGamemode); //store chosen gamemode 
        hideXPlayers(2); //hide MAX-selected gamemode players
        hideGamemodesScene(); //hide number of players screen
        showScoreboards();  //activate scoreboards
        showQuitKeyText();  //show return key text to mainmenu
    }

    public void Multiplayer3()
    {
        int selectedGamemode = 3;
        PlayerPrefs.SetInt(storedLastGamemode, selectedGamemode);
        hideXPlayers(1);
        hideGamemodesScene();
        showScoreboards();
        showQuitKeyText();
    }

    public void Multiplayer4()
    {
        int selectedGamemode = 4;
        PlayerPrefs.SetInt(storedLastGamemode, selectedGamemode);
        hideGamemodesScene();
        showScoreboards();
        showQuitKeyText();
    }
    
    //return to Main menu scene
    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

