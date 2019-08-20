using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightController : InteractableObject {

    public float rotationAmount = 45;

    public override void Interact () {
        Vector3 newRot = transform.rotation.eulerAngles;
        newRot.y += rotationAmount;
        transform.rotation = Quaternion.Euler(newRot);
    }
}