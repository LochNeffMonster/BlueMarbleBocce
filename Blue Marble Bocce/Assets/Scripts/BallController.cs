using UnityEngine;
using System.Collections;

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

        switch (currentState)
        {
            case ControllerState.Waiting:
                if (GameState._gameStateInstance.getState() == State.Palino_Throw)
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
                break;
            case ControllerState.Throwing:
                if (Input.GetKeyUp("space"))
                {
                    throwBall();
                    currentState = ControllerState.BallThrown;
                }
                else
                {
                    throwForce += forceIncrement;
                }
                
                break;
            case ControllerState.BallThrown:
                //if (currentBall.rigidbody.velocity == Vector3.zero)
                if (currentBall.rigidbody.IsSleeping())
                {
                    
                    if (GameState._gameStateInstance.getState() != State.Game_Over) {
                        if (!ballRequested)
                        {
                            ballRequested = true;
                            GameState._gameStateInstance.addBallToSet(currentBall);
                            GameState._gameStateInstance.updateGame();
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

    public void throwBall()
    {
        currentBall.rigidbody.isKinematic = false;
        currentBall.transform.parent = null;
        currentBall.rigidbody.AddForce(head.forward * throwForce, ForceMode.Impulse);
        // reset the force for the next ball
        throwForce = forceMinimum;
    }
}
