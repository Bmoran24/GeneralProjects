using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Movement : MonoBehaviour
{
    Vector3 objectPosition = Vector3.zero;

    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    float speed = 1.0f;

    Vector3 direction = new Vector3(1,-1,0);

    Vector3 velocity = Vector3.one;

    CollisionManager manager;

    bool setDirection = false;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<CollisionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(setDirection==false)
        {
            objectPosition = new Vector3(Random.Range(-15,-6),Random.Range(4,6));
            setDirection = true;
        }

        if(transform.position.y<=-6)
        {
            GameObject newEnemy = Instantiate(enemyPrefab);
            newEnemy.transform.position = new Vector3(Random.Range(-15,-6), Random.Range(4, 6));
            manager.Collidables.Add(newEnemy.GetComponent<SpriteInfo>());

            Destroy(gameObject);
        }

        velocity = direction * speed * Time.deltaTime;


        objectPosition += velocity;
        transform.position = objectPosition;
    }
}
