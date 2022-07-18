using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticsMasterManager : MonoBehaviour
{
    public CosmeticsUpdateManager playerCostmeticManager;
    public PlayerDressingRoom dressingRoom;

    // Holding variables for selected cosmetics for each slot
    public Material headMaterial;
    public Material bodyMaterial;
    public Material armMaterial;

    private void Start()
    {
        CreatePlayerConnections();
        SetInitialMaterials();
        UpdateAllMaterials();
    }

    // Start is called before the first frame update
    public void CreatePlayerConnections()
    {
        playerCostmeticManager = GameObject.Find("Player").GetComponent<CosmeticsUpdateManager>();
        dressingRoom = GameObject.Find("PlayerDressingRoom").GetComponent<PlayerDressingRoom>();
    }

    void SetInitialMaterials()
    {
        headMaterial = dressingRoom.headMaterials[0];
        bodyMaterial = dressingRoom.bodyMaterials[0];
        armMaterial = dressingRoom.armMaterials[0];
    }

    public void UpdateHeadMaterial(Material newMat)
    {
        headMaterial = newMat;
        playerCostmeticManager.SetNewHeadMaterial(headMaterial);
    }

    public void UpdateBodyMaterial(Material newMat)
    {
        bodyMaterial = newMat;
        playerCostmeticManager.SetNewBodyMaterial(bodyMaterial);
    }

    public void UpdateArmMaterial(Material newMat)
    {
        armMaterial = newMat;
        playerCostmeticManager.SetNewArmMaterial(armMaterial);
    }

    public void UpdateAllMaterials()
    {
        playerCostmeticManager.SetNewHeadMaterial(headMaterial);
        playerCostmeticManager.SetNewArmMaterial(armMaterial);
        playerCostmeticManager.SetNewBodyMaterial(bodyMaterial);
    }



}
