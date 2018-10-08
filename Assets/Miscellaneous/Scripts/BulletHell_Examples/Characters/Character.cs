using UnityEngine;

namespace Battlerock
{
    [RequireComponent(typeof(AudioSource))]
    public class Character : MonoBehaviour
    {
        public Stats stats;

        public AnimateCoroutineExample anim;
        protected Rigidbody _rigidbody;

        public AudioClip sfx;

        public GameObject explosion;

        public virtual void TakeDamage(int amount)
        {
            if (this.stats.health >= 1)
            {
                this.stats.health -= amount;
            }
            else
            {
                Dead();
            }
        }

        public virtual void Dead()
        {
            Instantiate(explosion, this.transform.position, explosion.transform.rotation);
            Destroy(this.gameObject, .1f);
        }
    }

    [System.Serializable]
    public class Stats
    {
        public float speed;
        public float rotateSpeed;
        public float jumpForce;
        public int maxHealth;
        public int health;
        public int lives;
        public int special;
    }
}