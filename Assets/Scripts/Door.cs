﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject {
    public override void Interact () {
        if (GameManager.Instance.numKeys > 0) {
            Debug.Log ("Opening door");
            GameManager.Instance.numKeys--;
            Destroy (gameObject);
        } else {
            Debug.Log ("No key!");
        }
    }
}