using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnOrOff : MonoBehaviour
{
    public GameObject objectToToggle;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision detected");
            if(objectToToggle.activeInHierarchy== true)
            {
                objectToToggle.SetActive(false);
            }
            else
            {
                objectToToggle.SetActive(true);
            }
        }
    }


}
