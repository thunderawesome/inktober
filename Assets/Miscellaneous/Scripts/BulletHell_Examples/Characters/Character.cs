using UnityEngine;

namespace Battlerock
{
    public class Character : MonoBehaviour
    {
        public Stats stats;

        public AnimateCoroutineExample anim;
        protected Rigidbody _rigidbody;

        public AudioClip sfx;

        public GameObject explosion;

        public bool isDead = false;

        public virtual void TakeDamage(int amount)
        {
            if (this.stats.health >= 1)
            {
                this.stats.health -= amount;
            }
            else
            {
                if (this.stats.health <= 0)
                {
                    this.stats.health = 0;
                }

                // Seems backwards. But, we don't want to repeatedly call this.
                // isDead is set inside the Dead() method.
                if (isDead == false)
                {
                    Dead();
                }
            }
        }

        public virtual void Dead()
        {
            isDead = true;

            if (CameraShake.Instance != null)
            {
                CameraShake.Instance.Shake();
            }

            if (stats.lives > 0)
            {
                stats.lives--;
            }
            Instantiate(explosion, this.transform.position, explosion.transform.rotation);
            //Destroy(this.gameObject, .1f);
            this.gameObject.SetActive(false);
        }
    }

    [System.Serializable]
    public class Stats
    {
        public float speed;
        public float rotateSpeed;
        public float jumpForce;
        public float doubleJumpForceModifier;
        public int maxHealth;
        public int health;
        public int lives;
        public int maxLives;
        public int special;
    }
}