using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticsUpdateManager : MonoBehaviour
{
    public Material bodyMaterial;
    public Material headMaterial;
    public Material armMaterial;

    public SkinnedMeshRenderer body;
    public SkinnedMeshRenderer head;
    public MeshRenderer[] arms;

    public CosmeticsMasterManager cosmeticManager;

    private void Start()
    {
        cosmeticManager = GameObject.Find("GameManager").GetComponentInChildren<CosmeticsMasterManager>();
        cosmeticManager.CreatePlayerConnections();
        cosmeticManager.UpdateAllMaterials();
    }

    public void SetNewBodyMaterial(Material newMat)
    {
        bodyMaterial = newMat;
        UpdateBodyMaterials();
    }

    public void SetNewHeadMaterial(Material newMat)
    {
        headMaterial = newMat;
        UpdateHeadMaterials();
    }

    public void SetNewArmMaterial(Material newMat)
    {
        armMaterial = newMat;
        UpdateArmMaterials();
    }

    public void UpdateAllMaterials()
    {
        UpdateArmMaterials();
        UpdateBodyMaterials();
        UpdateHeadMaterials();
    }

    public void UpdateBodyMaterials()
    {
        Material[] mats = body.materials;
        mats[1] = bodyMaterial;
        body.materials = mats;
    }

    public void UpdateHeadMaterials()
    {
        Material[] mats = body.materials;
        mats[0] = headMaterial;
        body.materials = mats;
    }

    public void UpdateArmMaterials()
    {
        arms[0].material = armMaterial;
        arms[1].material = armMaterial;
    }

}
