using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCycler : MonoBehaviour
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
        foreach (GameObject theseObjects in objectPhases)
        {
            theseObjects.SetActive(false);
        }
    }

    void ProcessNextPhase()
    {
        objectPhases[currentPhase].SetActive(true);
        Invoke("SwapToNextPhase", growthDelayTimer);
    }

    void SwapToNextPhase()
    {
        objectPhases[currentPhase].SetActive(false);
        currentPhase++;
        if(currentPhase > objectPhases.Length - 1)
        {
            currentPhase = 0;
        }
        ProcessNextPhase();
    }
}
