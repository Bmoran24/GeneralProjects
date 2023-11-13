using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.SocialPlatforms.Impl;

public class CollisionManager : MonoBehaviour
{
    [SerializeField]
    private List<SpriteInfo> collidables = new List<SpriteInfo>();

    float timer = 0;
    int score = 0;

    GameObject player;
    GameObject powerPrefab;

    public int Score { get { return score; } }

    public List<SpriteInfo> Collidables { get { return collidables; } set { collidables = value; } }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Spaceship");
        powerPrefab = collidables[0].PowerPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= .8f && GameObject.Find("Spaceship") != null)
        {
            timer = 0;
            score++;
            GetComponentInParent<TextMeshPro>().text = "Score: " + score;
        }

        for(int i=0; i<collidables.Count; i++)
        {
            if (collidables[i]==null)
            {
                collidables.Remove(collidables[i]);
                break;
            }
        }

        foreach (SpriteInfo s in collidables)
        {
            s.IsColliding = false; 
        }

        for (int i = 0; i < collidables.Count; i++)
        {
            for (int j = 0; j < collidables.Count; j++)
            {
                if (collidables[i] == null || collidables[j]==null)
                {
                    continue;
                }

                if (AABBCheck(collidables[i], collidables[j]) && i != j)
                {
                    if (collidables[i].IsEnemy && collidables[j].IsEnemy)
                    {
                        continue;
                    }
                    else if (collidables[i].IsEnemyBullet && collidables[j].IsEnemy)
                    {
                        continue;
                    }
                    else if (collidables[i].IsEnemy && collidables[j].IsEnemyBullet)
                    {
                        continue;
                    }
                    else if (collidables[i].IsPower)
                    {
                        player.GetComponent<MovementController>().Speed += 2;
                        collidables.Remove(powerPrefab.GetComponent<SpriteInfo>());
                        Destroy(collidables[i]);
                        continue;
                    }
                    else if (collidables[j].IsPower)
                    {
                        player.GetComponent<MovementController>().Speed += 1;
                        Destroy(collidables[j]);
                        continue;
                    }


                    collidables[i].IsColliding = true;
                    collidables[j].IsColliding = true;
                }
            }
        }

    }

    /// <summary>
    /// Checks if 2 rectangular sprites are colliding
    /// </summary>
    /// <param name="spriteA">Sprite 1</param>
    /// <param name="spriteB">Sprite 2</param>
    /// <returns>True if the sprites are colliding, false otherwise</returns>
    private bool AABBCheck(SpriteInfo spriteA, SpriteInfo spriteB)
    {
        return spriteB.RectMin.x < spriteA.RectMax.x &&
             spriteB.RectMax.x > spriteA.RectMin.x &&
             spriteB.RectMax.y > spriteA.RectMin.y &&
             spriteB.RectMin.y < spriteA.RectMax.y;
    }
}
       