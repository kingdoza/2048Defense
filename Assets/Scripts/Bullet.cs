using System.Collections;
using UnityEngine;

public class Bullet : PooledObject {
    public int damage;
    private const float Speed = 50f;

    public void Fire(Enemy _target) {
        if(_target == null)
            Destroy();
        StartCoroutine(KeepMovingTo(_target));
    }

    private IEnumerator KeepMovingTo(Enemy _target) {
        Vector3 targetPos = _target.transform.position;
        bool isArrived = Vector3.Distance(transform.position, targetPos) < 0.0001f;
        while(!isArrived && _target) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
            isArrived = Vector3.Distance(transform.position, targetPos) < 0.0001f;
            yield return null;
        }
        if(_target != null)
            _target.Hit(damage);
        Destroy();
    }
}
