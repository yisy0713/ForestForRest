using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] PassiveItem;
    [SerializeField]
    private GameObject[] dropItem;
    [SerializeField]
    private GameObject Enemy;

    [SerializeField]
    ParticleSystem spawnerParticle;

    [SerializeField]
    private int minEnemySpawnNumber;

    [SerializeField]
    private int maxEnemySpawnNumber;

    [SerializeField]
    private int minDropItem;

    [SerializeField]
    private int maxDropItem;

    private int enemySpawnNumber;

    public bool isSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawnNumber = Random.Range(minEnemySpawnNumber, maxEnemySpawnNumber);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawned)
        {
            CheckAllEnemiesDead();
        }
    }

    public void EnemySpawn()
    {
        if (!isSpawned)
        {
            isSpawned = true;
            StartCoroutine(SpawnEnemyAfterDelay(enemySpawnNumber, 1f));
        }
    }

    private void Reward()
    {
        GameObject obj = PassiveItem[Random.Range(0, PassiveItem.Length)];
        Vector3 dropPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);//_transform.position;

        Instantiate(obj, dropPos, Quaternion.identity);
    }

    IEnumerator SpawnEnemyAfterDelay(int count, float delay)
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        Instantiate(Enemy, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(delay);
    }

    private void CheckAllEnemiesDead()
    {
        //GameObject[] spawnEnemies = GameObject.FindGameObjectsWithTag("SpawnedEnemy");
        if (Enemy.GetComponent<EnemyManager>().enemyDead)
        {
            spawnerParticle.Stop();
            DropItem();
            Reward();
            Destroy(gameObject);
        }

        //if (spawnEnemies.Length == 0)
        //{
        //    spawnerParticle.Stop();
        //    DropItem();
        //    Reward();
        //    Destroy(gameObject);
        //}
    }

    public void particleActive()
    {
        spawnerParticle.Play();

    }

    private void DropItem()
    {
        int numOfDropItem = Random.Range(minDropItem, maxDropItem);
        for (int i = 0; i < numOfDropItem; i++)
        {
            GameObject obj = dropItem[Random.Range(0, dropItem.Length)];
            Vector3 dropPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

            Instantiate(obj, dropPos, Quaternion.identity);
        }
    }
}
