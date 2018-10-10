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

        protected void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                target = other.transform;
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

        protected void FixedUpdate()
        {
            Move();
        }

        public override void Dead()
        {
            base.Dead();
            WavesController.Instance.GetCurrentSubWave.currentNumberOfDefeatedEnemies++;
            WavesController.Instance.StartCoroutine(WavesController.Instance.MoveToNextSubWaveIfPossible());

        }
    }
}