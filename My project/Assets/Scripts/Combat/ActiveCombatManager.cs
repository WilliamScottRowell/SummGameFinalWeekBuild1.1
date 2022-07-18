using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;

public class ActiveCombatManager : MonoBehaviour
{
    public CombatController meleeController;
    public ProjectileLauncher projectileController;
    public MeshRenderer[] projectileVisuals;
    public GameObject targetReticle;

    bool meleeOn = true;

    // Start is called before the first frame update
    void Start()
    {
        meleeController.meleeEnabled = true;
        projectileController.projectilesEnabled = false;
        HideProjectileVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            meleeOn = !meleeOn;

            if(meleeOn)
            {
                meleeController.meleeEnabled = true;
                projectileController.projectilesEnabled = false;
                HideProjectileVisuals();
            }
            else
            {
                meleeController.meleeEnabled = false;
                projectileController.projectilesEnabled = true;
                ShowProjectileVisuals();
            }
        }
    }

    void ShowProjectileVisuals()
    {
        foreach(MeshRenderer renderer in projectileVisuals)
        {
            renderer.enabled = true;
            targetReticle.SetActive(true);
        }
    }

    void HideProjectileVisuals()
    {
        foreach (MeshRenderer renderer in projectileVisuals)
        {
            renderer.enabled = false;
            targetReticle.SetActive(false);
        }
    }
}
