using UnityEngine;
using UnityEngine.UI;

namespace Battlerock
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Player : Character
    {
        public PlayerManager.PlayerNumber PlayerNumber = PlayerManager.PlayerNumber.One;

        private string saveData;

        public Slider healthbarSlider;
        private GameObject m_healthbar;

        public TMPro.TextMeshProUGUI livesText;

        private Weapon m_weapon;

        public GameObject playerPrefab;

        void Start()
        {
            PlayerManager.Instance.players.Add(transform);
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

                var healthTransform = healthbarSlider.transform;
                m_healthbar = Instantiate(healthbarSlider.gameObject);
                m_healthbar.transform.position = new Vector3(healthTransform.position.x * ((int)PlayerNumber+1), healthTransform.position.y, healthTransform.position.z);
                m_healthbar.name = "PlayerHealth";

                m_healthbar.transform.SetParent(canvas.transform, false);
            }
            m_healthbar.SetActive(true);
            m_healthbar.GetComponent<Slider>().maxValue = this.stats.maxHealth;
            m_healthbar.GetComponent<Slider>().minValue = 0;

            livesText = m_healthbar.GetComponentInChildren<TMPro.TextMeshProUGUI>();

            livesText.text = stats.lives.ToString();
        }

        public void HealthBar()
        {
            if (m_healthbar)
            {
                m_healthbar.GetComponent<Slider>().value = this.stats.health;
            }
        }

        public override void Dead()
        {
            base.Dead();

            GameManager.Instance.GameOver(stats.lives <= 0);

            livesText.text = stats.lives.ToString();
        }

        protected internal virtual void OnEnable()
        {
            
            stats.health = stats.maxHealth;
            GameManager.Instance.IsGameOver = false;
            isDead = false;

            if (stats.lives <= 0)
            {
                stats.lives = stats.maxLives;
                livesText.text = stats.lives.ToString();
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
            }
        }

        private void Attack()
        {
            if (Input.GetButton(InputManager.ATTACK + "_" + PlayerNumber.ToString()) || Input.GetAxis(InputManager.ATTACK + "_" + PlayerNumber.ToString()) > 0)
            {
                m_weapon.FireProjectile();
            }
            else
            {
                var particleSystem = m_weapon.projectile.GetComponent<ParticleSystem>();

                if (particleSystem.GetEmissionRate() == 0)
                {
                    return;
                }

                m_weapon.isFiring = false;

                particleSystem.SetEmissionRate(0);
            }
        }

        private void Move()
        {
            Vector3 velocity = _rigidbody.velocity;

            if (Input.GetAxis(InputManager.VERTICAL_MOVEMENT + "_" + PlayerNumber.ToString()) == 0 && Input.GetAxis(InputManager.HORIZONTAL_MOVEMENT + "_" + PlayerNumber.ToString()) == 0)
            {
                velocity = Vector3.zero;
                anim.Reset();
                return;
            }
            else
            {
                anim.Play();                
            }

            velocity = new Vector3((Input.GetAxis(InputManager.HORIZONTAL_MOVEMENT + "_" + PlayerNumber.ToString()) * stats.speed * Time.deltaTime), velocity.y, (Input.GetAxis(InputManager.VERTICAL_MOVEMENT + "_" + PlayerNumber.ToString()) * stats.speed * Time.deltaTime));
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