using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject Light;
    public GameObject player;
    public float Fuel = 100;
    public float burnRate = 2f;
    public float MaxRange;
    private float fireScale;

	// Use this for initialization
	void Start () {
        // StartCoroutine("oneSecPrint");
	}

    IEnumerator oneSecPrint()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            OutputFuel();
        }
    }
	
	// Update is called once per frame
    void Update () {
        burnFuel();
        // if (Fuel <= 0)
        // {
        //     player.GetComponent<PlayerController>().isDead = true;
        //     //Debug.Log("You're dead!");
        // }
	}

    void burnFuel()
    {
        if (Fuel > 0)
        {
            Fuel -= burnRate * Time.deltaTime;
            fireScale = Mathf.Clamp(Fuel, 0, MaxRange);
            Light.transform.localScale = new Vector3(fireScale / 2, fireScale / 2, fireScale / 2);
        }
    }

    //void giveFuel(float amount)
    //{
    //    //when space bar is pressed
    //    //if player is colliding with the fire
    //}

    //void receiveFuel(float amount)
    //{
    //    //if player is colliding with fire
    //    //give sticks from player inventory
    //}

    //for testing purposes only
    void OutputFuel()
    {
        Debug.Log("CAMPFIRE :: Remaining Fuel: " + Fuel);
    }
}
