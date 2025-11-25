using STARTD.Game.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner.Spawn();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
