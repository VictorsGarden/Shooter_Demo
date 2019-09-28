using UnityEngine;

[AddComponentMenu("Pool/PoolSetup")]
public class PoolSetup : MonoBehaviour {

    [SerializeField] private PoolManager.PoolInstance[] pools;

    private void OnValidate() {
        for (int i = 0; i < pools.Length; i++) {
            pools[i].name = pools[i].prefab.name;
        }
    }

    private void Awake() {
        Initialize();
    }

    private void Initialize() {
        PoolManager.Initialize(pools);
    }

}
