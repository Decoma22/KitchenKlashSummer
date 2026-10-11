using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameChop : MonoBehaviour
{
    public IEnumerator Mini_Chop(Transform knife)
    {
        print("chopping");
        Vector3 lastMousePosition = Input.mousePosition;
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
                
                knife.Translate(0, mouseDelta.x, 0);
                if(knife.transform.position.x > 4)
                {
                    mouseDelta.x = 4;
                }
                if (knife.transform.position.x < 0.1)
                {
                    mouseDelta.x = 0;
                }
                lastMousePosition = Input.mousePosition;
            }


            yield return null;
        }
    }
}
