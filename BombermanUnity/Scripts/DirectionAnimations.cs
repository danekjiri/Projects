using UnityEngine;

public class DirectionAnimation : MonoBehaviour
{
    //graphic component
    private SpriteRenderer sr;

    //graphic stuff
    public Sprite[] RunningSprites;
    public Sprite IdleSprite; 
    private int currentSprite = 0;

    //animation timing variabless
    private float nextActionTime = 0f;
    private float animationPeriod = 0.2f;
    private bool isIdle = true;

    //init grapical component
    private void Awake(){
        sr = GetComponent<SpriteRenderer>();
    }

    //every frame check state and update
    private void Update(){
        if ((isIdle == false) && (Time.time > nextActionTime)){ //if holding button update animation
            nextActionTime = Time.time + animationPeriod;
            currentSprite = ( currentSprite + 1 ) % RunningSprites.Length;
            sr.sprite = RunningSprites[currentSprite];
        }
        else if (isIdle == true){ //if not holding any key, show previous direction idle
            sr.sprite = IdleSprite;
        }
        
    }
    private void OnEnable() {
        sr.enabled = true;
    }

    private void OnDisable() {
        sr.enabled = false;
    }

    //change idle flag from outside
    public void IsIdle(bool currentState){
        isIdle = currentState;
    }
}
