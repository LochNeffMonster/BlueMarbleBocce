using UnityEngine;
using System.Collections;

public class PowerSlider : MonoBehaviour {
    BallController ballController;
    public GameObject powerBar;
    public float percentFull = 0.0f;
    float powerBarMaxZScale = 27.5f;
    float powerBarMaxZOffset = 4.0f;

	// Use this for initialization
	void Start () {
        // store the instance of the BallController so that the current throw force can be used
        ballController = (BallController)FindObjectOfType(typeof(BallController));
        // use the max size of the bar to know where the center of the bar will end up
        powerBarMaxZOffset = 0.5f * powerBarMaxZScale;
	}
	
	// Update is called once per frame
	void Update () {

        percentFull = ballController.getThrowPercent();
        Transform powerBarTransform = powerBar.transform;
        // resize the power bar to relative size of the throw force;
        powerBar.transform.localScale = new Vector3(powerBarTransform.localScale.x, powerBar.transform.localScale.y, percentFull * powerBarMaxZScale);
        // move the power bar closer or further from the center of the court, since the position will be the center of the bar object
        powerBar.transform.position = new Vector3(powerBarTransform.position.x, powerBarTransform.position.y, -9 + percentFull * powerBarMaxZOffset);      
	}

    void OnGUI()
    {

    }
}
