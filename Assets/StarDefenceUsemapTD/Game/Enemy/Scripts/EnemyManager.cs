using STARTD.Common;
using STARTD.Game.Enemy;
using STARTD.Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    private List<EnemyBehaviour> spawnedEnemys = new();

    [SerializeField] private EnemySpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner.enemys = spawnedEnemys;
        spawner.Spawn();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ForceSetTargets()
    {
        foreach(var enemy in spawnedEnemys)
        {
            enemy.ForceSetTarget();
        }
    }

    public void EnemyKilled(EnemyBehaviour killedEnemy)
    {
        if (spawnedEnemys.Contains(killedEnemy))
        {
            spawnedEnemys.Remove(killedEnemy);
            HealthBarManager.Singleton.RemoveHealthBar(killedEnemy.transform);
            Destroy(killedEnemy.gameObject);
            GameScene.Singleton.TryAddGold(killedEnemy.DropGold);
        }


        if (spawner.queue.Count <= 0 && spawnedEnemys.Count <= 0)
        {
            GameScene.Singleton.WinGame();
        }
        else if(spawner.queue.Count > 0 && spawnedEnemys.Count <= 0)
        {
            spawner.Spawn();
        }
    }

    public void SpawnElite(int index)
    {
        spawner.SpawnElite(index);
    }
}
