using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.XR.ARSubsystems; // Needed for TrackableType

public class ARTapPlace : MonoBehaviour
{
    public GameObject placement;
    private ARSessionOrigin arOrigin;
    private Pose placementPose; // Simple data structure that represents a 3D-point
    private ARRaycastManager rayCastMgr; // Needed to Raycast
    private bool placementValid = false;

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        rayCastMgr = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    private void UpdatePlacementIndicator() {
        if (placementValid) {
            placement.SetActive(true);
            placement.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        } else {
            placement.SetActive(false);
        }
    }

    private void UpdatePlacementPose() {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); // Describes the center point of our screen
        var hitmonchan = new List<ARRaycastHit>();
        // Physics.Raycast();
        // Casts a ray, from point origin, in direction direction, of length maxDistance, against all colliders in the Scence
        rayCastMgr.Raycast(screenCenter, hitmonchan, TrackableType.Planes);
        // Screenpoint where it shoots the ray from,
        // hitResults are objects where the ray hits a physical surface, 
        // Optional trackable type - defaults to .all, collisions with the physical world that can be detected. FeaturePoint is any distinguishing point that can be identified

        placementValid = hitmonchan.Count > 0;
        if (placementValid) {
            placementPose = hitmonchan[0].pose;

            var cameraForward
        }

    }
}
