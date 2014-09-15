using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {
    private bool buttonHeld = false;
    private bool throwBall = false;
    public float forceMinimum = 0.001f;
    public float forceMaximum = 0.1f;
    private float forceIncrement = 0.001f;
    public float throwForce;

	// Use this for initialization
	void Start () {
        throwForce = forceMinimum;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space") && !buttonHeld)
            buttonHeld = true;
        else if (Input.GetKeyUp("space"))
        {
            buttonHeld = false;
            throwBall = true;
        }

        if(buttonHeld && (throwForce < forceMaximum)){
            throwForce += forceIncrement;
        }

        if(throwBall){
            rigidbody.AddForce(Vector3.forward * throwForce, ForceMode.Impulse);
        }
	}
}
