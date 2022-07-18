using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public GameObject TalentTree;
    public GameObject ModelRoot;
    public GameObject[] obj, weapons;
    public float LightningDistance, LightningHeight, BeamDistance, minionDistance;
    public float FireballCooldown, WindBladesCooldown, IceSpikesCooldown, LightningCooldown, LightBeamCooldown, VoidBeamCooldown, MinionCooldown;
    float FC, WC, IC, LC, LBC, VC, MC;
    bool isEquipped = false;
    bool isMeleeAttack = false;
    int weaponIndex = 1, minionCycle = 0;
    PlayerStatsManager statsHandler;
    public GameObject player, weapon, weaponControls, rightArm, talentUI;
    Melee weaponLogic;
    public Vector3 weaponPlace;
    public Quaternion weaponRotation, activeWeaponRotation;
    public Combat state;
    public Rigidbody rigid;
    GameObject[] minions = new GameObject[3];

    // Melee System Control
    public bool meleeEnabled = true;


    void Start()
    {
        talentUI = GameObject.Find("TalentUI");
        statsHandler = GameObject.Find("StatManager").GetComponent<PlayerStatsManager>();
        rightArm = GameObject.FindWithTag("RightArm");
        player = this.gameObject;
        weaponLogic = GetComponentInChildren<Melee>();
        weaponControls = weaponLogic.gameObject;
        weapon = weaponControls.transform.GetChild(0).gameObject;
        weaponPlace = weapon.transform.localPosition;
        weaponRotation = weapon.transform.localRotation;
        activeWeaponRotation = weaponRotation * Quaternion.Euler(0f, -90f, 0f);
    }
    void Update()
    {
        // Melee System Logic
        if(meleeEnabled)
        {
            ShowMeleeVisuals();
            UpdateMeleeSystem();
        }
        else
        {
            HideMeleeVisuals();
        }
        
        // Magic Combat Logic
        UpdateMagicSystem();

    }

    void UpdateMeleeSystem()
    {
        EquipUnequipWeapon();
        WeaponAttackLogic();
        if (Input.GetKeyDown("q") && !isEquipped)
        {
            weaponIndex++;
            if (weaponIndex >= weapons.Length)
            {
                weaponIndex = 0;
            }
            Destroy(weapon);
            GameObject temp = Instantiate(weapons[weaponIndex], weaponControls.transform.position, weaponControls.transform.rotation * weaponRotation, weaponControls.transform);

            // Fixes an error swapping weapons while game is running
            MeleeDamage meleeLogic = temp.GetComponentInChildren<MeleeDamage>();
            meleeLogic.FindAfterStart();


            weapon = null;
            weapon = temp;
            weaponLogic.UpdateChild(weapon);
        }
    }

    void ShowMeleeVisuals()
    {
        weapon.SetActive(true);
    }

    void HideMeleeVisuals()
    {
        weapon.SetActive(false);
    }


    void UpdateMagicSystem()
    {
        UpdateCooldown();

        if (Input.GetKeyDown("f") && FC == 0f)
        {
            StartFireball();
            FC = FireballCooldown;
        }
        if (Input.GetKeyDown("r") && WC == 0f)
        {
            StartCoroutine(StartWindBlades());
            WC = WindBladesCooldown;
        }
        if (Input.GetKeyDown("t") && IC == 0f)
        {
            StartIceSpikes();
            IC = IceSpikesCooldown;
        }
        if (Input.GetKeyDown("e") && LC == 0f)
        {
            StartLightning();
            LC = LightningCooldown;
        }
        if (Input.GetKeyDown("g") && LBC == 0f)
        {
            StartCoroutine(StartBeam());
            LBC = LightBeamCooldown;
        }
        if (Input.GetKeyDown("v") && VC == 0f)
        {
            StartVoidBeam();
            VC = VoidBeamCooldown;
        }
        if (Input.GetKeyDown("b") && MC == 0f)
        {
            CreateMinions();
        }
    }



    void UpdateCooldown()
    {
        if (FC > 0f)
        {
            FC = FC - Time.deltaTime;
        }
        else FC = 0f;
        if (WC > 0f)
        {
            WC = WC - Time.deltaTime;
        }
        else WC = 0f;
        if (IC > 0f)
        {
            IC = IC - Time.deltaTime;
        }
        else IC = 0f;
        if (LC > 0f)
        {
            LC = LC - Time.deltaTime;
        }
        else LC = 0f;
        if (LBC > 0f)
        {
            LBC = LBC - Time.deltaTime;
        }
        else LBC = 0f;
        if (VC > 0f) {
            VC = VC - Time.deltaTime;
        }
        else VC = 0f;
        if (MC > 0f) {
            MC = MC - Time.deltaTime;
        }
        else MC = 0f;
    }
    public void MeleeAttackOff()
    {
        isMeleeAttack = false;
    }
    void EquipUnequipWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !weaponLogic.isSlashing)
        {
            if (!isEquipped)
            {
                if (weapon.tag == "Sword")
                {
                    weapon.transform.localPosition = new Vector3(1.1f, 0.44f, 0.62f);
                    weapon.transform.localRotation = activeWeaponRotation;
                    isEquipped = true;
                }
                else if (weapon.tag == "Spear")
                {
                    weapon.transform.localPosition = new Vector3(1.1f, 0.44f, 0.62f);
                    weapon.transform.localRotation = Quaternion.Euler(0f, -90.7f, -55f);
                    isEquipped = true;
                }
                else if (weapon.tag == "Axe")
                {
                    weapon.transform.localPosition = new Vector3(1.1f, 0.11f, 0.45f);
                    weapon.transform.localRotation = activeWeaponRotation * Quaternion.Euler(30f, 60f, 20f);
                    isEquipped = true;
                }
            }
            else if (isEquipped)
            {
                weapon.transform.localPosition = weaponPlace;
                weapon.transform.localRotation = weaponRotation;
                isEquipped = false;
            }
        }
    }
    //Toggle, untoggle between basic attack and throw skill
    void WeaponAttackLogic()
    {
        if (Input.GetMouseButtonDown(0) && isEquipped && !isMeleeAttack)
        {
            if (weapon.tag == "Sword")
            {
                //weaponLogic.StartThreeSixtySlashing();
                //weaponLogic.StartNormalSlashing();
                weaponLogic.StartSwingThrow();
            }
            else if (weapon.tag == "Spear")
            {
                weaponLogic.StartSpearThrust();
                //weaponLogic.StartSwingThrow();
            }
            if (weapon.tag == "Axe")
            {
                //weaponLogic.StartThreeSixtySlashing();
                //weaponLogic.StartAxeSlashing();
                weaponLogic.StartSwingThrow();
            }
            isMeleeAttack = true;
        }
    }
    public GameObject GetTalentSystem() {
        return talentUI;
    }
    public PlayerStatsManager GetStatSystem() {
        return statsHandler;
    }
    void StartFireball()
    {
        GameObject o = Instantiate(obj[0], ModelRoot.transform.position + ModelRoot.transform.forward * 3f + new Vector3(0f, 3f, 0f), this.transform.rotation);
        o.GetComponent<Rigidbody>().useGravity = false;
    }
    IEnumerator StartWindBlades()
    {
        for(int i = 0; i < 3; i++)
        {
            Quaternion rotate = Quaternion.Euler(Random.Range(-45f, 45f), this.transform.rotation.y + 90f, Random.Range(-45f, 45f));
            GameObject o = Instantiate(obj[1], ModelRoot.transform.position + ModelRoot.transform.forward * 3f + new Vector3(0f, 3f, 0f), rotate);
            o.GetComponent<Rigidbody>().useGravity = false;
            yield return new WaitForSeconds(0.2f);
        }
    }
    void StartIceSpikes() {
        for(int i = -2; i < 3; i++) {
            GameObject o = Instantiate(obj[2], ModelRoot.transform.position + ModelRoot.transform.forward * 3f + new Vector3(0f, 3f, 0f) + ModelRoot.transform.right * i * 2, this.transform.rotation);
            o.GetComponent<Rigidbody>().useGravity = false;
        }
    }
    void StartLightning() {
        GameObject o = Instantiate(obj[3], ModelRoot.transform.position + ModelRoot.transform.forward * LightningDistance + new Vector3(0f, LightningHeight, 0f), this.transform.rotation);
        o.GetComponent<LightningCloud>().LightningPosition = ModelRoot.transform.position + ModelRoot.transform.forward * LightningDistance + new Vector3(0f, LightningHeight * 0.5f, 0f);
    }
    public void CreateLightning(Vector3 LightningPosition) {
        GameObject o = Instantiate(obj[4], LightningPosition, this.transform.rotation);
        o.transform.localScale = new Vector3(o.transform.localScale.x, LightningHeight, o.transform.localScale.z);
        for(int i = 0; i < 2; i++) {
            GameObject o1 = Instantiate(obj[4], LightningPosition + new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f)), this.transform.rotation);
            o1.transform.localScale = new Vector3(o.transform.localScale.x, LightningHeight, o.transform.localScale.z);
        }
    }
    IEnumerator StartBeam()
    {
        Vector3 v = ModelRoot.transform.position + ModelRoot.transform.forward * BeamDistance;
        for (int i = 1; i < 10; i++)
        {
            GameObject o = Instantiate(obj[5], v + new Vector3(0f, 8f * i, 0f), this.transform.rotation);
            BeamCircle c = o.GetComponent<BeamCircle>();
            switch (i)
            {
                case 1: c.Size = 28;
                    c.Lifespan = 5.6f;
                    break;
                case 2: c.Size = 30;
                    c.Lifespan = 5.3f;
                    break;
                case 3: c.Size = 32;
                    c.Lifespan = 5.0f;
                    break;
                case 4: c.Size = 33;
                    c.Lifespan = 4.7f;
                    break;
                case 5: c.Size = 34;
                    c.Lifespan = 4.4f;
                    break;
                case 6: c.Size = 33;
                    c.Lifespan = 4.1f;
                    break;
                case 7: c.Size = 32;
                    c.Lifespan = 3.8f;
                    break;
                case 8: c.Size = 30;
                    c.Lifespan = 3.5f;
                    break;
                case 9: c.Size = 28;
                    c.Lifespan = 3.2f;
                    c.CreateBeam = true;
                    break;
                default: break;
            }
            yield return new WaitForSeconds(0.3f);
        }
        for(int i = 10; i < 30; i++)
        {
            GameObject o = Instantiate(obj[5], v + new Vector3(0f, 8f * i, 0f), this.transform.rotation);
            BeamCircle c = o.GetComponent<BeamCircle>();
            if (i % 2 == 0) c.Size = 28;
            else c.Size = 30;
            c.Lifespan = 2.9f;
        }
    }
    public void CreateBeam(Vector3 position)
    {
        GameObject o = Instantiate(obj[6], position + new Vector3(0f, -28f, 0f), this.transform.rotation);
    }
    void StartVoidBeam() {
        GameObject o = Instantiate(obj[7], ModelRoot.transform.position + ModelRoot.transform.forward * 3f + new Vector3(0f, 3f, 0f), ModelRoot.transform.rotation);
    }
    void CreateMinions() {
        int indexOfNewMinion = -1;
        for(int i = 0; i < minions.Length; i++) {
            if(indexOfNewMinion == -1 && minions[i] == null) {
                indexOfNewMinion = i;
            }
        }
        if(indexOfNewMinion > -1) {
            Vector3 placement = ModelRoot.transform.position + ModelRoot.transform.forward + Vector3.Slerp(-ModelRoot.transform.right * minionDistance, ModelRoot.transform.right * minionDistance, indexOfNewMinion * 0.51f) + new Vector3(0f, (Mathf.Pow(indexOfNewMinion-1, 2) * -0.5f + 1) * minionDistance, 0f);
            GameObject o = Instantiate(obj[8], placement, new Quaternion());
            GameObject o1 = Instantiate(obj[10], placement, new Quaternion(), ModelRoot.transform);
            o.GetComponent<TurretMinion>().Origin = o1;
            minions[indexOfNewMinion] = o;
            minionCycle++;
            if(minionCycle >= 3) {
                MC = MinionCooldown;
                minionCycle = 0;
            }
        }
    }
    public void CreateMinionBullets(Vector3 position, Vector3 velocity) {
        GameObject o = Instantiate(obj[9], position, new Quaternion());
        o.GetComponent<Rigidbody>().velocity = velocity;
    }
}
