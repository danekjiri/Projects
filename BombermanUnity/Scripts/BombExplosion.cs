using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    //explosion data
    public GameObject ExplosionPrefab; //graphic representation
    public LayerMask LayerObsidian;
    public LayerMask LayerDestructible;

    //explosion data
    private int explosionLenght;
    private float explosionLifeTime = 1.5f;
    private float deathExplosionDistance = 0.5f;

    //needed scripts
    private DestructiblesCollisions destructiblesCollisionScript;
    private BombPlant bombPlantScript;
    private Gameloop gameloopScript;

    //init script
    private void Start()
    {
        explosionLenght = getSetExplosionLenght();
        bombPlantScript = GetComponent<BombPlant>();
        destructiblesCollisionScript = GetComponent<DestructiblesCollisions>();
        gameloopScript = GetComponent<Gameloop>();
    }

    //get initial explosion lenght from settings
    private int getSetExplosionLenght()
    {
        int lenght = 1;
        if (PlayerPrefs.HasKey("explosionLenght"))
            lenght = PlayerPrefs.GetInt("explosionLenght") + 1; //lenght value stored as dropbox array indexes -> index 0 is meant to be lenght 1

        return lenght;
    }

    //return true if collide with destructible/undestructible layer
    //and do destructible collision logic
    private bool gridCollision(Vector2 explosionPosition, Vector2 explosionDirection)
    {
        if (Physics2D.OverlapBox(explosionPosition, Vector2.one / 2f, 0f, LayerDestructible))
        {
            explodeBomb(0, explosionPosition, explosionDirection); //cover the desctructible box with last explosion
            destructiblesCollisionScript.DestroyBox(explosionPosition); //destructible collision logic
            return true;
        }
        else if (Physics2D.OverlapBox(explosionPosition, Vector2.one / 2f, 0f, LayerObsidian))
            return true; //collision with a wall - stop
        return false;
    }

    //generate explosion on given coordinates and keep heading by its vector if not collision
    private void explodeBomb(int explosionLenght, Vector2 explosionPosition, Vector2 explosionDirection)
    {   
        GameObject explosion = Instantiate(ExplosionPrefab, explosionPosition, Quaternion.identity); //create new explosion on given coordinates
        Destroy(explosion, explosionLifeTime); //destroy the explosion by given time

        if (explosionLenght < 1) //recurence condition
            return;
        
        explosionPosition += explosionDirection;
        bool isGridCollision = gridCollision(explosionPosition, explosionDirection); //find out if collision with box
        if (isGridCollision == false)
            explodeBomb(explosionLenght-1, explosionPosition, explosionDirection); //if not collision and explosion > 1 then keep explode in given direction
    }

    //explosion subroutine - explote valid directions/track bombs
    public IEnumerator ExplodeAfterTime(float bombLifetime, GameObject bomb, List<GameObject> bombs)
    {
        yield return new WaitForSeconds(bombLifetime); //keep trying untill bombLifetime

        Vector2 bombExplodePosition = bomb.transform.position; //store current bomb position and round it
        bombExplodePosition.x = Mathf.Round(bombExplodePosition.x);
        bombExplodePosition.y = Mathf.Round(bombExplodePosition.y);

        //try explosion in every possible direction
        explodeBomb(explosionLenght, bombExplodePosition, Vector2.left);
        explodeBomb(explosionLenght, bombExplodePosition, Vector2.right);
        explodeBomb(explosionLenght, bombExplodePosition, Vector2.up);
        explodeBomb(explosionLenght, bombExplodePosition, Vector2.down);

        bombs.Remove(bomb);
        Destroy(bomb);
        bombPlantScript.IncrementBombAvailable(); //another bomb can be placed
    }

    //check if player stepp in explosion
    public void WasPlayerInExplosion(GameObject[] players)
    {
        GameObject[] explosions = GameObject.FindGameObjectsWithTag("Explosion");
        foreach (GameObject player in players)
        {
            Vector2 playerPosition = player.transform.position;
            foreach(GameObject explosion in explosions)
            {
                Vector2 explosionPosition = explosion.transform.position;
                float distance = Vector2.Distance(playerPosition, explosionPosition);
                
                if (distance < deathExplosionDistance)
                {
                    player.SetActive(false);
                }
            }
        }
        gameloopScript.checkIfEnd(); //findout how many players survived explosion
    }

    //upgrade blast lenght
    public void IncrementExplosionLenght()
    {
        explosionLenght++;
    }
}

