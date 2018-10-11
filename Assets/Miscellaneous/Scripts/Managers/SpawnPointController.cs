using Battlerock;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages all the spawnpoints in the current level.
/// </summary>
public class SpawnPointController : MonoBehaviour
{
    #region Public Variables

    /// <summary>
    /// An easily accessible static instance of this class.
    /// </summary>
    public static SpawnPointController Instance;

    public Transform[] PlayerSpawnPoints
    {
        get { return m_playerSpawnPoints.ToArray(); }
    }

    #endregion

    #region Public Properties

    public Transform CurrentSpawnPoint
    {
        get
        {
            if (m_currentSpawnPoint != null)
            {
                return m_currentSpawnPoint;
            }
            else
            {
                InitializeSpawnPoints();
                return m_currentSpawnPoint;
            }
        }
        set { m_currentSpawnPoint = value; }
    }

    #endregion

    #region Private Variables

    /// <summary>
    /// The container transform that holds all the spawnpoints in a scene.
    /// </summary>
    private Transform m_playerSpawnPointContainer;

    /// <summary>
    /// A collection of spawnpoints that will be cycled through as the player progresses.
    /// </summary>
    private Queue<Transform> m_playerSpawnPoints;

    /// <summary>
    /// The current spawn point that the player is at.
    /// </summary>
    private Transform m_currentSpawnPoint;

    #endregion

    #region Unity Methods
    /// <summary>
    /// Unity's built-in method (Called before anything else)
    /// </summary>
    private void Awake()
    {
        Instance = this;
        //InitializeSpawnPoints();
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.LogFormat("<color=green>OnSceneLoaded: {0} && Mode: {1}</color>", scene.name, mode);
        if (GameManager.Instance.IsGameOver == false)
        {
            InitializeSpawnPoints();
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Sets up all the spawnpoints in the scene.
    /// </summary>
    public void InitializeSpawnPoints()
    {
        // Set up a queue of Transforms
        m_playerSpawnPoints = new Queue<Transform>();
        m_playerSpawnPoints.Clear();

        StartCoroutine(TeleportWhenReady());
    }

    /// <summary>
    /// Trigger teleportation through an EventListener
    /// </summary>
    /// <param name="OnCompleted">Handles any cases when this succeeds/fails</param>
    private void TriggerTeleportationToNewSpawnPoint(System.Action OnCompleted)
    {
        OnCompleted();
    }

    /// <summary>
    /// Teleports the player to the next spawnpoint (after a fraction of a second)
    /// </summary>
    /// <returns></returns>
    public IEnumerator TeleportWhenReady(bool canMoveToNextWave = true)
    {
        while (GameManager.Instance.spawnPointController == null)
        {
            yield return null;
        }

        if (GameManager.Instance.IsGameOver == true)
        {
            // Look in the scene for the spawn point container transform
            m_playerSpawnPointContainer = GameManager.Instance.spawnPointController.transform;
            if (m_playerSpawnPointContainer == null)
            {
                Debug.LogWarning("No Player Spawn Point Container assigned in the inspector. Make sure it has all the player spawn points as child objects to it.");
                yield return null;
            }
            else
            {
                foreach (Transform child in m_playerSpawnPointContainer)
                {
                    if (child != m_playerSpawnPointContainer)
                    {
                        m_playerSpawnPoints.Enqueue(child);
                    }
                }

                m_currentSpawnPoint = m_playerSpawnPoints.Dequeue();
            }

            TriggerTeleportationToNewSpawnPoint
                (() =>
                {
                    Debug.Log("teleport trigger stuff happening");

                    Debug.Log("CurrentSpawnPoint: " + m_currentSpawnPoint.name);
                    GameManager.Instance.Player.transform.position = m_currentSpawnPoint.position;
                    GameManager.Instance.Player.transform.rotation = m_currentSpawnPoint.rotation;

                    GameManager.Instance.IsGameOver = false;

                });
            yield break;
        }

        while (m_playerSpawnPointContainer == null || m_playerSpawnPoints == null || m_playerSpawnPoints.Count == 0)
        {
            // Look in the scene for the spawn point container transform
            m_playerSpawnPointContainer = GameManager.Instance.spawnPointController.transform;
            if (m_playerSpawnPointContainer == null)
            {
                Debug.LogWarning("No Player Spawn Point Container assigned in the inspector. Make sure it has all the player spawn points as child objects to it.");
                yield return null;
            }
            else
            {
                if(m_playerSpawnPointContainer.childCount == 0)
                {
                    m_playerSpawnPoints.Enqueue(GameManager.Instance.Player);
                }
                else
                {
                    foreach (Transform child in m_playerSpawnPointContainer)
                    {
                        if (child != m_playerSpawnPointContainer)
                        {
                            m_playerSpawnPoints.Enqueue(child);
                        }
                    }
                }               

                m_currentSpawnPoint = m_playerSpawnPoints.Dequeue();
                yield break;
            }
            yield return null;
        }

        TriggerTeleportationToNewSpawnPoint
            (() =>
            {
                Debug.Log("entered trigger teleport");
                //If there are no waves in the scene then don't even bother moving ahead.
                if (WavesController.Instance == null) return;
                Debug.Log("teleport trigger stuff happening");
                // Don't bother running this method if the level is cleared OR if there are no more spawn points to teleport to.
                if (WavesController.Instance.AreAllWavesCompleted == true || m_playerSpawnPoints.Count <= 0)
                {
                    Debug.Log("exit out of teleportation");
                    return;
                }
                m_currentSpawnPoint = m_playerSpawnPoints.Dequeue();
                Debug.Log("CurrentSpawnPoint: " + m_currentSpawnPoint.name);
                
                GameManager.Instance.Player.transform.position = m_currentSpawnPoint.position;
                GameManager.Instance.Player.transform.rotation = m_currentSpawnPoint.rotation;
            });
    }
    #endregion
}