using UnityEngine;

public class TowerGrid : MonoBehaviour {
    public Vector2Int pos;
    private GameObject towerPrefab;
    public Tower tower;

    private void Awake() {
        towerPrefab = TowerController.Instance.towerPrefab;
    }

    public Tower MakeNewTower() {
        GameObject towerObject = ObjectPoolManager.Instance.towerPool.PullOut(transform.position);
        //GameObject towerObject = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        Tower tower = towerObject.GetComponent<Tower>();
        tower.InitializePower();
        this.tower = tower;
        return tower;
    }

    public Tower MakeNewTower(int _power) {
        GameObject towerObject = ObjectPoolManager.Instance.towerPool.PullOut(transform.position);
        //GameObject towerObject = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        Tower tower = towerObject.GetComponent<Tower>();
        tower.InitializePower(_power);
        this.tower = tower;
        return tower;
    }

    public int MoveTowerLeft() {
        if(tower == null)
            return 0;
        TowerGrid targetgrid = GetTargetGrid(Vector2Int.left);
        if(targetgrid == this)
            return 0;
        tower.MoveTo(targetgrid);
        tower = null;
        return 1;
    }

    public int MoveTowerRight() {
        if(tower == null)
            return 0;
        TowerGrid targetgrid = GetTargetGrid(Vector2Int.right);
        if(targetgrid == this)
            return 0;
        tower.MoveTo(targetgrid);
        tower = null;
        return 1;
    }

    public int MoveTower(Vector2Int _moveDir) {
        if(tower == null)
            return 0;
        TowerGrid targetgrid = GetTargetGrid(_moveDir);
        if(targetgrid == this)
            return 0;
        tower.MoveTo(targetgrid);
        tower = null;
        return 1;
    }

    private TowerGrid GetTargetGrid(Vector2Int _searchDir) {
        TowerGrid targetgrid;
        TowerGrid currentGrid = GridBox.Instance.GetGrid(pos);
        TowerGrid nextGrid = GridBox.Instance.GetGrid(pos + _searchDir);
        while(nextGrid != null && !nextGrid.IsTower()) {
            currentGrid = nextGrid;
            nextGrid = GridBox.Instance.GetGrid(currentGrid.pos + _searchDir);
        }
        if(nextGrid == null || !nextGrid.CanEnter(tower.Power))
            targetgrid = currentGrid;
        else
            targetgrid = nextGrid;
        return targetgrid;
    }

    public bool IsTower() {
        return tower != null;
    }

    public bool CanEnter(int _power) {
        if(tower == null)
            return true;
        if(tower.Power != _power || tower.isDoubled)
            return false;
        return true;
    }

    public void InitializeDoubled() {
        if(tower == null)
            return;
        tower.isDoubled = false;
    }
}
