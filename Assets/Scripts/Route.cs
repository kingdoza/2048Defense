using UnityEngine;

public class Route : MonoBehaviour {
    private Transform[] points;

    private void Awake() {
        points = transform.GetComponentsInChildren<Transform>();
    }

    public Transform GetPoint(int _pointIndex) {
        _pointIndex++;
        if(_pointIndex >= points.Length || _pointIndex < 1)
            return null;
        return points[_pointIndex];
    }
}
