using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour {
    public GameObject Player;
    public int stickValue = 20;

    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.name == "Player") {
            Debug.Log ("You picked up a stick!");
            GameManager.Instance.numSticks += stickValue;
            Destroy (gameObject);
        }
    }
}