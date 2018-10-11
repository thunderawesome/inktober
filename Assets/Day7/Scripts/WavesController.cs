using System;
using System.Collections;
using UnityEngine;

namespace Battlerock
{
    /// <summary>
    /// Manages all aspects of each Wave and its sub-systems.
    /// </summary>
    [Serializable]
    public class Waves
    {
        public float timeToWaitBeforeWaveStarts;
        public SubWaves[] subWaves;
        public bool isWaveCompleted;
    }

    /// <summary>
    /// A sub-system of Waves. Used to organize groupings of subsets that count towards overall Wave completion.
    /// </summary>
    [Serializable]
    public class SubWaves
    {
        public float timeToWaitBeforeWaveStarts;
        public GameObject[] enemies;
        public int currentNumberOfDefeatedEnemies;
        public int enemyCount;
        public bool isSubWaveCompleted;
    }

    /// <summary>
    /// 1.) Create an array of Waves that should be set up in the editor beforehand (not at runtime).
    /// 2.) Determine which wave is currently active.
    /// 3.) Make it easy to determine if all sub-waves (that belong to the currently active wave) are completed.
    /// 4.) Create the ability to reset all waves and sub-waves and start back at the first wave.
    /// 5.) Make it easy to progress to the next wave.
    /// 6.) Set up a method/property to determine if all waves are completed.
    /// </summary>
    public class WavesController : MonoBehaviour, IWavesController
    {
        #region Private Variables

        private int m_currentWaveIndex = 0;
        private int m_currentSubWaveIndex = 0;

        private IEnumerator m_coroutine;

        #endregion

        #region Public Variables

        public static WavesController Instance;
        public Waves[] waves;

        public GameObject waveCompleteObject;

        public GameObject allWavesClearObject;

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns the Current Wave.
        /// </summary>
        public Waves GetCurrentWave
        {
            get { return waves[m_currentWaveIndex]; }
        }

        /// <summary>
        /// Returns the Current Sub-Wave.
        /// </summary>
        public SubWaves GetCurrentSubWave
        {
            get { return GetCurrentWave.subWaves[m_currentSubWaveIndex]; }
        }

        /// <summary>
        /// Gets the Current Wave index.
        /// </summary>
        public int GetCurrentWaveIndex
        {
            get { return m_currentWaveIndex; }
        }

        /// <summary>
        /// Gets the Current Sub-Wave index.
        /// </summary>
        public int GetCurrentSubWaveIndex
        {
            get { return m_currentSubWaveIndex; }
        }

        /// <summary>
        /// Determines if all sub-waves for the Current Wave have been completed.
        /// </summary>
        public bool IsCurrentWaveCompleted
        {
            get
            {
                var subWaves = waves[m_currentWaveIndex].subWaves;
                for (int i = 0; i < subWaves.Length; i++)
                {
                    // If any of the sub-waves is incomplete then we assume tha the Current Wave is incomplete.
                    if (subWaves[i].isSubWaveCompleted == false)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Determines if all enemies for the Current Sub-Wave have been defeated.
        /// </summary>
        public bool IsCurrentSubWaveCompleted
        {
            get
            {
                if (GetCurrentSubWave.currentNumberOfDefeatedEnemies >= GetCurrentSubWave.enemyCount)
                {
                    GetCurrentSubWave.currentNumberOfDefeatedEnemies = 0;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Determines if all waves for the Current Level have been completed.
        /// </summary>
        public bool AreAllWavesCompleted
        {
            get
            {
                for (int i = 0; i < waves.Length; i++)
                {
                    // If any of the sub-waves is incomplete then we assume tha the Current Wave is incomplete.
                    if (waves[i].isWaveCompleted == false)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        #endregion

        #region Private Methods

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            // Reset all waves.
            ResetWaves();

            // Sets up Waves & Sub-Waves
            InitializeWavesAndSubWaves();

            // Start the waves.
            //yield return StartNextWaveAfterTimer();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space) == true)
            {
                GetCurrentSubWave.currentNumberOfDefeatedEnemies++;
                StartCoroutine(MoveToNextSubWaveIfPossible());
            }
#endif
        }

        private void InitializeWavesAndSubWaves()
        {
            // Find all child _Waves
            Transform[] waveTransforms = UtilityExtensions.GetComponentsOnlyInChildren<Transform>(transform);
            int numberOfWaves = waveTransforms.Length;

            int numberOfSubWaves = 0;
            int numberOfCharacters = 0;

            int totalNumberOfSubWaves = 0;
            int totalNumberOfCharacters = 0;

            // Set up array of _Waves
            waves = new Waves[numberOfWaves];
            for (int i = 0; i < numberOfWaves; i++)
            {
                // Create new _Wave instance for each _Wave that is found as a child of the this script's transform
                waves[i] = new Waves();
                //Debug.Log("<color=#a00000ff>Wave Name: " + waveTransforms[i].name + "</color>");

                // Find all child _SubWaves of the current _Wave being looped through
                Transform[] subWaveTransforms = UtilityExtensions.GetComponentsOnlyInChildren<Transform>(waveTransforms[i].transform);
                numberOfSubWaves = subWaveTransforms.Length;

                //Set up array of _SubWaves
                waves[i].subWaves = new SubWaves[numberOfSubWaves];
                for (int j = 0; j < numberOfSubWaves; j++)
                {
                    // Create new _SubWave instance for each _SubWave that is found as a child of the current _Wave transform
                    waves[i].subWaves[j] = new SubWaves();
                    //Debug.Log("<color=darkblue>Sub-Wave Name: " + subWaveTransforms[j].name + "</color>");

                    Transform[] characters = UtilityExtensions.GetComponentsOnlyInChildren<Transform>(subWaveTransforms[j].transform);
                    numberOfCharacters = characters.Length;

                    waves[i].subWaves[j].enemyCount = numberOfCharacters;


                    waves[i].subWaves[j].enemies = new GameObject[numberOfCharacters];
                    for (int k = 0; k < numberOfCharacters; k++)
                    {  
                        waves[i].subWaves[j].enemies[k] = characters[k].gameObject;

                        characters[k].gameObject.SetActive(false);
                        //Debug.Log("<color=green>Character Name: " + characters[k].name + "</color>");
                        totalNumberOfCharacters++;
                    }

                    totalNumberOfSubWaves++;
                }
            }

            StartCoroutine(StartNextWaveAfterTimer());

            //Debug.Log("<color=red><b>Waves: " + numberOfWaves + "</b></color>");
            //Debug.Log("<color=lime><b>Sub-Waves: " + totalNumberOfSubWaves + "</b></color>");
            //Debug.Log("<color=lightblue><b>Characters: " + totalNumberOfCharacters + "</b></color>");
        }

        /// <summary>
        /// Starts the next Wave after the timer is done counting down.
        /// </summary>
        /// <returns>IEnumerator (usually a Coroutine)</returns>
        public IEnumerator StartNextWaveAfterTimer()
        {
            // Set up the custom Timer coroutine for the Wave
            m_coroutine = Timer(GetCurrentWave.timeToWaitBeforeWaveStarts);

            // Start the timer for the next wave.
            yield return StartCoroutine(m_coroutine);

            // Set up the custom Timer coroutine for the Sub-Wave
            m_coroutine = Timer(GetCurrentSubWave.timeToWaitBeforeWaveStarts);

            // Start the timer for the sub-waves.
            yield return StartCoroutine(m_coroutine);

            // After the Current Wave's timer has been used once we should set it to ZERO.
            GetCurrentWave.timeToWaitBeforeWaveStarts = 0;

            EnableEnemiesInSubWave();
        }

        /// <summary>
        /// Starts the next Sub-Wave after the timer is done counting down.
        /// </summary>
        /// <returns>IEnumerator (usually a Coroutine)</returns>
        private IEnumerator StartNextSubWaveAfterTimer()
        {
            // Set up the custom Timer coroutine for the Sub-Wave
            m_coroutine = Timer(GetCurrentSubWave.timeToWaitBeforeWaveStarts);

            yield return StartCoroutine(m_coroutine);

            EnableEnemiesInSubWave();
        }

        /// <summary>
        /// Marks all Waves and Sub-Waves as incomplete and starts over at the First Wave.
        /// </summary>
        private void ResetWaves()
        {
            // Resets the wave index back to the first wave
            m_currentWaveIndex = 0;

            for (int i = 0; i < waves.Length; i++)
            {
                for (int j = 0; j < waves[i].subWaves.Length; j++)
                {
                    // Resets all sub-waves back to incomplete
                    waves[i].subWaves[j].isSubWaveCompleted = false;
                }
                // Resets all waves in level back to incomplete
                waves[i].isWaveCompleted = false;
            }
        }

        /// <summary>
        /// Enables enemies for the current wave/sub-wave when the timer finishes counting down.
        /// </summary>
        private void EnableEnemiesInSubWave()
        {
            if (AreAllWavesCompleted == true) return;

            for (int i = 0; i < GetCurrentSubWave.enemies.Length; i++)
            {
                if (GetCurrentSubWave.enemies[i] != null)
                {
                    GetCurrentSubWave.enemies[i].SetActive(true);
                }
            }
        }

        /// <summary>
        /// Timer that is used in-between waves and sub-waves for managing spawn times of enemies.
        /// </summary>
        /// <param name="value">Time in seconds.</param>
        /// <returns></returns>
        private IEnumerator Timer(float value)
        {
            // Uses the Wave timer and Sub-Wave timers as values
            // to manage spawn timing for enemies in each wave.
            yield return new WaitForSeconds(value);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Transition to the next wave if all sub-waves are completed first.
        /// </summary>
        public IEnumerator MoveToNextWaveIfPossible()
        {
            // If the current wave is incomplete, then we won't bother continuing
            if (IsCurrentWaveCompleted == false)
            {                
                yield break;
            }

            // Mark the current wave as completed
            GetCurrentWave.isWaveCompleted = true;
            Debug.Log("Current Wave " + m_currentWaveIndex + ": COMPLETE!");

            // Reset the sub-wave index when a new Wave starts
            m_currentSubWaveIndex = 0;

            // If we do NOT exceed the number of waves in a level
            if (m_currentWaveIndex < waves.Length - 1)
            {
                m_currentWaveIndex++;
            }

            Debug.Log("Current Wave Index: " + m_currentWaveIndex);

            if (AreAllWavesCompleted == true)
            {
                // If all waves are marked as complete, then we trigger the results screen
                yield return AllWavesClear();
            }
            else
            {

                // Otherwise, we move to the next wave.
                yield return StartNextWaveAfterTimer();

            }
        }

        /// <summary>
        /// Transition to the next sub-wave if all enemies are defeated first.
        /// </summary>
        public IEnumerator MoveToNextSubWaveIfPossible()
        {
            // If the current sub-wave is incomplete, then we won't bother continuing
            if (IsCurrentSubWaveCompleted == false)
            {                
                yield break;
            }

            waveCompleteObject.SetActive(false);

            var currentSubWave = GetCurrentSubWave;

            // Mark the current sub-wave as completed
            currentSubWave.isSubWaveCompleted = true;
            Debug.Log("Current Sub-Wave " + GetCurrentSubWaveIndex + ": COMPLETE!");

            for (int i = 0; i < currentSubWave.enemies.Length; i++)
            {
                currentSubWave.enemies[i].SetActive(false);
            }

            waveCompleteObject.SetActive(true);

            // If we do NOT exceed the number of sub-waves that belong to the current wave
            if (m_currentSubWaveIndex < GetCurrentWave.subWaves.Length - 1)
            {
                m_currentSubWaveIndex++;
            }

            if (IsCurrentWaveCompleted == true)
            {
                if (AreAllWavesCompleted == true)
                {
                    yield return AllWavesClear();
                }
                else
                {
                    yield return MoveToNextWaveIfPossible();
                }
            }

            // If all sub-waves are marked as complete, then we will move to the next wave
            // Starts the next Sub-Wave after the timer is done counting down.
            yield return StartNextSubWaveAfterTimer();
        }

        /// <summary>
        /// Toggle the results screen or cinematic that plays after clearing all waves in a single level.
        /// </summary>
        public IEnumerator AllWavesClear()
        {
            for (int i = 0; i < waves.Length; i++)
            {
                // If any of the waves are marked as FALSE then that means all waves have not been cleared.
                if (waves[i].isWaveCompleted == false)
                {
                    // Starts the next wave after the timer is done counting down.
                    yield return StartNextWaveAfterTimer();
                }
            }

            allWavesClearObject.SetActive(true);
            // TODO: Add winning condition
            Debug.Log("ALL WAVES CLEARED!");
        }

        #endregion
    }
}