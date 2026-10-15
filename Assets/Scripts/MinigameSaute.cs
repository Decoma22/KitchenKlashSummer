using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MinigameSaute : MonoBehaviour
{
    //To-Do: Remove this comment, it is a test for Git Ignore.
    [SerializeField] private float rotationSpeed = 0.15f;
    [SerializeField] private float rotationSmoothness = 8f;
    float maxTiltX = 45f;
    float maxTiltZ = 45f;

    float resistanceDistance = 20f;
    float edgeResistance = 0.15f;

    public IEnumerator Mini_Pan(Transform pan)
    {

        print("Sauteing");
        Vector3 lastMousePosition = Input.mousePosition;
        Quaternion targetRotation = pan.rotation;

       

        while (true)
        {

            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;

            }
            if (Input.GetMouseButton(0))
                {
                    Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

                    float rotationX = mouseDelta.y * rotationSpeed;
                    float rotationZ = mouseDelta.x * rotationSpeed;

                Vector3 currentRotation = targetRotation.eulerAngles;
                currentRotation.x = NormalizeAngle(currentRotation.x);
                currentRotation.z = NormalizeAngle(currentRotation.z);

                float xResistance = GetResistance(currentRotation.x, maxTiltX);
                float zResistance = GetResistance(currentRotation.z, maxTiltZ);

                rotationX *= xResistance;
                rotationZ *= zResistance;

                Quaternion deltaRotation = Quaternion.Euler(rotationX, rotationZ, 0f);

                    targetRotation = targetRotation * deltaRotation;


                print(pan.rotation);
                lastMousePosition = Input.mousePosition;
                }

                pan.rotation = Quaternion.Slerp(pan.rotation, targetRotation, rotationSmoothness * Time.deltaTime);



            yield return null;
    }
}
    private float GetResistance(float currentAngle, float maxAngle)
    {
        float distanceToEdge = maxAngle - Mathf.Abs(currentAngle);

        if (distanceToEdge >= resistanceDistance)
        {
            return 1f;
        }
        float t = Mathf.InverseLerp(0f, resistanceDistance, distanceToEdge);
        t = Mathf.SmoothStep(0f, 1f, t);
        return Mathf.Lerp(edgeResistance, 1f, t);
    }
    private float NormalizeAngle(float angle) 
    { 
        if (angle > 180f)
        {
            angle -= 360f;
        }
        return angle;
    }

}
