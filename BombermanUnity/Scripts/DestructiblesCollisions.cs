using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class DestructiblesCollisions : MonoBehaviour
{
    //background data
    public LayerMask LayerDestructible;
    public Tilemap Destructible;

    //upgrades data
    public GameObject[] Upgrades;
    private List<GameObject> existingUpgrades;
    private float pickupDistance = 0.3f;
    private int generatingProbability; //probability 0-100

    //players data
    private GameObject[] players;

    //needed scripts
    private BombExplosion bombExplosionScript;
    private BombPlant bombPlantScript;
    private PlayerMovement playerMovementScript;

    //init script
    private void Start()
    {
        generatingProbability = getSetGeneratingProbability();
        players = GameObject.FindGameObjectsWithTag("Player");
        existingUpgrades = new List<GameObject>(); //keep track of all active powerups

        bombExplosionScript = GetComponent<BombExplosion>();
        bombPlantScript = GetComponent<BombPlant>();
        playerMovementScript = GetComponent<PlayerMovement>();
    }

    //get set probability from settings
    private int getSetGeneratingProbability()
    {
        int probability = 40;

        if (PlayerPrefs.HasKey("powerupProb"))
            probability = (PlayerPrefs.GetInt("powerupProb") + 1 ) * 10; //probability value stored as dropbox array indexes -> index 0 is meant to be probability 10%

        return probability;
    }
    
    //every frame update
    private void Update()
    {
        steppedOnUpgrade();
    }

    //checks if some player step on generated upgrade box
    private void steppedOnUpgrade()
    {
        GameObject toBeDeleted = null;
        foreach (GameObject upgrade in existingUpgrades)
        {
            Vector2 upgradePosition = upgrade.transform.position;
            foreach(GameObject player in players)
            {
                Vector2 playerPosition = player.transform.position;
                float distance = Vector2.Distance(upgradePosition, playerPosition);
            
                if (distance < pickupDistance)
                {
                    activateUpgrade(upgrade);
                    toBeDeleted = upgrade;
                }
            }
        }
        
        existingUpgrades.Remove(toBeDeleted);
        Destroy(toBeDeleted);
    }

    //execute given upgrade action
    private void activateUpgrade(GameObject upgrade)
    {
        string upgradeTag = upgrade.tag;
        switch (upgradeTag)
        {
            case "BiggerBlast":
                bombExplosionScript.IncrementExplosionLenght();
                break;
            case "ExtraBomb":
                bombPlantScript.IncrementBombCount();
                break;
            case "FasterMovement":
                playerMovementScript.IncrementPlayerMovement();
                break;
        }
    }

    //generate random upgrade with some probability
    private void generateUpgrade(Vector2 explosionPosition)
    {
        int randomNumber = Random.Range(0, 100);
        int randomItem = Random.Range(0, Upgrades.Length);

        if (randomNumber <= generatingProbability){
            GameObject generatedItem = Instantiate(Upgrades[randomItem], explosionPosition, Quaternion.identity);
            existingUpgrades.Add(generatedItem);
        }
    }

    //destructible box collision logic
    public void DestroyBox(Vector2 explosionPosition)
    {
        Vector3Int cell = Destructible.WorldToCell(explosionPosition); //get the cell coordinates by explosion position
        TileBase tile = Destructible.GetTile(cell); //get the box on given cell coordinates

        if (tile != null)
        {
            generateUpgrade(explosionPosition);
            Destructible.SetTile(cell, null); //if on explosion position is destructible box, then remove it
        }
    }

}
