using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //player data
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.down; //start position
    private int speed;

    //control movement keys
    public KeyCode UpKey = KeyCode.W;
    public KeyCode LeftKey = KeyCode.A;
    public KeyCode RightKey = KeyCode.D;
    public KeyCode DownKey = KeyCode.S;

    //movement directions
    public DirectionAnimation DirUp;
    public DirectionAnimation DirDown;
    public DirectionAnimation DirLeft;
    public DirectionAnimation DirRight;
    private DirectionAnimation dirCurrent;

    //returnKey to main menu
    public KeyCode ReturnKey = KeyCode.Escape;
    
    //init start direction
    private void Start()
    {
        speed = getSetStartSpeed();
        dirCurrent = DirDown;
        DirDown.enabled = true;
        DirLeft.enabled = DirRight.enabled = DirUp.enabled = false;
    }

    //get initial speed set in settings
    private int getSetStartSpeed()
    {
        int value = 2;

        if (PlayerPrefs.HasKey("startSpeed"))
            value = PlayerPrefs.GetInt("startSpeed") + 1; //speed value stored as dropbox array indexes -> index 0 is meant to be speed 1
        
        return value;
    }

    //init player object 
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //every frame check inputs and move object given direction
    private void Update()
    {
        checkIfReturnKey();
        checkInput();
        movePlayer();
    }

    //return to Main menu - press ESCAPE
    private void checkIfReturnKey()
    {
        if (Input.GetKey(ReturnKey) == true)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    //check all keys pressed in current frame
    private void checkInput()
    {
        if (Input.GetKey(UpKey) == true)
        {
            upadtePosition(Vector2.up, DirUp);
            dirCurrent = DirUp;
            DirUp.IsIdle(false);
        }
        else if (Input.GetKey(DownKey) == true)
        {
            upadtePosition(Vector2.down, DirDown);
            dirCurrent = DirDown;
            DirDown.IsIdle(false);
        }
        else if (Input.GetKey(LeftKey) == true)
        {
            upadtePosition(Vector2.left, DirLeft);
            dirCurrent = DirLeft;
            DirLeft.IsIdle(false);
        }
        else if (Input.GetKey(RightKey) == true)
        {
            upadtePosition(Vector2.right, DirRight);
            dirCurrent = DirRight;
            DirRight.IsIdle(false);
        }
        else
        {
            upadtePosition(Vector2.zero, dirCurrent);
            dirCurrent.IsIdle(true);
        }
    }

    //update animation and direction
    private void upadtePosition(Vector2 newDirection, DirectionAnimation newAnimation)
    {
        DirUp.enabled = (newAnimation == DirUp);
        DirDown.enabled = (newAnimation == DirDown);
        DirLeft.enabled = (newAnimation == DirLeft);
        DirRight.enabled = (newAnimation == DirRight);

        direction = newDirection;
    }

    //move player by new coordinates direction
    private void movePlayer()
    {
        Vector2 newPosition = rb.position + (speed * direction * Time.fixedDeltaTime); //new position by given vector and speed
        rb.MovePosition(newPosition);
    }

    //speed increase upgrade
    public void IncrementPlayerMovement()
    {
        speed++;
    }
    
}
