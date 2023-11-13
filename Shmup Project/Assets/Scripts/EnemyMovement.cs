using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Vector3 objectPosition = Vector3.zero;

    [SerializeField]
    SpriteRenderer bulletPrefab;

    [SerializeField]
    float speed = 1.0f;

    Vector3 direction = Vector3.left;

    Vector3 velocity = Vector3.one;

    CollisionManager manager;

    List<SpriteRenderer> bullets = new List<SpriteRenderer>();

    bool left = true;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        objectPosition = transform.position;
        manager = GameObject.Find("Manager").GetComponent<CollisionManager>();

    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;

        // Shoots a bullet at the player
        if(timer>1)
        {
            timer = 0;

            SpriteRenderer bullet = Instantiate(bulletPrefab);
            Vector3 shipPos = GetComponentInParent<Transform>().position;

            bullet.transform.position = new Vector3(shipPos.x, shipPos.y - 1, shipPos.z);

            bullets.Add(bullet);

            bullet.GetComponentInParent<SpriteInfo>().IsEnemyBullet = true;

            manager.Collidables.Add(bullet.GetComponentInParent<SpriteInfo>());
        }

        velocity = direction * speed * Time.deltaTime;

        if(left)
        {
            direction = Vector3.left;
        }
        else
        {
            direction = Vector3.right;
        }

        if(objectPosition.x<=-16f)
        {
            left = false;
        }

        if(objectPosition.x>=16)
        {
            left = true;
        }

        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i] == null)
            {
                continue;
            }

            if (bullets[i].gameObject==null)
            {
                Destroy(bullets[i].gameObject);
            }

            Vector3 currentPosition = bullets[i].transform.position;

            bullets[i].transform.position = new Vector3(currentPosition.x,
                currentPosition.y - .05f, currentPosition.z);

            if (currentPosition.y < -6)
            {
                Destroy(bullets[i].gameObject);
                bullets.Remove(bullets[i]);
            }
        }

        // Checks for 

        objectPosition += velocity;
        transform.position = objectPosition;
    }
}
