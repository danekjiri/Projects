using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlant : MonoBehaviour
{
    //bomb control
    public GameObject BombPrefab; //graphic representation
    public KeyCode BombDropKey;
    private List<GameObject> bombs = new List<GameObject>();

    //bomb data
    private int bombCount;
    private float bombLifetime = 2.5f;
    private int bombAvailable;

    //players data
    private GameObject[] players;
    private float bombTriggerDistance = 0.9f;

    //explosion script
    private BombExplosion explosionScript;

    //init before game
    private void Start()
    {
        bombCount = getSetBombCount();
        bombAvailable = bombCount;
        players = GameObject.FindGameObjectsWithTag("Player");
        explosionScript = GetComponent<BombExplosion>();
    }

    //findout and return set initial bomb count
    private int getSetBombCount()
    {
        int count = 1;

        if (PlayerPrefs.HasKey("bombCount"))
            count = PlayerPrefs.GetInt("bombCount") + 1; //storing dropbox indexes -> index 0 is meant to be 1 bomb

        return count;
    }

    //every frame update
    private void Update()
    {
        if ((Input.GetKeyDown(BombDropKey)) && (bombAvailable > 0)) //bomb drop conditions
            placeBomb();
        steppedPlayerOutOfBomb();
        explosionScript.WasPlayerInExplosion(players);
    }

    //check if player stepped out of bomb radius so it could collide
    private void steppedPlayerOutOfBomb()
    {
        Vector2 playerPosition = this.transform.position;

        foreach (GameObject bomb in bombs)
        {
            Vector2 bombPosition = bomb.transform.position;
            float distance = Vector2.Distance(playerPosition, bombPosition);
            
            if (distance > bombTriggerDistance) //if player far enought, the bomb starts behave as collapsible object
            {
                Collider2D bombCollider = bomb.GetComponent<Collider2D>();
                bombCollider.isTrigger = false;
            }
        }
    }

    //placing bomb on current player coordinates
    private void placeBomb()
    {
        Vector2 playerPosition = transform.position; //get position and round it
        playerPosition.x = Mathf.Round(playerPosition.x);
        playerPosition.y = Mathf.Round(playerPosition.y);

        GameObject placedBomb = Instantiate(BombPrefab, playerPosition, Quaternion.identity); //create bomb object
        bombs.Add(placedBomb); //keep track of placed bombs
        bombAvailable--;
        Destroy(placedBomb, bombLifetime+1); //if player dies faster than bomb explode, it will be destroyed
        
        StartCoroutine(explosionScript.ExplodeAfterTime(bombLifetime, placedBomb, bombs));
    }

    //upgrade the number of bombs
    public void IncrementBombCount()
    {
       bombCount++;
       bombAvailable++;
    }

    //restore exploded bomb
    public void IncrementBombAvailable()
    {
       bombAvailable++;
    }

}
