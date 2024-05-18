using System.Collections;
using TMPro;
using UnityEngine;

public class Tower : SquareUnit {
    private int power;
    public int Power => power;
    protected override int Value { get => power; set => power = value; }
    private TextMeshPro powerText;
    public bool isDoubled = false;
    private const int MoveSpeed = 30;
    private const float ScaleSpeed = 0.05f;
    private ObjectPool bulletPool;

    private new void Awake() {
        bulletPool = ObjectPoolManager.Instance.bulletPool;
        bulletPool.Initialize(50);
        //powerText = transform.GetChild(0).GetComponent<TextMeshPro>();
        base.Awake();
    }

    private new void OnEnable() {
        ++TowerController.Instance.towerCount;
        //StartCoroutine(EnlargeScale());
        StartCoroutine(KeepAttacking());
        base.OnEnable();
    }

    private IEnumerator KeepAttacking() {
        while(GameManager.Instance.IsGame) {
            yield return null;
            Enemy targetEnemy = EnemyList.Instance.GetFirstEnemy();
            if(targetEnemy == null)
                continue;
            Attack(targetEnemy);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Attack(Enemy _target) {
        Bullet bullet = bulletPool.PullOut(transform.position).GetComponent<Bullet>();
        bullet.damage = power;
        bullet.Fire(_target);
    }

    public new void Destroy() {
        --TowerController.Instance.towerCount;
        StopAllCoroutines();
        base.Destroy();
    }

/*
    private IEnumerator EnlargeScale() {
        Vector3 originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        while(transform.localScale.x < originalScale.x && 
            transform.localScale.x < originalScale.x) {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, ScaleSpeed);
            yield return null;
        }
    } */

    public void InitializePower() {
        const float ProbabilityOf4 = 1/3f;
        float probability = Random.Range(0, 1f);
        if(probability <= ProbabilityOf4)
            power = 4;
        else
            power = 2;
        ApplyValue(power);
        //SetTextAndColor();
    }

    private new void ApplyValue(int _value) {
        base.ApplyValue(_value);
        GetComponent<SpriteRenderer>().color = TowerColorCalculator.Instance.GetPowerColor(power);
    }

    public void InitializePower(int _power) {
        ApplyValue(_power);
        //SetTextAndColor();
    }

    public void DoublePower() {
        isDoubled = true;
        power*= 2;
        ApplyValue(power);
    }

    private void SetColor() {
        GetComponent<SpriteRenderer>().color = TowerColorCalculator.Instance.GetPowerColor(power);
    }

    public IEnumerator KeepMovingTo(TowerGrid _targetGrid) {
        Vector3 target = _targetGrid.transform.position;
        while(Vector3.Distance(transform.position, target) >= 0.0001f) {
            transform.position = Vector3.Lerp(transform.position, target, MoveSpeed * Time.deltaTime);
            yield return null;
        }
        
    }

    public void MoveTo(TowerGrid _targetGrid) {
        StartCoroutine(KeepMovingTo(_targetGrid));
        if(_targetGrid.tower) {
            _targetGrid.tower.DoublePower();
            Destroy();
            //Destroy(gameObject);
        }
        else
            _targetGrid.tower = this;
        //_targetGrid.tower = _targetGrid.tower ? _targetGrid.tower : this;
    }
}
