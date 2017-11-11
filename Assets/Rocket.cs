using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 5f;
    [SerializeField] float rocketThrust = 100f;
    Rigidbody rigidBody;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Okay collision");
                break;
            case "Fuel":
                Debug.Log("Fuel collision");
                break;
            default:
                Debug.Log("Dead");
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) // Can thrust while rotating
        {
            Debug.Log("Thrusting");
            rigidBody.AddRelativeForce(Vector3.up * rocketThrust);
            if (!audioSource.isPlaying) // Prevent Layer
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // Take manual control of rotation

        float rotationThisFrame = rcsThrust + Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Turning Left");
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("Turning Right"); 
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // Resume physics control of rotation
    }

}
