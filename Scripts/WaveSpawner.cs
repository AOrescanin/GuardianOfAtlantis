using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles the start of the round when enemies are being spawned
public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Waves
    {
        public string name;
        public float waveSpawnRate;
        public SubWaves [] subWaves;
        public Transform spawnPointA;
            public Transform spawnPointB;
        [System.Serializable]
        public class SubWaves
        {

            public WaveGroup [] ememies;
            public float enemySpawnRate;

            [System.Serializable]
            public class WaveGroup
            {
                public Transform enemy;
                public int count;
            }
        }
    }

    [SerializeField] private Waves [] waves;
    private int nextWaveIndex = 0;
    [SerializeField] private float searchCooldown = .9f;
    private float searchCountdown;
    [SerializeField] GameObject RoundDisplay;
    [SerializeField] Button playButton;
    StateManager stateManager;

    private void Start() 
    {
        searchCountdown = searchCooldown;
        stateManager = StateManager.instance;
    }

    private void Update()
    {
        if(stateManager.getState() == StateManager.SpawnState.PLAYING)
        {
            if(!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }
    }

    public void StartWave()
    {
        if(stateManager.getPowerCoreIsActive() && stateManager.getRound() < 31)
        {
            nextWaveIndex = 30;
        }
        
        if(stateManager.getState() == StateManager.SpawnState.WAITING)
        {
            StartCoroutine(SpawnWaves(waves[nextWaveIndex]));
        }
    }

    private bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;

        if(searchCountdown <= 0)
        {
            searchCountdown = searchCooldown;

            if(GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator SpawnWaves(Waves _wave)
    {
        stateManager.setState(StateManager.SpawnState.PLAYING);
        stateManager.incrementRound();
    
        for(int j = 0; j < _wave.subWaves.Length; j++)
        {
            for(int i = 0; i < _wave.subWaves[j].ememies.Length; i++)
            {
                for(int y = 0; y < _wave.subWaves[j].ememies[i].count; y++)
                {
                    if(y % 2 == 0)
                    {
                        SpawnEnemy(_wave.subWaves[j].ememies[i].enemy, _wave.spawnPointA);
                    }
                    else
                    {
                        SpawnEnemy(_wave.subWaves[j].ememies[i].enemy, _wave.spawnPointB);
                    }

                    yield return new WaitForSeconds(_wave.subWaves[j].enemySpawnRate);
                }
            }
            yield return new WaitForSeconds(_wave.waveSpawnRate);
        }
        yield break;
    }

    private void SpawnEnemy(Transform _enemy, Transform _spawnPoint)
    {
        Instantiate(_enemy, _spawnPoint.position, _spawnPoint.rotation);
    }

    private void WaveCompleted()
    {
        stateManager.setState(StateManager.SpawnState.WAITING);
        stateManager.displayRoundNumber();

        if(nextWaveIndex + 1 >= waves.Length)
        {
            nextWaveIndex = 0;
            return;
        }

        nextWaveIndex ++;
    }

}
