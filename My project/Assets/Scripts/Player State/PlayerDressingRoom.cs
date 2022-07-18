using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDressingRoom : MonoBehaviour
{
    public Material[] bodyMaterials;
    public Material[] headMaterials;
    public Material[] armMaterials;

    public int currBodyMatNum;
    public int currHeadMatNum;
    public int currArmMatNum;

    public SkinnedMeshRenderer body;
    public SkinnedMeshRenderer head;
    public MeshRenderer[] arms;

    // Connect to cosmetics master manager
    public CosmeticsMasterManager cosmeticsManager;

    private void Start()
    {
        cosmeticsManager = GameObject.Find("GameManager").GetComponentInChildren<CosmeticsMasterManager>();
    }

    // Start is called before the first frame update
    public void NextBodyMat()
    {
        currBodyMatNum++;
        if(currBodyMatNum >= bodyMaterials.Length)
        {
            currBodyMatNum = 0;
        }
        UpdateBodyMaterials();
    }

    public void NextHeadMat()
    {
        currHeadMatNum++;
        if(currHeadMatNum >= headMaterials.Length)
        {
            currHeadMatNum = 0;
        }
        UpdateHeadMaterials();
    }

    public void NextArmMat()
    {
        currArmMatNum++;
        if(currArmMatNum >= armMaterials.Length)
        {
            currArmMatNum = 0;
        }
        UpdateArmMaterials();
    }

    void UpdateBodyMaterials()
    {
        cosmeticsManager.UpdateBodyMaterial(bodyMaterials[currBodyMatNum]);
    }

    void UpdateHeadMaterials()
    {
        cosmeticsManager.UpdateHeadMaterial(headMaterials[currHeadMatNum]);
    }

    void UpdateArmMaterials()
    {
        cosmeticsManager.UpdateArmMaterial(armMaterials[currArmMatNum]);
    }
}
