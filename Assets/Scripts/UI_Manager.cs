using UnityEngine;
using TMPro;

public class UI_Manager : DestroyableSingleton<UI_Manager> {
    [SerializeField] private TextMeshProUGUI killScore;
    [SerializeField] private TextMeshProUGUI lifeScore;
    [SerializeField] private ResultPanel resultPanel;

    public void ApplyKills() {
        killScore.text = GameManager.Instance.Kills.ToString("N0");
    }

    public void ApplyLifes() {
        lifeScore.text = GameManager.Instance.Lifes.ToString("N0");
    }

    public void ShowGameResults() {
        resultPanel.ApplyResults();
        resultPanel.Show();
    }

    public void HideGameResults() {
        resultPanel.Hide();
    }
}
