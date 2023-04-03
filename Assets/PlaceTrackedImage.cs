using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/** Given an ARTrackedImageManager, tracks images and places objects on them if detected.
Modified from code by Allister of Playful Technology in the tutorial found here: https://youtu.be/gpaq5bAjya8
*/

// Maps a string name to a GameObject prefab.
[System.Serializable]
public struct DioramaImage{
    public string name;
    public GameObject diorama;
}

[RequireComponent(typeof(ARTrackedImageManager))]
public class PlaceTrackedImage : MonoBehaviour {

    // Ref to AR tracked image manager component
    private ARTrackedImageManager _trackedImageManager;

    // Prefabs
    public List<DioramaImage> ARPrefabs; // name of prefab should match the corresponding 2D image in the reference images

    // Dict of created prefabs
    private readonly Dictionary<string, GameObject> _instPrefabs = new Dictionary<string, GameObject>();

    void Awake() {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    // Add the event
    void OnEnable() {
        _trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    // Remove the event
    void OnDisable() {
        _trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // Event Handler
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
        foreach (var trackedImg in eventArgs.added) {
            var imgName = trackedImg.referenceImage.name;
            foreach (var curPrefab in ARPrefabs) {
                if (curPrefab.name == imgName && !_instPrefabs.ContainsKey(imgName)) {
                    GameObject newPrefab = Instantiate (curPrefab.diorama, trackedImg.transform); // anchor that new prefab to the tracked image
                    _instPrefabs[imgName] = newPrefab;
                }
            }
        }
        // Update: set them active if their image is currently being tracked (within the FOV in the world)
        foreach (var trackedImg in eventArgs.updated) {
            _instPrefabs[trackedImg.referenceImage.name].SetActive(trackedImg.trackingState == TrackingState.Tracking);
        }
        // Destroy
        foreach (var trackedImg in eventArgs.removed) {
            Destroy(_instPrefabs[trackedImg.referenceImage.name]);
            _instPrefabs.Remove(trackedImg.referenceImage.name);

        }
    }
}
