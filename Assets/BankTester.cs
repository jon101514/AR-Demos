using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Allows for testing the Bank game in-editor. */
public class BankTester : MonoBehaviour
{

    // PRIVATE
    private const float ROTATE_SPEED = 32f;

    private void Update() {
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * ROTATE_SPEED * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
        }
    }

    private void Shoot() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            if (hit.transform.GetComponent<Shootable>() != null) { // If we hit a Shootable...

            }
        }
    }
}
