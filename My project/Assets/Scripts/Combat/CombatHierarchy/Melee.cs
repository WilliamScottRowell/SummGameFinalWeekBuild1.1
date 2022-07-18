using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;

public class Melee : MonoBehaviour
{
    public float slashAngle = 270f;
    public Collider coll;
    public CombatController c;
    public float slashRate = 4;
    public bool isSlashing = false;
    Transform childControls, childChildControls;
    GameObject player, modelRoot;
    void Start()
    {
        childControls = transform.GetChild(0);
        childChildControls = transform.GetChild(0).GetChild(0);
        player = GameObject.FindWithTag("Player");
        modelRoot = GameObject.FindWithTag("ModelRoot");
    }
    public void UpdateChild(GameObject newWeapon) {
        childControls = null;
        childControls = newWeapon.transform;
        childChildControls = null;
        childChildControls = newWeapon.transform.GetChild(0);
    }
    public void StartThreeSixtySlashing()
    {
        StartCoroutine(ThreeSixtySlashCoroutine());
    }
    public void StartNormalSlashing()
    {
        StartCoroutine(NormalSlash());
    }
    public void StartSpearThrust() {
        StartCoroutine(SpearThrust());
    }
    public void StartAxeSlashing()
    {
        StartCoroutine(AxeSlash());
    }
    public void StartSwingThrow() {
        StartCoroutine(SwingThrow());
    }
    IEnumerator ThreeSixtySlashCoroutine()
    {
        isSlashing = true;
        Quaternion startRotation = transform.localRotation;
        float endZRot = 360f;
        float duration = 1f;
        float t = 0;
        float time;
        while (t < 1f)
        {
            t += Time.deltaTime * slashRate;
            //time = Mathf.Min(1f, t + Time.deltaTime / duration);
            //Debug.Log(time);
            Vector3 newEulerOffset = new Vector3(0, 1, 0) * (endZRot * t);
            // global z rotation
            // transform.localRotation = Quaternion.Euler(newEulerOffset) * startRotation;
            // local z rotation
            transform.localRotation = startRotation * Quaternion.Euler(newEulerOffset);
            yield return null;
        }
        transform.localRotation = startRotation;
        isSlashing = false;
        c.MeleeAttackOff();
    }
    IEnumerator NormalSlash()
    {
        isSlashing = true;
        Quaternion startRotation = childControls.localRotation;
        float endZRot = -60f;
        float duration = 1f;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * slashRate;
            Vector3 newEulerOffset = new Vector3(0, 0, 1) * (endZRot * t);
            childControls.localRotation = startRotation * Quaternion.Euler(newEulerOffset);
            yield return null;
        }
        childControls.localRotation = startRotation;
        isSlashing = false;
        c.MeleeAttackOff();
    }
    IEnumerator SpearThrust()
    {
        isSlashing = true;
        Quaternion startRotation = childControls.localRotation;
        Quaternion startChildRotation = childChildControls.localRotation;
        Vector3 startPosition = childControls.localPosition;
        float endYRot = 360f;
        float distanceInFront = 2f;
        float duration = 1f;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * slashRate;
            Vector3 newEulerOffset = new Vector3(0, 1, 0) * (endYRot * t);
            Vector3 newSpearOffset = new Vector3(0, 0, 1) * (distanceInFront * 2 * t);
            if (t > 0.5f) {
                newSpearOffset = new Vector3(0, 0, 1) * (distanceInFront) + new Vector3(0, 0, -1) * (distanceInFront * 2 * (t - 0.5f));
            }
            childChildControls.localRotation = startChildRotation * Quaternion.Euler(newEulerOffset);
            childControls.localPosition = startPosition + newSpearOffset;
            yield return null;
        }
        childControls.localRotation = startRotation;
        childChildControls.localRotation = startChildRotation;
        childControls.localPosition = startPosition;
        isSlashing = false;
        c.MeleeAttackOff();
    }
    IEnumerator AxeSlash()
    {
        isSlashing = true;
        Quaternion startRotation = childControls.localRotation;
        float endXRot = 60f;
        float duration = 1f;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * slashRate;
            Vector3 newEulerOffset = new Vector3(1, 0, 0) * (endXRot * 2 * t);
            if (t > 0.5f)
            {
                newEulerOffset = new Vector3(1, 0, 0) * (endXRot) + new Vector3(-1, 0, 0) * (endXRot * 2 * (t - 0.5f));
            }
            childControls.localRotation = startRotation * Quaternion.Euler(newEulerOffset);
            yield return null;
        }
        childControls.localRotation = startRotation;
        isSlashing = false;
        c.MeleeAttackOff();
    }
    IEnumerator SwingThrow()
    {
        isSlashing = true;
        Quaternion startRotation = childControls.localRotation;
        //Quaternion startChildRotation = childChildControls.localRotation;
        Vector3 startPosition = childControls.localPosition;
        Vector3 forward = modelRoot.transform.forward;
        childControls.parent = null;
        Quaternion startWorldRotation = childControls.rotation;
        Vector3 startWorldPosition = childControls.position;
        float endYRot = 360f;
        float distanceInFront = 25f;
        float t = 0;
        while (t < 10f)
        {
            t += Time.deltaTime;
            Vector3 newEulerOffset = new Vector3(0, 1, 0) * (endYRot * t);
            Vector3 newSpearOffset = forward * (distanceInFront * 2 * t);
            if (t > 1f) {
                newSpearOffset = (player.transform.position - childControls.position).normalized * 0.1f;
                childControls.position = childControls.position + newSpearOffset;
                if((childControls.position - player.transform.position).magnitude < 3f) {
                    break;
                }
            }
            else {
                childControls.position = startWorldPosition + newSpearOffset;
            }
            Debug.Log(player.transform.position - childControls.position);
            //childChildControls.localRotation = startChildRotation * Quaternion.Euler(newEulerOffset);
            childControls.rotation = startWorldRotation * Quaternion.Euler(newEulerOffset);
            yield return new WaitForSeconds(0.001f);
        }
        childControls.parent = this.transform;
        childControls.localRotation = startRotation;
        //childChildControls.localRotation = startChildRotation;
        childControls.localPosition = startPosition;
        isSlashing = false;
        c.MeleeAttackOff();
    }
}