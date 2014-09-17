using UnityEngine;
using System.Collections;

public enum ControllerState { Waiting, HoldingBall, Throwing, BallThrown}

public class BallController : MonoBehaviour {

    public ControllerState currentState = ControllerState.Waiting;
    public Transform head;
    private bool buttonHeld = false;
    private bool ballHeld;
    private bool timeToThrow = false;
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
        currentBall = (GameObject) GameObject.Instantiate(ballPrefab, gameObject.transform.position, gameObject.transform.rotation);
        currentBall.transform.parent = gameObject.transform;
        ballHeld = true;
        currentBall.rigidbody.isKinematic = true;
        currentState = ControllerState.HoldingBall;

	}
	
	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case ControllerState.Waiting:
                break;
            case ControllerState.HoldingBall:
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
                if (currentBall.rigidbody.velocity == Vector3.zero)
                {
                    generateBall();
                    currentState = ControllerState.HoldingBall;
                }
                break;
        }
        /*if (currentBall) { 
            if (Input.GetKeyDown("space") && !buttonHeld)
                buttonHeld = true;
            else if (Input.GetKeyUp("space"))
            {
                buttonHeld = false;
                timeToThrow = true;
            }
        
            if(buttonHeld && (throwForce < forceMaximum)){
                throwForce += forceIncrement;
            }

            
            if (currentBall.rigidbody.velocity == Vector3.zero && ballHeld == false)
            {
                generateBall();
            }
            if(timeToThrow){
                throwBall();
            }
        }*/
	}
    public void generateBall()
    {
        currentBall = (GameObject)GameObject.Instantiate(ballPrefab, gameObject.transform.position, gameObject.transform.rotation);
        currentBall.transform.parent = gameObject.transform;
        ballHeld = true;
        currentBall.rigidbody.isKinematic = true;
    }

    public void throwBall()
    {
        currentBall.rigidbody.isKinematic = false;
        currentBall.transform.parent = null;
        currentBall.rigidbody.AddForce(head.forward * throwForce, ForceMode.Impulse);
        timeToThrow = false;
        ballHeld = false;
    }
}
