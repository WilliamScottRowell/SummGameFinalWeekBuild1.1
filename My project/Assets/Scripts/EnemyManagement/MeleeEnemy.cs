using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CMF
{
    public class MeleeEnemy : Enemies
    {

        public Transform playerTransform;
        public Transform bladeTransform;
        public float attackRange = 3;

        // Start is called before the first frame update
        void Start()
        {
            playerTransform = GameObject.Find("Player").transform;

        }
        private void Update()
        {
            if (transform.GetComponent<EnemyMovement>() && !transform.GetComponentInChildren<TurretEnemy>())
            {
                transform.GetComponent<EnemyMovement>().stopDistance = 2f;

            }

            if (Mathf.Abs((playerTransform.position - transform.position).magnitude) < attackRange)
            {
                StartCoroutine(SlashCoroutine());
            }

        }

        public float slashRate = 4;
        [SerializeField]
        public bool isSlashing = false;
        IEnumerator SlashCoroutine()
        {
            isSlashing = true;
            Quaternion startRotation = bladeTransform.localRotation;
            float endZRot = 270f;
            float duration = 1f;
            float t = 0;
            float time;
            while (t < 1f)
            {
                //Debug.Log("slashing. " + transform.localRotation.eulerAngles);
                t += Time.deltaTime * slashRate;
                time = Mathf.Min(1f, t + Time.deltaTime / duration);
                Vector3 newEulerOffset = new Vector3(0, 1, 0) * (endZRot * t);
                // global z rotation
                //transform.localRotation = Quaternion.Euler(newEulerOffset) * startRotation;
                // local z rotation
                bladeTransform.localRotation = startRotation * Quaternion.Euler(newEulerOffset);
                yield return null;
            }

            endZRot = 0f;
            t = 0;
            while (t < 1f)
            {
                //Debug.Log("slashing. " + transform.localRotation.eulerAngles);
                t += Time.deltaTime * slashRate;
                time = Mathf.Min(1f, t + Time.deltaTime / duration);
                Vector3 newEulerOffset = new Vector3(0, 1, 0) * (endZRot * t);
                // global z rotation
                //transform.localRotation = Quaternion.Euler(newEulerOffset) * startRotation;
                // local z rotation
                bladeTransform.localRotation = startRotation * Quaternion.Euler(newEulerOffset);
                yield return null;
            }

            isSlashing = false;
        }



    }
}
