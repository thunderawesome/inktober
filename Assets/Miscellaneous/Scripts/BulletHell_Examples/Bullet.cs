using System.Collections.Generic;
using UnityEngine;

namespace Battlerock
{

    public class Bullet : MonoBehaviour
    {
        public ParticleSystem part;
        public List<ParticleCollisionEvent> collisionEvents;

        public int damage = 1;

        void Start()
        {
            part = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();
            var collision = part.collision;
            collision.enabled = true;
            collision.bounce = .5f;
        }

        void OnParticleCollision(GameObject other)
        {
            int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(part, other, collisionEvents);
            int i = 0;
            while (i < numCollisionEvents)
            {
                if (other.gameObject.GetComponent<Character>() != null)
                {
                    other.gameObject.GetComponent<Character>().TakeDamage(damage);
                }
                i++;
            }
        }

    }
}