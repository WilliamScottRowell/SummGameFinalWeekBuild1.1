using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF {
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeReference]
        public int enemyLayer = 8;
        [SerializeReference]
        public float damage = 1;
        [SerializeReference]
        public Vector3 initialVelocity;
    }
}