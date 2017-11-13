using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedRocket : MonoBehaviour {

    [SerializeField] ParticleSystem explosionParticle;
    float destroyTime = 2f;
    bool played = false;

    // Use this for initialization
    void Start ()
    {

    }

	// Update is called once per frame
	void Update ()
    {
        if (!played)
        {
            explosionParticle.Play();
            played = true;
        }
        Destroy(gameObject, destroyTime);
    }
}
