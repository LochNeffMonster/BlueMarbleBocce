using UnityEngine;
using System.Collections;

public class CourtBounds : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit(Collider other)
    {
        other.gameObject.tag = "OUT_OF_BOUNDS";
        //Destroy(other.gameObject);
    }
}
