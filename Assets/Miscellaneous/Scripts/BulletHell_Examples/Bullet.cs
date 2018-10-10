using System.Collections.Generic;
using UnityEngine;

namespace Battlerock
{
    [RequireComponent(typeof(AudioSource))]
    public class Bullet : MonoBehaviour
    {
        public ParticleSystem part;
        public List<ParticleCollisionEvent> collisionEvents;

        public int damage = 1;

        private AudioSource m_audioSource;

        void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
            part = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();
            m_audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            m_audioSource.enabled = true;
        }

        private void OnDisable()
        {
            m_audioSource.enabled = false;
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