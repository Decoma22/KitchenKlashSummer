using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameChop : MonoBehaviour
{
    public IEnumerator Mini_Chop(Transform knife, Transform ingredient)
    {
        print("chopping");
        Vector3 lastMousePosition = Input.mousePosition;
        float slideRate = 0.001f;
       
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
                float moveY = mouseDelta.y * Time.deltaTime * 0.1f;

                Vector3 newPosition = knife.position;
                newPosition.y = Mathf.Clamp(newPosition.y + moveY, 1.32f, 1.8f);
                knife.position = newPosition;
                
                lastMousePosition = Input.mousePosition;
            }
            if (Input.GetKey(KeyCode.A))
            {
                float moveX = -slideRate;

                Vector3 newPositionIng = ingredient.position;
                //newPositionIng.x = newPositionIng.x + moveX;
                newPositionIng.x = Mathf.Clamp(newPositionIng.x + moveX, -6.85f, -6.3f);
                ingredient.position = newPositionIng;
               // print(ingredient.position);
            }
            if (Input.GetKey(KeyCode.D))
            {
                float moveX = slideRate;

                Vector3 newPositionIng = ingredient.position;
                //newPositionIng.x = newPositionIng.x + moveX;
                newPositionIng.x = Mathf.Clamp(newPositionIng.x + moveX, -6.85f, -6.3f);
                ingredient.position = newPositionIng;
                //print(ingredient.position);
            }

            yield return null;
        }
    }
}

