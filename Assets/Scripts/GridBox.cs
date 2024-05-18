using System.Linq.Expressions;
using UnityEngine;

public class GridBox : DestroyableSingleton<GridBox> {
    private const int GridLength = 4;
    private TowerGrid[] grids = new TowerGrid[GridLength * GridLength];
    [SerializeField] private float gridGap;
    [SerializeField] private GameObject gridPrefab;
    public readonly int GridCount = GridLength * GridLength;

    private void Awake() {
        ArrangeGrid();
        ObjectPoolManager.Instance.towerPool.Initialize(GridCount);
        //TowerController.Instance.MakeTowers();
    }

    private void ArrangeGrid() {
        for(int i = 0; i < GridCount; ++i) {
            Vector2Int gridPos = new Vector2Int(i % GridLength, i / GridLength);
            Vector3 gridLocation = GetGridLocation(gridPos);
            GameObject gridObject = Instantiate(gridPrefab, gridLocation, Quaternion.identity);
            gridObject.transform.SetParent(transform);
            grids[i] = gridObject.GetComponent<TowerGrid>();
            grids[i].pos = gridPos;
        }
    }

    private Vector3 GetGridLocation(Vector2Int _pos) {
        const float MidPos = (GridLength - 1) / 2f;
        float xGap = (_pos.x - MidPos) * gridGap;
        float yGap = (_pos.y - MidPos) * gridGap;
        return transform.position + new Vector3(xGap, yGap);
    }

    public TowerGrid GetGrid(Vector2Int _pos) {
        if(!(_pos.x >= 0 && _pos.x < GridLength &&
            _pos.y >= 0 && _pos.y < GridLength))
            return null;
        int index = _pos.y * GridLength + _pos.x;
        return grids[index];
    }

    public TowerGrid GetGrid(int _index) {
        if(_index >= 0 && _index < GridCount)
            return grids[_index];
        return null;
    }

    public TowerGrid GetRandomGrid() {
        int xPos = Random.Range(0, GridLength);
        int yPos = Random.Range(0, GridLength);
        return GetGrid(new Vector2Int(xPos, yPos));
    }

    public TowerGrid GetGridFromDirection(Vector2Int _moveDir, int i) {
        int xPos, yPos;
        if(_moveDir.x == 0) {
            xPos = i % GridLength;
            yPos = i / GridLength;
        }
        else {
            xPos = i / GridLength;
            yPos = i % GridLength;
        }
        if(_moveDir.x > 0)
            xPos = GridLength - 1 - xPos;
        else if(_moveDir.y > 0)
            yPos = GridLength - 1 - yPos;
        Vector2Int gridPos = new Vector2Int(xPos, yPos);
        return GetGrid(gridPos);
    }
}
