using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private float currentTime,createTime = 1f;
    public GameObject enemyFactory;
    private float minTime = 1, maxTime = 5;

    private bool LeftRight;
    // Start is called before the first frame update
    void Start()
    {
        LeftRight = true;
        createTime = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (LeftRight)
        {
            transform.Translate(Time.deltaTime,0, 0);
        }
        else
        {
            transform.Translate(-Time.deltaTime,0, 0);
        }

        if (transform.position.x > 3.5)
            LeftRight = false;
        if (transform.position.x < -3.5)
            LeftRight = true;

        if (Time.time - currentTime>createTime)
        {
            currentTime = Time.time;
            GameObject enemy = Instantiate(enemyFactory);
            enemy.transform.position = transform.position;

            createTime = Random.Range(minTime, maxTime);
        }
    
    }
}
