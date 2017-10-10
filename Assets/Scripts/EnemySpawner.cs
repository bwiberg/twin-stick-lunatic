using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class EnemySpawner : MonoBehaviour {
	[SerializeField] private int InitialEnemyCount = 5;
	[SerializeField] private float SpawnInterval = 2.0f;
	[SerializeField] private float X0; 
	[SerializeField] private float X1; 
	[SerializeField] private float Z0; 
	[SerializeField] private float Z1; 
	[SerializeField] private float SpawnY;

	[SerializeField] private GameObject[] EnemyPrefabs;
	
	private float latestEnemySpawnTime = 0.0f;
	
	private void Start () {
		for (int i = 0; i < InitialEnemyCount; ++i) {
			SpawnEnemy();
		}
		
		StartCoroutine(SpawnEnemiesContinuosly());
	}
	
	private IEnumerator SpawnEnemiesContinuosly() {
		for (;;) {
			if (Time.time - latestEnemySpawnTime > SpawnInterval) {
				SpawnEnemy();
				latestEnemySpawnTime = Time.time;
			}
			yield return null;
		}
	}

	private void SpawnEnemy() {
		var position = new Vector3(Random.Range(X0, X1), SpawnY, Random.Range(Z0, Z1));
		var rotation = Quaternion.Euler(0, Random.Range(0.0f, 2 * Mathf.PI), 0);
		Instantiate(EnemyPrefabs.GetRandom(), position, rotation, transform);
	}
}
