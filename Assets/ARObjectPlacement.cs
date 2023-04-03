using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;


/** 
Modified from Samyam's Tutorial found here: https://www.youtube.com/watch?v=mDLmqhhY-6g
*/
[RequireComponent(requiredComponent: typeof(ARRaycastManager), requiredComponent2: typeof(ARPlaneManager))]
public class ARObjectPlacement : MonoBehaviour
{
    // PUBLIC
    public GameObject prefab; // item to place

    // PRIVATE
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // COMPONENTS
    private ARRaycastManager _arRM;
    private ARPlaneManager _arPM;

    private void Awake() {
        _arPM = GetComponent<ARPlaneManager>();
        _arRM = GetComponent<ARRaycastManager>();
    }

    private void OnEnable() {
        // turn on enhanced touchsupport
        EnhancedTouch.TouchSimulation.Enable(); // For simulating in editor
        EnhancedTouch.EnhancedTouchSupport.Enable(); // actually enables 
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable() {
        EnhancedTouch.TouchSimulation.Disable(); // For simulating in editor
        EnhancedTouch.EnhancedTouchSupport.Disable(); // actually disable 
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    /** Event fires off when the user touches the screen.
    */
    private void FingerDown(EnhancedTouch.Finger finger) {
        if (finger.index != 0) { return; }

        if (_arRM.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon)) {
            foreach (ARRaycastHit hit in hits) {
                Pose pose = hit.pose;
                GameObject obj = Instantiate(prefab, pose.position, pose.rotation);
                if (_arPM.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp) {
                    Vector3 pos = obj.transform.position;
                    Vector3 camPos = Camera.main.transform.position;
                    Vector3 dir = camPos - pos; // facing direction
                    Vector3 targetRot = Quaternion.LookRotation(dir).eulerAngles;
                    Vector3 scaledEuler = Vector3.Scale(targetRot, obj.transform.up.normalized);
                    Quaternion targetRotQuat = Quaternion.Euler(scaledEuler);
                    obj.transform.rotation = obj.transform.rotation * targetRotQuat;
                }
            }
        }
    }
}
