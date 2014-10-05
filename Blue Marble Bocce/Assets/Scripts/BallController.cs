using UnityEngine;
using System.Collections;


/// The Controller State component serves the purpose of throwing ball in the game world
/// 
/// The controller can be in the following states
public enum ControllerState { Waiting, HoldingBall, Throwing, BallThrown}

public class BallController : MonoBehaviour {

    public ControllerState currentState = ControllerState.Waiting;
    public Transform head;
    private bool ballHeld;
    public float forceMinimum = 1.0f;
    public float forceMaximum = 90.0f;
    public float forceIncrement = 1.0f;
    public float throwForce;

    public GameObject ballPrefab;
    private GameObject currentBall;

	// Use this for initialization
	void Start () {
        head = gameObject.transform.parent;
        throwForce = forceMinimum;
        currentState = ControllerState.Waiting;
	}
	
	// Update is called once per frame
	void Update () {
        bool ballRequested = false;

        // take different actions based on the state of the controller
        switch (currentState)
        {
            case ControllerState.Waiting:
                if (GameState._gameStateInstance.getState() == State.Pallino_Throw)
                {
                    if (!ballRequested)
                    {
                        ballRequested = true;
                        setBall(GameState._gameStateInstance.requestBall());
                        currentState = ControllerState.HoldingBall;
                    }

                }
                break;
            case ControllerState.HoldingBall:
                ballRequested = false;
                if (Input.GetKeyDown("space"))
                {
                    currentState = ControllerState.Throwing;
                }
                if (GUI.Button(new Rect(Screen.width/2, Screen.height/2, 50, 50), "Hold to Throw"))
                {
                    currentState = ControllerState.Throwing;
                    Debug.Log("Clicked the button with an image");
                }
                    
                break;
            case ControllerState.Throwing:
                if (Input.GetKeyUp("space"))
                {
                    throwBall();
                    currentState = ControllerState.BallThrown;
                }
                else
                {
                    if(throwForce < forceMaximum)
                        throwForce += forceIncrement;
                }
                
                break;
            case ControllerState.BallThrown:
                if (currentBall.rigidbody.IsSleeping() || currentBall.gameObject.tag == "OUT_OF_BOUNDS")
                {
                    
                    if (GameState._gameStateInstance.getState() != State.Menu) {
                        if (!ballRequested)
                        {   
                            GameState._gameStateInstance.addBallToSet(currentBall);
                            GameState._gameStateInstance.updateGame();
                            // request a new ball
                            ballRequested = true;
                            setBall(GameState._gameStateInstance.requestBall());
                        }
                    }
                    else
                    {
                        currentState = ControllerState.Waiting;
                    }
                }
                break;
        }
	}

    public void OnGUI()
    { 
        /*
        switch (currentState)
        {
            case ControllerState.HoldingBall:
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 200, 50), "Tap to increase power"))
                {
                    buttonPressed = true;
                    if (throwForce < forceMaximum){
                        throwForce += forceIncrement;
                    }

                }
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + Screen.height / 5, 100, 50), "Tap to Throw"))
                {
                    throwBall();
                    currentState = ControllerState.BallThrown;
                    buttonPressed = false;
                }
                break;
            case ControllerState.Throwing:
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 50, 50), "Let go to throw"))
                {
                    throwBall();
                    currentState = ControllerState.BallThrown;
                }
                break;
            default:
                break;
        }
         */
    }

    // set up the input object as the next ball to be thrown by the controller
    public void setBall(GameObject ball)
    {
        if (ball != null) { 
            currentBall = ball;
            currentBall.transform.position = gameObject.transform.position;
            currentBall.transform.rotation = gameObject.transform.rotation;
            currentBall.transform.parent = gameObject.transform;
            currentBall.rigidbody.isKinematic = true;
            currentState = ControllerState.HoldingBall;
        }
    }

    // throw the ball in the direction that the player is looking
    public void throwBall()
    {
        currentBall.rigidbody.isKinematic = false;
        currentBall.transform.parent = null;
        currentBall.rigidbody.AddForce(head.forward * throwForce, ForceMode.Impulse);
        // reset the force for the next ball
        throwForce = forceMinimum;
    }

    // return the current throw power relative to the max throw force
    public float getThrowPercent()
    {
        return throwForce/forceMaximum;
    }
}
