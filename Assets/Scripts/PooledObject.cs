using UnityEngine;

public abstract class PooledObject : MonoBehaviour{
    protected ObjectPool objectPool;
    public ObjectPool ObjectPool { set => objectPool = value; }

    protected void Awake() {
        gameObject.SetActive(false);
    }

    protected virtual void Destroy() {
        objectPool.Insert(gameObject);
    } 
}