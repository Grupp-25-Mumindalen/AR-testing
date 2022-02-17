using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.XR.ARSubsystems; // Needed for TrackableType

public class ARTapPlace : MonoBehaviour
{
    [SerializeField]
    private GameObject placement;
    [SerializeField]
    private GameObject objToPlace;
    private Pose placementPose; // Simple data structure that represents a 3D-point
    private ARRaycastManager rayCastMgr; // Needed to Raycast
    private ARPlaneManager arPlaneMgr;
    private bool placementValid = false;
    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        rayCastMgr = this.GetComponent<ARRaycastManager>();
        arPlaneMgr = this.GetComponent<ARPlaneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!active) {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
            PlaceObject();
        }
    }

    /*  placeObject()
        Checks for input and validity of placement indicator to place the object
    */
    private void PlaceObject() {
        if(placementValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) { // Checks if plane valid, if screen touched and check phase of the fingers (the first) to see if it just began
            Instantiate(objToPlace, placementPose.position, placementPose.rotation);
            active = true;
            DisablePlanes();
        }
    }

    public void EnablePlanes() {
        arPlaneMgr.enabled = true;
        active = false;
        placement.SetActive(true);
    }

    public void DisablePlanes() {
        arPlaneMgr.enabled = false;
        placement.SetActive(false);
    }

    public bool ObjActive() {
        return active;
    }

    /*  UpdatePlacementIndicator()
        Updates placement of object
    */
    private void UpdatePlacementIndicator() {
        if (placementValid) {
            placement.SetActive(true);
            placement.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        } else {
            placement.SetActive(false);
        }
    }

    /*  UpdatePlacementPose()
        Records where to place object
    */
    private void UpdatePlacementPose() {
        // Finding a plane to hit
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
            // Placement on hit plane
            placementPose = hitmonchan[0].pose;

            // Rotation and placement in relation to camera
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized; // Enhetsvektor, just a direction no scalar
            placementPose.rotation = Quaternion.LookRotation(cameraBearing); // Creates rotation from forward and upward direction
        }
    }
}