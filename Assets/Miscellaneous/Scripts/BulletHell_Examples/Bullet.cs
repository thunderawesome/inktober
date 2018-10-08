using UnityEngine;

namespace Battlerock
{

    public class Bullet : MonoBehaviour
    {
        public ParticleSystem part;

        public int damage = 1;

        void Start()
        {
            part = GetComponent<ParticleSystem>();

        }

        void OnParticleCollision(GameObject other)
        {
            if (other.GetComponent<Character>() != null)
            {
                other.GetComponent<Character>().TakeDamage(damage);
            }
        }
    }
}