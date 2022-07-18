using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBuilder : MonoBehaviour
{
    // Object management
    public GameObject[] objectPhases;

    // Growth management
    public float growthDelayTimer;
    int currentPhase = 0;

    // Start is called before the first frame update
    void Start()
    {
        DisableAllToStart();
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
        if (objectPhases.Length > currentPhase)
        {
            Invoke("GrowNextPhase", growthDelayTimer);
        }
    }

    void GrowNextPhase()
    {
        objectPhases[currentPhase].SetActive(true);
        currentPhase++;
        ProcessNextPhase();
    }

}
