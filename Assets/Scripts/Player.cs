using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //TO-DO: Get rid of Cruft
    //you can SerializeField to allow something to be modifiable in editor but not public
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] public Transform holdPoint;
    private CameraSwitch cameraSwitch;

    private void Start()
    {
        cameraSwitch = FindAnyObjectByType<CameraSwitch>();
    }

    private void Update()
    {
        if (cameraSwitch.IsUsingWorkstationCamera == false)
        {
            Evaluate_Movement();
        }
        
    }

    private void Evaluate_Movement()
        {
        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }
        //TO-DO: Remove Cruft
        //if moving diagonally the combined inputs (1,1) is more than (0,1), so normalize
        inputVector = inputVector.normalized;
        //converts input vector into a vector3 since position is vector3
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        //Time.deltaTime is the number of seconds since last frame, makes move framerate independent
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        //returns a normalized vector that reps the blue axis of the object
        //a slerp is a lerp used for rotation/spherical
        //slerp takes the current, new, and t (t beingover time it will go between the first two)
        float rotateSpeed = 15f;
        // Only rotate if the player is actually moving
        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(
                transform.forward,
                moveDir,
                Time.deltaTime * rotateSpeed
            );
        }
        // Debug.Log(inputVector);



    }

    public void Snap_Movement(Transform snapPoint)
    {
        transform.position = snapPoint.position;
    }

}
