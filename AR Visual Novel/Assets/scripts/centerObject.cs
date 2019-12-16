using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

public class centerObject : MonoBehaviour
{
    public GameObject PlaceIndicator;
    private ARSessionOrigin aROrigin;
    private ARRaycastManager m_RaycastManager;
    private Pose placementPose;
    private bool placementIsValid = false;
    // Start is called before the first frame update
    void Start()
    {
        aROrigin = GetComponent<ARSessionOrigin>();
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        m_RaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        placementIsValid = hits.Count > 0;
        if (placementIsValid)
        {

            placementPose = hits[0].pose;
            var camForward = Camera.main.transform.forward;
            Vector3 cameraBearing = new Vector3(camForward.x, 0, camForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

        }
    }
    private void UpdatePlacementIndicator()
    {
        if (placementIsValid)
        {
            PlaceIndicator.SetActive(true);
            PlaceIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
       

    }
}
