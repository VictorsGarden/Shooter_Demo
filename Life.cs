using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour {
    public float health;

    // Get damage from weapon
    public void GetDamage(float damage) {
        health -= damage;
        Debug.Log(health);
    }

	// Use this for initialization
	private void Start () {
        health = 100;	
	}
	
	// Update is called once per frame
	private void Update () {
	    if (health <= 0)
            Destroy(gameObject);
	}
}
