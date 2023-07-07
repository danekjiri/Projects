using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameloop : MonoBehaviour
{
    //timewindow between two rounds
    private float timeBetweenGames = 2.5f;

    //check who stays last and increment the score
    public void checkIfEnd()
    {   
        int playersCount = 0;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject winner = null;

        for (int i = 0; i < players.Length; i++) //find out number of active players
        {
            if (players[i].activeSelf == true)
            {
                playersCount++;
                winner = players[i];
            }
        }

        if (playersCount == 1) //if one - it is a winner
        {
            int winnerIndex = int.Parse(winner.name[6].ToString()) - 1;
            ScoreBoardController.instance.IncrementWinnerScore(winnerIndex);
            ScoreBoardController.instance.SaveLastScore();

            Invoke(nameof(newGame), timeBetweenGames); //new round in given time
        }
    }

    //reset current gamescene
    private void newGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
   
}
