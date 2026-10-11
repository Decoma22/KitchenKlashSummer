using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera perspectiveCamera;
    private Camera currentWorkstationCamera;
    public bool IsUsingWorkstationCamera;
    private Workstation currentWorkstation;
    public int currentIndex = 0;
    private Transform workstationParent;
    private MinigameManager minigameManager;
    public Ingredients ingredients;


    public void Start()
    {
        ShowPerspectiveCamera();
        IsUsingWorkstationCamera = false;
        workstationParent = GameObject.Find("Workstations").transform;
        minigameManager = FindFirstObjectByType<MinigameManager>();
        ingredients = FindFirstObjectByType<Ingredients>();

    }
    public void Update()
    {
        //TO-DO: move this process to an input script 
        if (currentWorkstationCamera != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                minigameManager.StopMinigame();
                ShowPerspectiveCamera();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SwitchWorkstation(1);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SwitchWorkstation(-1);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ingredients.DropItem();
            }
        }
        }

    public void SwitchWorkstation(int direction)
    {
        int workstationCount = workstationParent.childCount;
        currentIndex += direction;

        currentIndex = Mathf.Clamp(currentIndex, 0, workstationCount - 1);

        Debug.Log("Switching indext to:" + currentIndex);

        Transform nextWorkstation = workstationParent.GetChild(currentIndex);
        Workstation workstation = nextWorkstation.GetComponent<Workstation>();

        if (workstation != null)
        {
            workstation.EnterWorkstation();
        }
        //TO-DO: Debug switching (double press then offness always. might swithc index handling to cameraSwitcher
    }


    public void ShowPerspectiveCamera()
    {
        if (currentWorkstationCamera == null)
        {
      //      Debug.LogError("Camera Switch Error: unassigned Perspective camera");
        }
        if (currentWorkstationCamera != null)
        {
            currentWorkstationCamera.enabled = false;
            IsUsingWorkstationCamera = false;
            currentWorkstationCamera = null;
        }
        perspectiveCamera.enabled = true;
      //  Debug.LogError("Camera showing perspective");
    }

    public void ShowWorkstationCamera(Camera workstationCamera)
    {
        if (workstationCamera == null)
        {
            //Debug.LogError("Camera switch error: workstation camera unassigned");
        }
        if (currentWorkstationCamera == null)
        {
          //  Debug.LogError("Camera Switch Error: unassigned Perspective camera");
        }
        perspectiveCamera.enabled = false;

        if (currentWorkstationCamera != null)
        {
            currentWorkstationCamera.enabled = false;
        }


        currentWorkstationCamera = workstationCamera;
        currentWorkstationCamera.enabled = true;
        IsUsingWorkstationCamera = true;

        currentWorkstation = workstationCamera.GetComponentInParent<Workstation>();
        currentIndex = currentWorkstation.transform.GetSiblingIndex();

       // Debug.LogError("Showing workstation camera: " + currentWorkstationCamera.gameObject.name);

    }
}
