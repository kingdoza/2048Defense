using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerController : DestroyableSingleton<TowerController> {
    public GameObject towerPrefab;
    public int towerCount = 0;
    private bool canInput = true;

    void Update() {
        CheckArrowKeyInput();
    }

    private void CheckArrowKeyInput() {
        if(IsArrowKeyInputed() && canInput && GameManager.Instance.IsGame) {
            StartCoroutine(DelayInput());
            Vector2Int moveDir = GetMoveDirection();
            int movedTowerCount = MoveTowersTo(moveDir);
            if(movedTowerCount > 0)
                MakeRandomTowers(1);
        }

/*
        if(Input.GetKeyDown(KeyCode.LeftArrow) && canInput) {
            StartCoroutine(DelayInput());
            int movedTowerCount = 0;
            for(int i = 0; i < 4; ++i) {
                Vector2Int currentPos = new Vector2Int(i, 0);
                TowerGrid grid = GridBox.Instance.GetGrid(currentPos);
                movedTowerCount += grid.MoveTowerLeft();
            }
            for(int i = 0; i < 4; ++i) {
                Vector2Int currentPos = new Vector2Int(i, 0);
                TowerGrid grid = GridBox.Instance.GetGrid(currentPos);
                grid.InitializeDoubled();
            }
            if(movedTowerCount > 0)
                MakeRandomTowers(1);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow) && canInput) {
            StartCoroutine(DelayInput());
            int movedTowerCount = 0;
            for(int i = 0; i < 4; ++i) {
                Vector2Int currentPos = new Vector2Int(3 - i, 0);
                TowerGrid grid = GridBox.Instance.GetGrid(currentPos);
                movedTowerCount += grid.MoveTowerRight();
            }
            for(int i = 0; i < 4; ++i) {
                Vector2Int currentPos = new Vector2Int(3 - i, 0);
                TowerGrid grid = GridBox.Instance.GetGrid(currentPos);
                grid.InitializeDoubled();
            }
            if(movedTowerCount > 0)
                MakeRandomTowers(1);
        }
        */
    }

    private int MoveTowersTo(Vector2Int _moveDir) {
        int movedTowerCount = 0;
        TowerGrid[] gridsByMovingOrder = new TowerGrid[GridBox.Instance.GridCount];
        for(int i = 0; i < GridBox.Instance.GridCount; ++i) {
            TowerGrid grid = GridBox.Instance.GetGridFromDirection(_moveDir, i);
            gridsByMovingOrder[i] = grid;
            movedTowerCount += grid.MoveTower(_moveDir);
        }
        for(int i = 0; i < gridsByMovingOrder.Length; ++i) {
            gridsByMovingOrder[i].InitializeDoubled();
        }
        return movedTowerCount;
    }

    private bool IsArrowKeyInputed() {
        return (Input.GetKeyDown(KeyCode.LeftArrow) || 
            Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) || 
            Input.GetKeyDown(KeyCode.DownArrow));
    }

    private Vector2Int GetMoveDirection() {
        Vector2Int moveDir = new Vector2Int();
        if(Input.GetKeyDown(KeyCode.LeftArrow))
            moveDir = Vector2Int.left;
        if(Input.GetKeyDown(KeyCode.RightArrow))
            moveDir = Vector2Int.right;
        if(Input.GetKeyDown(KeyCode.UpArrow))
            moveDir = Vector2Int.up;
        if(Input.GetKeyDown(KeyCode.DownArrow))
            moveDir = Vector2Int.down;
        return moveDir;
    }

    private IEnumerator DelayInput() {
        canInput = false;
        const float InputDelay = 0.4f;
        yield return new WaitForSeconds(InputDelay);
        canInput = true;
    }

    public void MakeTowers() {
        MakeRandomTowers(2);
        /*
        Vector2Int pos1 = new Vector2Int(0, 0);
        Vector2Int pos2 = new Vector2Int(1, 0);
        Vector2Int pos3 = new Vector2Int(2, 0);
        Vector2Int pos4 = new Vector2Int(3, 0);
        TowerGrid grid1 = GridBox.Instance.GetGrid(pos1);
        TowerGrid grid2 = GridBox.Instance.GetGrid(pos2);
        TowerGrid grid3 = GridBox.Instance.GetGrid(pos3);
        TowerGrid grid4 = GridBox.Instance.GetGrid(pos4);
        grid1.MakeNewTower(16);
        grid2.MakeNewTower(8);
        grid3.MakeNewTower(4);
        grid4.MakeNewTower(4);
        */
    }

    public void MakeRandomTowers(int _count) {
        if(towerCount + _count > GridBox.Instance.GridCount)
            return;
        for(int i = 0; i < _count; ++i) {
            TowerGrid randomGrid = GridBox.Instance.GetRandomGrid();
            while(randomGrid.tower != null)
                randomGrid = GridBox.Instance.GetRandomGrid();
            randomGrid.MakeNewTower();
        }
    }

    public void DeleteAll() {
        for(int i = 0; i < GridBox.Instance.GridCount; ++i) {
            TowerGrid grid = GridBox.Instance.GetGrid(i);
            if(grid.tower) {
                grid.tower.Destroy();
                grid.tower = null;
            }
        }
    }
}
