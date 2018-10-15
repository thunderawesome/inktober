using UnityEngine;

namespace Battlerock
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(SphereCollider))]
    public class Enemy : Character
    {
        public Transform target;
        public Vector3 offset;

        protected float attackTimer = 2;

        [SerializeField]
        protected int damageWhenBumpedInto = 5;

        protected void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Waypoint")
            {
                target = other.GetComponent<Waypoint>().nextWaypoint;
                Flip();
            }

            if (other.tag == "Player")
            {
                target = other.transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.tag == "Player")
            {
                target = null;
            }
        }

        protected void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Player>().TakeDamage(damageWhenBumpedInto);
            }
        }

        protected void Move()
        {
            if (target)
            {
                Vector3 relativePos = target.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                transform.rotation = rotation;

                Vector3 velocity = _rigidbody.velocity;
                velocity = transform.forward * stats.speed * Time.deltaTime;
                _rigidbody.velocity = velocity;
            }
        }

        /// <summary>
        /// Determines which facing direction the character should be facing.
        /// </summary>
        private void Flip()
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        protected void FixedUpdate()
        {
            Move();
        }

        public override void Dead()
        {
            base.Dead();
            if (WavesController.Instance != null)
            {
                WavesController.Instance.GetCurrentSubWave.currentNumberOfDefeatedEnemies++;
                WavesController.Instance.StartCoroutine(WavesController.Instance.MoveToNextSubWaveIfPossible());
            }
        }
    }
}