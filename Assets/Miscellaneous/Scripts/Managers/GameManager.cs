using System.Collections;
using UnityEngine;

namespace Battlerock
{
    public class GameManager : Singleton<GameManager>
    {
        #region Public Variables

        /// <summary>
        /// Manages all the audio in the game.
        /// </summary>
        public GameObject audioController;

        public GameObject spawnPointController;

        public GameObject playerManager;

        /// <summary>
        /// A teleport point that the player spawns to whenever the inventory is active or a game over happens.
        /// </summary>
        public Transform teleportPoint;

        private bool m_isGameOver = false;

        public LEVEL currentLevel = LEVEL.MAINMENU;

        public LEVEL previousLevel = LEVEL.MAINMENU;

        #endregion

        #region Public Properties

        public bool IsGameOver
        {
            get { return m_isGameOver; }

            set
            {
                m_isGameOver = value;
            }
        }

        #endregion

        #region Private Variables


        private IEnumerator m_coroutine = null;

        #endregion

        #region Public Variables

        public GameObject lostLifeTextObject;
        public GameObject gameOverTextObject;

        #endregion

        #region Unity Methods

        /// <summary>
        /// Unity's built-in method (Called before anything else)
        /// </summary>
        private void Awake()
        {
            InitializeSubSystems();
        }

        // Unity's built-in update loop method
        private void Update()
        {
            if (WavesController.Instance != null)
            {
                if (WavesController.Instance.allWavesClearObject != null)
                {
                    if (WavesController.Instance.allWavesClearObject.activeSelf == true)
                    {
                        if (Input.GetKeyDown(KeyCode.R) == true)
                        {
                            WavesController.Instance.allWavesClearObject.SetActive(false);
                            PlayerManager.Instance.EnableOrDisableAllPlayers(true);
                            SpawnPointController.Instance.InitializeSpawnPoints();
                            SpawnPointController.Instance.StartCoroutine(SpawnPointController.Instance.TeleportWhenReady());
                        }
                    }
                }
            }

            if (m_isGameOver == true)
            {
                if (Input.GetKeyDown(KeyCode.R) == true)
                {
                    lostLifeTextObject.SetActive(false);
                    gameOverTextObject.SetActive(false);
                    PlayerManager.Instance.EnableOrDisableAllPlayers(true);
                    SpawnPointController.Instance.StartCoroutine(SpawnPointController.Instance.TeleportWhenReady());
                }
            }

            PauseOrResume();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Creates any missing sub-systems if they are NOT already in the scene.
        /// Then makes them a child of the _GameManager object.
        /// </summary>
        private void InitializeSubSystems()
        {
            if (SpawnPointController.Instance == null)
            {
                spawnPointController = new GameObject();
                spawnPointController.name = "_SpawnController_";
                SpawnPointController.Instance = spawnPointController.AddComponent<SpawnPointController>();
                spawnPointController.transform.parent = transform;
                spawnPointController.tag = "Spawner";
            }
            else
            {
                spawnPointController = GameObject.FindWithTag("Spawner");
                spawnPointController.transform.parent = transform;
            }

            if (PlayerManager.Instance == null)
            {
                playerManager = new GameObject();
                playerManager.name = "_PlayerManager_";
                PlayerManager.Instance = playerManager.AddComponent<PlayerManager>();
                playerManager.transform.parent = transform;
                playerManager.tag = "PlayerManager";
            }
            else
            {
                playerManager = GameObject.FindWithTag("PlayerManager");
                playerManager.transform.parent = transform;
            }
        }


        /// <summary>
        /// Toggle between pausing and resuming the game.
        /// </summary>
        private void PauseOrResume()
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true || Input.GetKeyDown(KeyCode.P) == true)
            {
                if (m_coroutine != null)
                {
                    StopCoroutine(m_coroutine);
                }

                if (AdjustTimeScale.isGamePaused == false)
                {
                    m_coroutine = AdjustTimeScale.ReduceTimeScale(AdjustTimeScale.INITIAL_VALUE, AdjustTimeScale.MINIMUM_TIME, AdjustTimeScale.TIME_MODIFIER);
                }
                else
                {
                    m_coroutine = AdjustTimeScale.RestoreTimeScale(AdjustTimeScale.TIME_MODIFIER);
                }

                StartCoroutine(m_coroutine);
            }
        }

        #endregion

        #region Public Methods

        public void GameOver(bool lostAllLives = false)
        {
            m_isGameOver = true;

            if (lostAllLives == true)
            {
                if (gameOverTextObject == null)
                {
                    Debug.LogWarningFormat("No game over text object assigned. Please assign in the inspector of the {0} class.", name);
                    return;
                }
                else
                {
                    gameOverTextObject.SetActive(true);
                }
            }
            else
            {
                if (lostLifeTextObject == null)
                {
                    Debug.LogWarningFormat("No life lost text object assigned. Please assign in the inspector of the {0} class.", name);
                    return;
                }
                else
                {
                    lostLifeTextObject.SetActive(true);
                }
            }
        }
        #endregion
    }
}