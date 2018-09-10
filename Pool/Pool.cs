using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Pool/ObjectPooling")]
public class Pool {

    List<PoolObject> objects;
    Transform objectsParent;

    public void Initialize(int count, PoolObject sample, Transform objectsParent) {
        objects = new List<PoolObject>();
        this.objectsParent = objectsParent;

        for (int i = 0; i < count; i++)
            AddObject(sample, objectsParent);        
    }

    public void AddObject(PoolObject sample, Transform objectsParent) {
        GameObject temp;

        if (sample.gameObject.scene.name == null) {
            temp = GameObject.Instantiate(sample.gameObject);
            temp.name = sample.name;
            temp.transform.SetParent(objectsParent);

        } else {
            temp = sample.gameObject;
        }

        objects.Add(temp.GetComponent<PoolObject>());
        temp.SetActive(false);
    }

    public PoolObject GetObject() {
        for (int i = 0; i < objects.Count; i++) {

            if (objects[i].gameObject.activeInHierarchy == false)
                return objects[i];
        }

        AddObject(objects[0], this.objectsParent);
        objects.RemoveAt(0);
        return objects[objects.Count - 1];
    }
}
