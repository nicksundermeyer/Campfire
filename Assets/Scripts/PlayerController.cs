using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    //movement variable
    public float movementSpeed = 20f;

    // light level checking
    public float minLightValue;
    private LightCheck lightChecker;

    // time allowed to be in darkness
    private float darknessTimer = 0f;
    public float maxDarknessTime;

    public bool printLightLevel;

    private Rigidbody rb;

    private void Awake() {
        lightChecker = GetComponentInChildren<LightCheck>();
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        Move();

        if(printLightLevel)
        {
            Debug.Log("Light Level: " + lightChecker.LightLevel);
        }

        if(lightChecker.LightLevel < minLightValue)
        {
            // Debug.Log("In Darkness: " + lightChecker.LightLevel);

            if(darknessTimer == 0f)
            {
                darknessTimer = Time.time;
            }

            if((Time.time - darknessTimer) > maxDarknessTime)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            darknessTimer = 0f;
        }
	}

    //call this to move the player 1 frame
    private void Move()
    {
        float horizAxis = Input.GetAxis("Horizontal");
        float vertAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(movementSpeed * Time.deltaTime * horizAxis, rb.velocity.y, movementSpeed * Time.deltaTime * vertAxis);

        rb.velocity = movement;
    }
}
