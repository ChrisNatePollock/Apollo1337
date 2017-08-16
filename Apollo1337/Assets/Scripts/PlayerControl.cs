using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable] needs to be above the class "Boundary" or unity cannot see it
//This allows it to be visible in the inspector
[System.Serializable]

public class Boundary {
    public float xMin, xMax, zMin, zMax;
}

public class PlayerControl : MonoBehaviour {

    //ship public vars
    public float speed;
    public float tilt;
    public Boundary boundary;

    //shooting public vars
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    //instantiate rigidbody
    private Rigidbody rb;

    //instantiate audiosource
    private AudioSource audioSource;

    private float nextFire;

    void Start() {
        //we need to instantiate the rigidbody component
        rb = GetComponent<Rigidbody>();
        //we need to instantiate the audiosource component
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if ((Input.GetButton("Jump") || Input.GetButton("Fire1")) && Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audioSource.Play();
        }
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        rb.position = new Vector3
            (
                Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}