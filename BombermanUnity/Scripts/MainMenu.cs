using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //clear all stored data to start with 0 scores
    private void Awake()
    {
       for (int i = 0; i < 4; i++)
       {
        PlayerPrefs.DeleteKey(i.ToString()); //stored score for player one in playerprefs has key 1
       }
       PlayerPrefs.DeleteKey("LastGamemode");
    }

    //switch to gamemode selection
    public void MapsMenu()
    {
        SceneManager.LoadScene("MapsMenu");
    }

    //quit the game button
    public void QuitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    //return to Main menu button
    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //switch to settings button
    public void Settings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    //switch to map1
    public void Map1()
    {
        SceneManager.LoadScene("GameMap1");
    }

    //switch to map2
    public void Map2()
    {
        SceneManager.LoadScene("GameMap2");
    }

    //switch to map3
    public void Map3()
    {
        SceneManager.LoadScene("GameMap3");
    }

}
