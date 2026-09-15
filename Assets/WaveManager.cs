using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public sealed class WaveManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Wave Settings")]
    [SerializeField, Min(1)] private int maxWaves = 5;
    [SerializeField, Min(1)] private int firstWaveEnemyCount = 3;
    [SerializeField, Min(0f)] private float timeBetweenWaves = 3f;
    [SerializeField, Min(0.05f)] private float spawnInterval = 0.5f;

    [Header("User Interface")]
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject restartButton;

    private readonly List<GameObject> livingEnemies = new List<GameObject>();
    private PlayerHealth playerHealth;

    public int CurrentWave { get; private set; }
    public bool IsComplete { get; private set; }
    public int LivingEnemyCount => livingEnemies.Count;

    private IEnumerator Start()
    {
        SetVictoryScreen(false);
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) playerHealth = playerObject.GetComponent<PlayerHealth>();

        if (!ValidateConfiguration()) yield break;

        for (CurrentWave = 1; CurrentWave <= maxWaves; CurrentWave++)
        {
            if (PlayerIsDead()) yield break;

            if (waveText != null) waveText.text = $"WAVE {CurrentWave} INCOMING";
            yield return new WaitForSeconds(timeBetweenWaves);

            int enemyCount = firstWaveEnemyCount + CurrentWave - 1;
            for (int i = 0; i < enemyCount; i++)
            {
                if (PlayerIsDead()) yield break;
                SpawnEnemy(CurrentWave);
                UpdateWaveUI();
                yield return new WaitForSeconds(spawnInterval);
            }

            while (livingEnemies.Count > 0)
            {
                RemoveDestroyedEnemies();
                UpdateWaveUI();
                if (PlayerIsDead()) yield break;
                yield return new WaitForSeconds(0.15f);
            }
        }

        CompleteGame();
    }

    private bool ValidateConfiguration()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("WaveManager: Enemy Prefab is not assigned.", this);
            if (waveText != null) waveText.text = "SETUP ERROR: ENEMY PREFAB";
            return false;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("WaveManager: Spawn Points are not assigned.", this);
            if (waveText != null) waveText.text = "SETUP ERROR: SPAWN POINTS";
            return false;
        }

        return true;
    }

    private void SpawnEnemy(int waveNumber)
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 position = point.position;
        if (NavMesh.SamplePosition(point.position, out NavMeshHit hit, 4f, NavMesh.AllAreas))
        {
            position = hit.position + Vector3.up;
        }

        GameObject enemy = Instantiate(enemyPrefab, position, point.rotation);
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null) enemyAI.ConfigureForWave(waveNumber);
        livingEnemies.Add(enemy);
    }

    private void RemoveDestroyedEnemies()
    {
        livingEnemies.RemoveAll(enemy => enemy == null);
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = $"WAVE: {CurrentWave} / {maxWaves}    ENEMIES: {livingEnemies.Count}";
        }
    }

    private bool PlayerIsDead()
    {
        return playerHealth != null && playerHealth.IsDead;
    }

    private void CompleteGame()
    {
        IsComplete = true;
        ProceduralAudio.PlayVictory();
        if (waveText != null) waveText.text = "ALL WAVES CLEARED";
        SetVictoryScreen(true);

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            FpsPlayerController controller = playerObject.GetComponent<FpsPlayerController>();
            if (controller != null)
            {
                controller.SetInputEnabled(false);
                controller.enabled = false;
            }

            WeaponShooter weapon = playerObject.GetComponentInChildren<WeaponShooter>();
            if (weapon != null) weapon.enabled = false;
        }

        GameRuntime.ReleaseCursor();
    }

    private void SetVictoryScreen(bool visible)
    {
        if (victoryText != null) victoryText.SetActive(visible);
        if (restartButton != null) restartButton.SetActive(visible);
    }

    private void OnValidate()
    {
        maxWaves = Mathf.Max(1, maxWaves);
        firstWaveEnemyCount = Mathf.Max(1, firstWaveEnemyCount);
        timeBetweenWaves = Mathf.Max(0f, timeBetweenWaves);
        spawnInterval = Mathf.Max(0.05f, spawnInterval);
    }
}
