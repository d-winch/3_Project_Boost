using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(12f, 0f, 0f);
    [SerializeField] float period = 2f;

    [Range(-1,1)][SerializeField] float movementFactor; //0 for not moved, 1 for fully moved

    Vector3 startingPosition; // Must be stored for absolute movement

	// Use this for initialization
	void Start () {
        startingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float cycles = Time.time / period; //Grows continually from 0
        const float tau = Mathf.PI * 2; // About 6.28
        float rawSineWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSineWave;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
	}
}
