using UnityEngine;
using System.Collections;

public class DisableOverTime : MonoBehaviour {

	public float timer = 2;

    public bool canDestroy = false;

	private void Start(){
		Invoke ("Disable", timer);
	}

	private void Disable(){

        if (canDestroy == true)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
	}
}
