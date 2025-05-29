using UnityEngine;
using System.Collections;
using UI;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public WaveUI waveUI;
        
        public GameObject enemyPrefab;
        public GameObject bossPrefab;
        
        public Transform[] spawnPoints;

        public int enemiesPerWave = 3;
        public float timeBetweenWaves = 5f;

        public int waveNumber = 1;
        private int _enemiesAlive = 0;
        
        private bool _isWaveInProgress = false;

        private void Start()
        {
            StartCoroutine(StartNextWave());
        }

        private void Update()
        {
            if (_enemiesAlive <= 0 && !_isWaveInProgress)
            {
                print("Começa a wave");
                StartCoroutine(StartNextWave());
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        IEnumerator StartNextWave()
        {
            _isWaveInProgress = true;
            
            Debug.Log("Wave " + waveNumber + " starting!");
            waveUI.ShowWave(waveNumber);
            yield return new WaitForSeconds(timeBetweenWaves);

            enemiesPerWave = waveNumber * 2; // Aumenta a dificuldade progressivamente

            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f); // Delay entre spawns (opcional)
            }
            
            if (waveNumber % 5 == 0)
            {
                SpawnBoss();
            }

            waveNumber++;
            _isWaveInProgress = false;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void SpawnEnemy()
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position + randomOffset, Quaternion.identity);

            _enemiesAlive++;

            enemy.GetComponent<Enemy>().OnDeath += OnEnemyDeath;
        }
        
        private void SpawnBoss()
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            GameObject boss = Instantiate(bossPrefab, spawnPoint.position + randomOffset, Quaternion.identity);

            _enemiesAlive++;

            boss.GetComponent<Enemy>().OnDeath += OnEnemyDeath;
        }

        private void OnEnemyDeath()
        {
            _enemiesAlive--;
        }
    }
}
