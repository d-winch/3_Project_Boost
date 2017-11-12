using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] GameObject destroyedRocket;

    [SerializeField] float rcsThrust = 5f;
    [SerializeField] float rocketThrust = 100f;
    [SerializeField] bool hardcoreMode;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    // Use this for initialization
    void Start ()
   {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");

        if(state != State.Alive) { return; } // Guard condition

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Okay collision");
                break;
            case "Fuel":
                Debug.Log("Fuel collision");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        Debug.Log("Dead");
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(explosion);
        Instantiate(destroyedRocket, transform.position, transform.rotation);
        transform.position = new Vector3(0, 1000, 0);
        rigidBody.velocity = Vector3.zero;
        Invoke("Death", 2f); // paramaterise time
    }

    private void StartSuccessSequence()
    {
        Debug.Log("Landing Pad collision");
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextScene", 2f); // paramaterise time
    }

    private void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadNextScene()
    {
        state = State.Alive;
        SceneManager.LoadScene(1); // todo allow for more than two levels
    }

    private void RespondToThrustInput()
    {
        if(state != State.Alive) { return; } // Guard condition

        if (Input.GetKey(KeyCode.Space)) // Can thrust while rotating
        {
            ApplyThrust();
        }
        else if (Input.GetKeyUp(KeyCode.Space) && hardcoreMode)
        {
            StartDeathSequence();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        Debug.Log("Thrusting");
        rigidBody.AddRelativeForce(Vector3.up * rocketThrust);
        if (!audioSource.isPlaying) // Prevent Layer
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
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
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
    }

}
