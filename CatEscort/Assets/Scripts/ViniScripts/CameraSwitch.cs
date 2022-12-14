using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Events;
using System.Linq;

public class CameraSwitch : MonoBehaviour
{
    Controls Input;
    public int CamIndex = 0;

    [Tooltip("These are the cameras to cycle in between")]
    [Header("Cameras")]
    public List<CinemachineVirtualCamera> Cameras = new List<CinemachineVirtualCamera>();
    [SerializeField] Transform CameraFollow;
    [SerializeField] GameObject corporalCat;
    [Tooltip("These are the Events that the space bar will activate")]
    [Header("Events")]
    public UnityEvent Events;

    CinemachineVirtualCamera CurrentCam;


    private void Awake()
    {
        Input = new Controls();
        GetAllCameras();
        Input.Actions.ActionButton.performed += ctx =>
        {
            Events.Invoke();
        };
        Input.Actions.LeftButton.performed += ctx =>
        {
            ChangeToNextCam();
        };
        Input.Actions.RightButton.performed += ctx =>
        {
            ChangeToPrevCam();
        };
        
    }
    //Gets all cameras from the scene and addst them to the Cameras list, it also instantiatest the first Camera
    private void GetAllCameras()
    {
        // Getting all Cinemachine Virtual Cameras and adding them to the list
        var CameraStandin = FindObjectsOfType<CinemachineVirtualCamera>();
        foreach(var n in CameraStandin)
        {
            Cameras.Add(n);
        }
        Cameras = Cameras.OrderBy(go => go.name).ToList();

        //Getting the default camera and setting that to be the active camera
        CurrentCam = Cameras[CamIndex];
        CurrentCam.Priority = 100;
       // corporalCat.transform.LookAt(new Vector3(CameraFollow.transform.position.x, CameraFollow.transform.position.y, CameraFollow.transform.position.z));
    }

    private void ChangeToNextCam()
    {
        CurrentCam.Priority = 0;
        //check if Camera is at end of list
        if (CamIndex == (Cameras.Count - 1))
        {
            CamIndex = 0;
        }
        else
        {
            CamIndex++;
        }
        CurrentCam = Cameras[CamIndex];
        CurrentCam.Priority = 100;
        //corporalCat.transform.LookAt(CameraFollow);

    }

    private void ChangeToPrevCam()
    {
        CurrentCam.Priority = 0;
        //check if Camera is at start of list to wraparound if not then just subtract 1 from the index
        if (CamIndex == 0)
        {
            CamIndex = (Cameras.Count - 1);
        }
        else
        {
            CamIndex--;
        }
        CurrentCam = Cameras[CamIndex];
        CurrentCam.Priority = 100;
        //corporalCat.gameObject.transform.LookAt(new Vector3(CameraFollow.transform.position.x, CameraFollow.transform.position.y, CameraFollow.transform.position.z));
    }

    private void OnEnable()
    {
        Input.Enable();
    }

    private void OnDisable()
    {
        Input.Disable();  
    }
}
