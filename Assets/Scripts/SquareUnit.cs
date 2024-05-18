using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements.Experimental;

public abstract class SquareUnit : PooledObject {
    private const float ScaleSpeed = 0.05f;
    protected abstract int Value {get; set;}
    private TextMeshPro numberText;

    protected new void Awake() {
        numberText = transform.GetChild(0).GetComponent<TextMeshPro>();
        base.Awake();
    }

    protected void OnEnable() {
        StartCoroutine(EnlargeScale());
    }

    protected IEnumerator EnlargeScale() {
        Vector3 originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        while(transform.localScale.x < originalScale.x && 
            transform.localScale.x < originalScale.x) {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, ScaleSpeed);
            yield return null;
        }
    }

    public void ApplyValue(int _value) {
        Value = _value;
        numberText.text = Value.ToString();
    }
}
