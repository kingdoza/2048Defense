using System.Collections.Generic;
using UnityEngine;

public class EnemyList : DestroyableSingleton<EnemyList> {
    public List<Enemy> enemies = new List<Enemy>();

    public Enemy GetFirstEnemy() {
        if(enemies.Count > 0)
            return enemies[0];
        return null;
    }

    public void DeleteAll() {
        while(enemies.Count > 0) {
            enemies[0].Destroy();
        }
    }
}
