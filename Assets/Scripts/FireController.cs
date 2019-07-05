using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour {
    public GameObject Light;
    public float Fuel = 100;
    public float burnRate = 2f;
    public float MaxRange;
    private float fireScale;

    // Use this for initialization
    void Start () {
        // StartCoroutine("oneSecPrint");
    }

    IEnumerator oneSecPrint () {
        while (true) {
            yield return new WaitForSeconds (1f);
            OutputFuel ();
        }
    }

    // Update is called once per frame
    void Update () {
        burnFuel ();
    }

    public void Interact () {
        if (GameManager.Instance.numSticks > 0)
            putFuelInFire ();
        else
            takeFuelFromFire ();
    }

    void burnFuel () {
        if (Fuel > 0) {
            Fuel -= burnRate * Time.deltaTime;
            fireScale = Mathf.Clamp (Fuel, 0, MaxRange);
            Light.transform.localScale = new Vector3 (fireScale / 2, fireScale / 2, fireScale / 2);
        }
    }

    /// <summary>
    /// Take fuel out of the campfire
    /// </summary>
    private void takeFuelFromFire () {
        Debug.Log ("Just took 10 fuel");
        int amount = 10;

        if (Fuel > amount) {
            Fuel -= amount;
            GameManager.Instance.torch.torchFuel += amount;
        } else {
            Debug.Log ("Not Enough Fuel!!");
        }
    }

    /// <summary>
    /// Put all currently held fuel into the campfire
    /// </summary>
    private void putFuelInFire () {
        Debug.Log ("Added " + (GameManager.Instance.numSticks) + " to Fire");

        Fuel += GameManager.Instance.numSticks;
        GameManager.Instance.numSticks = 0;
    }

    //for testing purposes only
    void OutputFuel () {
        Debug.Log ("CAMPFIRE :: Remaining Fuel: " + Fuel);
    }
}