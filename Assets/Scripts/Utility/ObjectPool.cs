using UnityEngine;

public class ObjectPool
{
    private GameObject[] objectPool;

    public void InitializePool(uint objectPoolSize, GameObject poolObject, string poolParentName)
    {
        objectPool = new GameObject[objectPoolSize];
        var objectPoolParent = new GameObject(poolParentName).transform;

        for (int i = 0; i < objectPool.Length; ++i)
        {
            objectPool[i] = GameObject.Instantiate(poolObject);
            objectPool[i].SetActive(false);
            objectPool[i].transform.SetParent(objectPoolParent);
        }
    }
    public GameObject GetNextObjectFromPool()
    {
        foreach (var obj in objectPool)
            if (!obj.activeInHierarchy)
                return obj;

        Debug.LogError("Pool size is too small!");
        Debug.Break();
        return null;
    }
}
