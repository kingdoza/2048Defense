using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private ObjectPool enemyPool;
    [SerializeField] private Route movingRoute;
    private Transform spawningPoint;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float spawnDeviation;

    private void Awake() {
        enemyPool = ObjectPoolManager.Instance.enemyPool;
        enemyPool.Initialize(10);
    }

    private void Start() {
        spawningPoint = movingRoute.GetPoint(0);
        StartCoroutine(KeepSpawningEnemy());
    }
    
    private IEnumerator KeepSpawningEnemy() {
        while(gameObject) {
            yield return null;
            if(GameManager.Instance.IsGame) {
                SpawnEnemy();
                float delay = Random.Range(spawnDelay - spawnDeviation, spawnDelay + spawnDeviation);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    private Enemy SpawnEnemy() {
        Enemy enemy = enemyPool.PullOut(spawningPoint.position).GetComponent<Enemy>();
        EnemyList.Instance.enemies.Add(enemy);
        enemy.ApplyValue(16);
        enemy.StartMove(movingRoute);
        return enemy;
    }
}
