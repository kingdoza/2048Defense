using System.Collections;
using UnityEngine;

public class Enemy : SquareUnit {
    private Route movingRoute;
    private int health = 100;
    private int targetPointIndex;
    private const float MoveSpeed = 1;
    protected override int Value { get => health; set => health = value; }
    private float originalScale;

    private new void Awake() {
        originalScale = transform.localScale.x;
        base.Awake();
    }

    public new void Destroy() {
        StopAllCoroutines();
        EnemyList.Instance.enemies.Remove(this);
        base.Destroy();
    }

    private void Die() {
        GameManager.Instance.Kills++;
        Destroy();
    }

    private void Hit() {
        GameManager.Instance.Lifes--;
        Destroy();
    }

    public void StartMove(Route _route) {
        movingRoute = _route;
        targetPointIndex = 0;
        StartCoroutine(KeepMoving());
    }

    private new void OnEnable() {
        transform.localScale = new Vector3(originalScale, originalScale);
        base.OnEnable();
    }

    private IEnumerator KeepMoving() {
        while(GameManager.Instance.IsGame) {
            yield return null;
            Vector3 targetPoint = movingRoute.GetPoint(targetPointIndex).position;
            bool isArrivedAtTarget = Vector3.Distance(transform.position, targetPoint) < 0.0001f;
            if(isArrivedAtTarget) {
                SetNextTargetPoint();
                continue;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, MoveSpeed * Time.deltaTime);
        }
    }

    private void SetNextTargetPoint() {
        if(!movingRoute.GetPoint(targetPointIndex + 1)) {
            Hit();
            return;
        }
        ++targetPointIndex;
    }

    public void Hit(int _damage) {
        if(!gameObject.activeSelf)
            return;
        health -= _damage;
        GameManager.Instance.totalDamage += _damage;
        ApplyValue(health);
        StopCoroutine(ActHitEffect());
        StartCoroutine(ActHitEffect());
        if(health <= 0) {
            Die();
        }
    }

    private IEnumerator ActHitEffect() {
        float minScale = originalScale;
        transform.localScale = new Vector3(minScale, minScale, minScale);
        float maxScale = originalScale * 1.2f;
        float duration = 0.04f;
        float time = 0f;
        while (time < duration) {
            float scaleFactor = Mathf.Lerp(minScale, maxScale, time / duration);
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            time += Time.deltaTime;
            yield return null;
        }
        time = 0f;
        while (time < duration) {
            float scaleFactor = Mathf.Lerp(maxScale, minScale, time / duration);
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
