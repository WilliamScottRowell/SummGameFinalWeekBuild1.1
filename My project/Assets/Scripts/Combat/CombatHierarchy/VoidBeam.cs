using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBeam : MonoBehaviour
{
    public int Size = 25;
    public float CoroutineDelay = 0.02f;
    GameObject player;
    void Start()
    {
        StartCoroutine(GrowAndCollapse());
    }
    IEnumerator GrowAndCollapse() {
        int t = 0;
        Vector3 startPosition = this.transform.localScale;
        while(t < Size) {
            t++;
            this.transform.localScale = new Vector3(this.transform.localScale.x * 1.1f, this.transform.localScale.y * 1.1f, -Mathf.Abs(this.transform.localScale.z * 1.2f));
            yield return new WaitForSeconds(CoroutineDelay);
        }
        Vector3 intermediatePosition = this.transform.localScale;
        while(t > 0) {
            t--;
            this.transform.localScale = this.transform.localScale + new Vector3(-0.8f, -0.8f, 1.6f);
            if(this.transform.localScale.x < 0.1) {
                break;
            }
            yield return new WaitForSeconds(CoroutineDelay);
        }
        Destroy(gameObject);
    }
}
