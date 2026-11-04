using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatText : MonoBehaviour
{
    Transform mainCam;
    Transform origin;
    [SerializeField] Transform worldSpaceCanvas;
    [SerializeField] private GameHandler.InstructStep currentInstruct;

    public Vector3 offset;
    void Start()
    {

        mainCam = Camera.main.transform;
        origin = transform.parent;
        transform.SetParent(worldSpaceCanvas);
     }

    void Update()
    {

        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        transform.position = origin.position - offset;
        
    }
    public void InstructStepUp()
    {
        currentInstruct = (GameHandler.InstructStep)((int)currentInstruct + 1);
    }
}
