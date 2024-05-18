using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerColorCalculator : DestroyableSingleton<TowerColorCalculator> {
    private Color minColor = new Color32(0, 180, 255, 255);
    private Color maxColor = new Color32(0, 83, 90, 255);
    private Color colorGap;

    private void Awake() {
        const int ColorStep = 8;
        colorGap = (maxColor - minColor) / ColorStep;
    }

    public Color GetPowerColor(int _power) {
        float powerToLog2 = Mathf.Log(_power, 2);
        int powerStep = Mathf.RoundToInt(powerToLog2) - 1;
        return minColor + colorGap * powerStep;
    }
}
