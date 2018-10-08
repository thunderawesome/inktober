using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.UI;

public class _PlayerManager : MonoBehaviour
{
    public enum PlayerNumber
    {
        One,
        Two
    }

    public CinemachineVirtualCamera cam;

    public GameObject gameOverScreen;

	public int lives = 5;

    public static _PlayerManager Instance;

    public Transform spawnPoint;

    public Text playerLivesText;

	// Use this for initialization
	void Awake ()
	{
	    Instance = this;
	}

    public void RevivePlayer(GameObject type, Vector3 position, PlayerNumber number)
    {
        lives--;
        if (lives <= 0) {
			GameOver ();
			return;
		}
        var go = Instantiate(type, position, Quaternion.identity);
        go.SetActive(true);
		

        playerLivesText.text = lives.ToString();
    }

	public void GameOver(){
		gameOverScreen.SetActive (true);
	}
}
