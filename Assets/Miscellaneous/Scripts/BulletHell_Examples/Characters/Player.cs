using UnityEngine;
using UnityEngine.UI;

namespace Battlerock
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Player : Character
    {
        #region Public Variables

        public PlayerManager.PlayerNumber PlayerNumber = PlayerManager.PlayerNumber.One;

        [HideInInspector]
        public Slider healthbarSlider;

        [HideInInspector]
        public TMPro.TextMeshProUGUI livesText;

        public GameObject playerPrefab;

        /// <summary>
        /// Determines what is considered floor or ground to the character.
        /// </summary>
        public LayerMask floorLayers;

        /// <summary>
        /// Timer for managing how high the character will jump depending on how long the jump button is held (Capped by a MAX held down value)
        /// </summary>
        public float m_shortHopTimer = 0;

        #endregion

        #region Private Variables

        /// <summary>
        /// Collider component attached to gameobject.
        /// </summary>
        private Collider m_collider;

        /// <summary>
        /// Velocity of character. Affects movement both horizontally and vertically.
        /// </summary>
        private Vector3 m_velocity = Vector3.zero;

        private GameObject m_healthbar;
        private Weapon m_weapon;

        /// <summary>
        /// Check to see if the character has already double jumped or not.
        /// </summary>
        private bool m_canDoubleJump = false;

        /// <summary>
        /// Simple check to determine if the character is currently on the ground or in the air.
        /// </summary>
        [SerializeField]
        private bool m_isOnGround = true;

        /// <summary>
        /// Determines where to start the raycast/linecast for the ground check.
        /// </summary>
        private GameObject m_groundCheckStartObj;

        /// <summary>
        /// Determines where the raycast/linecast will end for the ground check.
        /// </summary>
        private GameObject m_groundCheckEndObj;

        /// <summary>
        /// Gizmo starting position
        /// </summary>
        private Vector3 m_gizmoStartPos;

        /// <summary>
        /// Gizmo ending position
        /// </summary>
        private Vector3 m_gizmoEndPos;

        /// <summary>
        /// Gizmos use this to check for the bottom-most part of the character's collider and where to draw ground check gizmos.
        /// </summary>
        private const float GROUND_CHECK_OFFSET = 0.45f;

        #endregion

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            InitializeComponents();
            InitializeGroundCheck();
            InitializeGizmos();
        }

        void Start()
        {
            PlayerManager.Instance.players.Add(transform);
            InitHealthBar();
        }

        /// <summary>
        /// Unity's built-in gizmo drawing method
        /// </summary>
        private void OnDrawGizmos()
        {
            if (m_groundCheckEndObj == null || m_groundCheckStartObj == null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(m_groundCheckStartObj.transform.position, GROUND_CHECK_OFFSET);


            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_groundCheckEndObj.transform.position, GROUND_CHECK_OFFSET);
        }

        /// <summary>
        /// Sets up required component references for the character.
        /// </summary>
        private void InitializeComponents()
        {
            m_collider = GetComponentInChildren<Collider>();
        }

        /// <summary>
        /// Sets up the starting position for the gizmos.
        /// </summary>
        private void InitializeGizmos()
        {
            m_gizmoStartPos = m_collider.bounds.ClosestPoint(Vector3.down) + new Vector3(0, GROUND_CHECK_OFFSET, 0);
        }

        /// <summary>
        /// Sets up the ground checking system.
        /// </summary>
        private void InitializeGroundCheck()
        {
            m_gizmoStartPos = m_collider.bounds.ClosestPoint(Vector3.down * 2) + new Vector3(0, GROUND_CHECK_OFFSET, 0);
            m_gizmoEndPos = m_gizmoStartPos - new Vector3(0, GROUND_CHECK_OFFSET + GROUND_CHECK_OFFSET, 0);
            m_groundCheckStartObj = new GameObject("GroundCheck_START");
            m_groundCheckStartObj.transform.position = m_gizmoStartPos;
            m_groundCheckStartObj.transform.parent = transform;

            m_groundCheckEndObj = new GameObject("GroundCheck_END");
            m_groundCheckEndObj.transform.position = m_gizmoEndPos;
            m_groundCheckEndObj.transform.parent = transform;
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
                m_healthbar.transform.position = new Vector3(healthTransform.position.x * ((int)PlayerNumber + 1), healthTransform.position.y, healthTransform.position.z);
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

        private void FireWeapon()
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
            m_velocity = _rigidbody.velocity;

            if (Input.GetAxis(InputManager.VERTICAL_MOVEMENT + "_" + PlayerNumber.ToString()) == 0 && Input.GetAxis(InputManager.HORIZONTAL_MOVEMENT + "_" + PlayerNumber.ToString()) == 0)
            {
                m_velocity = Vector3.zero;
                anim.Reset();
                return;
            }
            else
            {
                anim.Play();
            }

            m_velocity = new Vector3(Input.GetAxis(InputManager.HORIZONTAL_MOVEMENT + "_" + PlayerNumber.ToString()) * stats.speed * Time.deltaTime, m_velocity.y, Input.GetAxis(InputManager.VERTICAL_MOVEMENT + "_" + PlayerNumber.ToString()) * stats.speed * Time.deltaTime);
            _rigidbody.velocity = m_velocity;

            Flip(m_velocity.x);
        }


        /// <summary>
        /// Determines which facing direction the character should be facing.
        /// </summary>
        /// <param name="inputValue"></param>
        private void Flip(float inputValue)
        {
            if (inputValue < 0)
            {
                transform.eulerAngles = new Vector3(0, 270, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 90, 0);
            }
        }

        #region Jump Methods

        /// <summary>
        /// Handles jump/in-air inputs.
        /// </summary>
        private void Jump()
        {
            IsOnGround();

            if (Input.GetButtonDown(InputManager.JUMP + "_" + PlayerNumber.ToString()) == true)
            {
                if (m_isOnGround == true)
                {
                    m_shortHopTimer = 0.15f;
                }
                else
                {
                    if (m_canDoubleJump == false)
                    {
                        m_canDoubleJump = true;
                        m_velocity.y = Mathf.Sqrt(stats.jumpForce * stats.doubleJumpForceModifier);
                        _rigidbody.velocity = m_velocity;
                    }
                }
            }

            if (m_isOnGround == false) return;
            if (m_shortHopTimer > 0)
            {
                //Short jump
                if (Input.GetButtonUp(InputManager.JUMP + "_" + PlayerNumber.ToString()))
                {
                    m_shortHopTimer = 0;
                    m_velocity.y = Mathf.Sqrt(stats.jumpForce);

                }
                else
                {// Normal jump
                    m_shortHopTimer -= Time.deltaTime;

                    if (m_shortHopTimer <= 0)
                    {
                        m_velocity.y = Mathf.Sqrt(2f * stats.jumpForce);
                    }
                }
                _rigidbody.velocity = m_velocity;
            }
        }

        /// <summary>
        /// A wrapper for determining if the character is on the ground or in the air.
        /// </summary>
        /// <returns>m_isOnGround: true/false</returns>
        private bool IsOnGround()
        {
            m_isOnGround = false;

            if (Physics.Linecast(m_groundCheckStartObj.transform.position, m_groundCheckEndObj.transform.position, floorLayers.value))
            {
                m_canDoubleJump = false;
                m_isOnGround = true;
            }
            return m_isOnGround;
        }
        #endregion

        void FixedUpdate()
        {
            Move();
            Jump();
        }

        private void Update()
        {
            HealthBar();
            if (m_weapon != null)
            {
                FireWeapon();
            }
        }
    }
}