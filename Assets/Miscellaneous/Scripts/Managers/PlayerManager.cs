using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

namespace Battlerock
{
    public class PlayerManager : MonoBehaviour
    {
        #region Public Enums

        public enum PlayerNumber
        {
            One,
            Two,
            Three,
            Four
        }

        #endregion

        #region Public Variables

        public CinemachineVirtualCamera cam;

        public static PlayerManager Instance;

        public bool canShareLives = false;

        public int lives = MAX_LIVES;

        public List<Transform> players = new List<Transform>();

        #endregion

        #region Private Variables

        private const int MAX_LIVES = 3;

        #endregion

        #region Unity Methods

        // Use this for initialization
        void Awake()
        {
            Instance = this;
        }

        #endregion

        #region Public Methods

        public void SpawnPlayer(GameObject type, Vector3 position, PlayerNumber number, bool isRespawning = false)
        {
            if (isRespawning == true)
            {
                lives--;
                if (lives <= 0)
                {
                    return;
                }
            }
            var go = Instantiate(type, position, Quaternion.identity);
            go.SetActive(true);
        }

        public void SpawnPlayer(GameObject player, PlayerNumber number, bool isRespawning = false)
        {
            if (isRespawning == true)
            {
                lives--;
                if (lives <= 0)
                {
                    return;
                }
            }
        }

        public void EnableOrDisableAllPlayers(bool enable = true)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].gameObject.SetActive(enable);
            }
        }

        #endregion


    }
}