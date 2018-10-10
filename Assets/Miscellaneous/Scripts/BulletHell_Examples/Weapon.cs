using UnityEngine;

namespace Battlerock
{
    public class Weapon : MonoBehaviour
    {
        public bool hasProjectiles = true;

        public Transform projectileSpawnPoint;
        public Transform projectile;

        private RaycastHit[] m_raycastHits = new RaycastHit[1];
        public float sphereCastRadius = .5f;

        public LayerMask lookMask;

        private Transform target = null;

        public Transform focusPoint = null;

        [HideInInspector]
        public bool isFiring = false;

        public void FireProjectile()
        {
            if (isFiring == true)
            {
                transform.LookAt(target);
            }

            projectile.position = projectileSpawnPoint.position;
            projectile.rotation = projectileSpawnPoint.rotation;
            projectile.GetComponent<ParticleSystem>().SetEmissionRate(25);

            isFiring = true;
        }

        private void FixedUpdate()
        {
            if (isFiring == true)
            {
                int numberOfHits = Physics.SphereCastNonAlloc(transform.parent.position, sphereCastRadius, transform.parent.forward, m_raycastHits, Mathf.Infinity, lookMask);
                if (numberOfHits > 0)
                {
                    target = m_raycastHits[0].collider.transform;
                }
                else
                {
                    target = focusPoint;
                }

                if (projectile.GetComponent<Bullet>().enabled == false)
                {
                    projectile.GetComponent<Bullet>().enabled = true;
                }
            }
            else
            {
                target = focusPoint;

                if (projectile.GetComponent<Bullet>().enabled == true)
                {
                    projectile.GetComponent<Bullet>().enabled = false;
                }
            }
        }
    }
}