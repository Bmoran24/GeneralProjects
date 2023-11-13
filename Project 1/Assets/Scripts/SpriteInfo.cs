using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpriteInfo : MonoBehaviour
{
    // Fields --
    [SerializeField]
    private float radius;

    [SerializeField]
    SpriteRenderer sRenderer;

    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    GameObject powerPrefab;

    [SerializeField]
    private Vector2 rectSize=Vector2.one;

    // Classifiers
    private bool isColliding = false;
    private bool isEnemyBullet = false;
    [SerializeField]
    private bool isPower = false;
    [SerializeField]
    private bool isEnemy = false;

    private CollisionManager manager;

    private GameObject player;

    // Properties --
    public Vector2 RectMin
    {
        get { return (Vector2)transform.position - rectSize / 2; }
    }

    public Vector2 RectMax
    {
        get { return (Vector2)transform.position + rectSize/2; }
    }

    public float Radius { get { return radius; } }

    public bool IsColliding { set { isColliding = value; } }

    public SpriteRenderer SRenderer { get { return sRenderer; } }

    public GameObject PowerPrefab { get { return powerPrefab; } }

    public bool IsEnemy { get { return isEnemy; } set { isEnemy = value; } }

    public bool IsEnemyBullet { get { return isEnemyBullet; } set { isEnemyBullet = value; } }

    public bool IsPower { get { return isPower; } set { isPower = value; } }

    // Methods --

    private void Start()
    {
        radius = (RectMax.x - RectMin.x) / 2;
       
        manager = GameObject.Find("Manager").GetComponent<CollisionManager>();
        player = GameObject.Find("Spaceship");

    }

    // Update is called once per frame
    void Update()
    {
        if(isColliding)
        {

            if(IsEnemy)
            {
                GameObject newEnemy=Instantiate(enemyPrefab);
                manager.Collidables.Add(newEnemy.GetComponent<SpriteInfo>());
                newEnemy.transform.position = new Vector3(Random.Range(-15, 15), Random.Range(0, 6));       
            }

            if(gameObject==player)
            {
                RemoveLife();
                GameObject power = Instantiate(powerPrefab);
                manager.Collidables.Add(power.GetComponent<SpriteInfo>());
                return;
            }
            
            SRenderer.color = Color.red;

            Destroy(gameObject);
        }   
    }

    private void OnDrawGizmos()
    {
        if(isColliding)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        
        //Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireCube(transform.position, rectSize);
    }

    private void RemoveLife()
    {
        if(GameObject.Find("Life 3").transform.position.x<98)
        {
            GameObject.Find("Life 3").transform.position=new Vector3(99,0,0);
            return;
        }
        else if(GameObject.Find("Life 2").transform.position.x < 98)
        {
            GameObject.Find("Life 2").transform.position = new Vector3(99, 0, 0);
            return;
        }
        else if(GameObject.Find("Life 1").transform.position.x < 98)
        {
            GameObject.Find("Life 1").transform.position = new Vector3(99, 0, 0);
            return;
        }

        Destroy(player);

        manager.Collidables.Clear();
        GameObject.Find("Game Over").transform.position = new Vector3(0, 1, 0);
        
    }
}
