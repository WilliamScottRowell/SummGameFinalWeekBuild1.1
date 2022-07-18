using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChanger : MonoBehaviour
{
    // Object management
    public GameObject[] objectPhases;
    GameObject activeObject;

    // Growth management
    public float growthDelayTimer;
    int currentPhase = 0;

    // Start is called before the first frame update
    void Start()
    {
        DisableAllToStart();
        activeObject = objectPhases[currentPhase];
        ProcessNextPhase();
    }

    void DisableAllToStart()
    {
        foreach(GameObject theseObjects in objectPhases)
        {
            theseObjects.SetActive(false);
        }
    }

    void ProcessNextPhase()
    {
        objectPhases[currentPhase].SetActive(true);
        if (objectPhases.Length > currentPhase + 1)
        {
            Invoke("SwapToNextPhase", growthDelayTimer);
        }
    }

    void SwapToNextPhase()
    {
        objectPhases[currentPhase].SetActive(false);
        if (objectPhases.Length > currentPhase + 1)
        {
            currentPhase++;
            ProcessNextPhase();
        }
    }

}
