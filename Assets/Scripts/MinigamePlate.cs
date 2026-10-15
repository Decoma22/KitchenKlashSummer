using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinigamePlate : MonoBehaviour
{
    private Rigidbody rb;
    private MeshCollider MeshCollider;
    
    public IEnumerator Mini_Plate(Transform plate, Transform ingredient)
    {
        rb = ingredient.GetComponentInChildren<Rigidbody>();
        MeshCollider = ingredient.GetComponentInChildren<MeshCollider>();
        print("plating");
        float basicY = plate.position.y + 0.5f;

        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                rb.useGravity = false;
                MeshCollider.enabled = false;
                float xScreenScale = Input.mousePosition.x / Screen.width;
                float yScreenScale = (Input.mousePosition.y / Screen.height);


                Vector3 newPosition;
                
                newPosition.y = basicY;
                newPosition.z = Mathf.Lerp(-8f, 6f, yScreenScale) + 8f;
                newPosition.x = Mathf.Lerp(-4f, 4f, xScreenScale);
                newPosition.z = Mathf.Clamp(newPosition.z, -6.6f, 6.6f);
                newPosition.x = Mathf.Clamp(newPosition.x, -3f, -1f);
                ingredient.position = newPosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                print("dropped");
                rb.useGravity = true;
                MeshCollider.enabled = true;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                ingredient.SetParent(plate, true);
                Workstation workstation = plate.GetComponentInParent<Workstation>();
                if (workstation != null)
                {
                    Destroy(workstation);
                }
                plate.SetParent(null, true);
                if (plate.GetComponent<Ingredients>() == null)
                {
                    plate.AddComponent<Ingredients>();
                }
            }
            yield return null;
        }
    }
}