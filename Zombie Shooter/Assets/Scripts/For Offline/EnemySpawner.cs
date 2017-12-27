using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public bool devMode;
    public Wave[] waves;
    public Enemy enemy;
	public ScoreManager ScoreMGR;

    LivingEntity playerEntity;
    Transform playerT;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    MapGenerator map;

   public float timeBetweenCampingChecks = 2;
    float campThresholdDistance = 1.5f;
    float nextCampCheckTime;
    Vector3 campPositionOld;
    bool isCamping;

    bool isDisabled;

    public event System.Action<int> OnNewWave;

    void Start()
    {
        playerEntity = FindObjectOfType<offlinePlayerController>();
        playerT = playerEntity.transform;

        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        campPositionOld = playerT.position;
        playerEntity.OnDeath += OnPlayerDeath;

        map = FindObjectOfType<MapGenerator>();
        NexWave();
		ScoreMGR.enemycount.text = "Enemies To Kill :- " + currentWave.enemyCount;
    }

    void Update()
    {
        if (!isDisabled)
        {
            if (Time.time > nextCampCheckTime)
            {
                nextCampCheckTime = Time.time + timeBetweenCampingChecks;

                isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campThresholdDistance);
                campPositionOld = playerT.position;
            }
			if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

				StartCoroutine("SpawnEnemy");
            }
        }
		if (devMode) {
			if (Input.GetKeyDown(KeyCode.Return)) {
				StopCoroutine("SpawnEnemy");
				foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
					GameObject.Destroy(enemy.gameObject);
				}
				NexWave();
			}
		}
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;

        Transform spawnTile = map.GetRandomOpenTile();
        if (isCamping)
        {
            spawnTile = map.GetTileFromPosition(playerT.position);
        }
        Material tileMat = spawnTile.GetComponent<Renderer>().material;
        Color initialColour = tileMat.color;
        Color flashColour = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {

            tileMat.color = Color.Lerp(initialColour, flashColour, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;
		spawnedEnemy.SetCharacteristics (currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColour);
    }

    void OnPlayerDeath()
    {
        isDisabled = true;
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
		ScoreMGR.enemycount.text = "Enemies To Kill :- " + enemiesRemainingAlive;
		ScoreMGR.CoinCollected (currentWave.coinsToGive);
        if (enemiesRemainingAlive == 0)
            NexWave();
    }

    void ResetPlayerPosition()
    {
        playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
    }

    void NexWave()
    {
		if (currentWaveNumber > 0)
			AudioManager.instance.PlaySound2D ("Level Complete");
        currentWaveNumber++;
		ScoreMGR.tmptext.text = "WAVE " + currentWaveNumber;
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];
            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = currentWave.enemyCount;
			ScoreMGR.enemycount.text = "Enemies To Kill :- " + currentWave.enemyCount;
        }
        if (OnNewWave != null)
        {
            OnNewWave(currentWaveNumber);
        }
        ResetPlayerPosition();
    }

    [System.Serializable]
    public class Wave
    {
		public bool infinite;
        public int enemyCount;
        public float timeBetweenSpawns;

		public float moveSpeed;
		public int hitsToKillPlayer;
		public float enemyHealth;
		public Color skinColour;
		public int coinsToGive;
    }
}
