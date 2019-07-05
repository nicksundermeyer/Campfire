using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.name == "Player") {
            Debug.Log ("You picked up a key!");
            GameManager.Instance.numKeys++;
            Destroy (gameObject);
        }
    }
}