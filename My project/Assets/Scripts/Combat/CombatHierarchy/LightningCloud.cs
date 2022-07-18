using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCloud : MonoBehaviour
{
    public Vector3 LightningPosition;
    GameObject player;
    private int iteration;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        StartCoroutine(Grow());
    }
    IEnumerator Grow() {
        while(iteration < 25) {
            this.transform.localScale = this.transform.localScale * 1.1f;
            iteration++;
            yield return new WaitForSeconds(0.1f);
        }
        player.GetComponent<CombatController>().CreateLightning(LightningPosition);
        yield return new WaitForSeconds(0.2f);
        player.GetComponent<CombatController>().CreateLightning(LightningPosition);
        yield return new WaitForSeconds(0.2f);
        player.GetComponent<CombatController>().CreateLightning(LightningPosition);
        yield return new WaitForSeconds(0.2f);
        player.GetComponent<CombatController>().CreateLightning(LightningPosition);
        Destroy(gameObject, 1);
    }
}
