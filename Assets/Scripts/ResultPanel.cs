using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ResultPanel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI killsResult;
    [SerializeField] private TextMeshProUGUI damagesResult;

    public void ApplyResults() {
        killsResult.text = GameManager.Instance.Kills.ToString("N0");
        damagesResult.text = GameManager.Instance.totalDamage.ToString("N0");
    }

    public void Show() {
        StartCoroutine(FadeIn());
    }

    public void Hide() {
        GetComponent<CanvasGroup>().alpha = 0;
    }

    private IEnumerator FadeIn() {
        const float FadeInDuration = 0.5f;
        float transparencyDelta = 1 / FadeInDuration * Time.deltaTime;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        while(canvasGroup.alpha < 1) {
            canvasGroup.alpha += transparencyDelta;
            yield return null;
        }
    }
}
