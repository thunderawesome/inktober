using UnityEngine;
using UnityEngine.UI;

namespace Battlerock
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Player : Character
    {
        public _PlayerManager.PlayerNumber PlayerNumber = _PlayerManager.PlayerNumber.One;

        private string saveData;

        public Slider healthbarSlider;
        private GameObject m_healthbar;

        private Weapon m_weapon;

        public GameObject playerPrefab;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            InitHealthBar();
        }

        private void InitHealthBar()
        {
            if (!m_healthbar)
            {
                var canvas = GameObject.FindWithTag("UICanvas");

                if (canvas == null)
                {
                    canvas = Instantiate(new GameObject());
                    var canvasComponent = canvas.AddComponent<Canvas>();
                    canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.tag = "UICanvas";
                    canvas.name = "Canvas";

                    var scalar = canvas.AddComponent<CanvasScaler>();
                    scalar.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

                    canvas.AddComponent<GraphicRaycaster>();
                }

                m_healthbar = Instantiate(healthbarSlider.gameObject);
                m_healthbar.name = "PlayerHealth";

                m_healthbar.transform.SetParent(canvas.transform, false);
            }
            m_healthbar.SetActive(true);
            m_healthbar.GetComponent<Slider>().maxValue = this.stats.maxHealth;
            m_healthbar.GetComponent<Slider>().minValue = 0;
        }

        public void HealthBar()
        {
            if (m_healthbar)
            {
                m_healthbar.GetComponent<Slider>().value = this.stats.health;
            }
        }

        protected internal virtual void OnEnable()
        {
            // See PlayerActions.cs for this setup.

            stats.health = stats.maxHealth;

            if (_PlayerManager.Instance != null)
            {
                _PlayerManager.Instance.cam.Follow = transform;
                _PlayerManager.Instance.cam.LookAt = transform;
            }

            m_weapon = GetComponentInChildren<Weapon>();
            m_weapon.projectile.GetComponent<ParticleSystem>().SetEmissionRate(0);

            InitHealthBar();
        }

        private void OnDestroy()
        {
            if (this.stats.health <= 0)
            {
                if (m_healthbar)
                {
                    m_healthbar.SetActive(false);
                }

                if (_PlayerManager.Instance)
                {
                    _PlayerManager.Instance.RevivePlayer(playerPrefab, _PlayerManager.Instance.spawnPoint.position, PlayerNumber);
                }
            }
        }

        private void Attack()
        {
            if (Input.GetButton(InputManager.ATTACK))
            {
                this.GetComponent<AudioSource>().enabled = true;
                m_weapon.FireProjectile();
            }
            else
            {
                var particleSystem = m_weapon.projectile.GetComponent<ParticleSystem>();

                if (particleSystem.GetEmissionRate() == 0)
                {
                    return;
                }

                this.GetComponent<AudioSource>().enabled = false;

                m_weapon.isFiring = false;

                particleSystem.SetEmissionRate(0);
            }
        }

        private void Move()
        {
            Vector3 velocity = _rigidbody.velocity;

            if (Input.GetAxis(InputManager.VERTICAL_MOVEMENT) == 0 && Input.GetAxis(InputManager.HORIZONTAL_MOVEMENT) == 0)
            {
                velocity = Vector3.zero;
                anim.Reset();
                return;
            }
            else
            {
                anim.Play();
            }

            velocity = new Vector3((Input.GetAxis(InputManager.HORIZONTAL_MOVEMENT) * stats.speed * Time.deltaTime), velocity.y, (Input.GetAxis(InputManager.VERTICAL_MOVEMENT) * stats.speed * Time.deltaTime));
            _rigidbody.velocity = velocity;
        }

        void FixedUpdate()
        {
            Move();
        }

        private void Update()
        {
            HealthBar();
            Attack();
        }
    }
}