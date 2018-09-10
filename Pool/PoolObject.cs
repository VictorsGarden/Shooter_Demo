using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Pool/PoolObject")]
public class PoolObject : MonoBehaviour {

	public void ReturnToPool() {
        gameObject.SetActive(false);
    }
}
