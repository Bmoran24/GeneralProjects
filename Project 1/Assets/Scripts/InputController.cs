using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField]
    MovementController movementController;

    [SerializeField]
    SpriteRenderer bulletPrefab;

    CollisionManager manager;

    List<SpriteRenderer> bullets = new List<SpriteRenderer>();

    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<CollisionManager>();
    }

    void Update()
    {
        for(int i=0; i<bullets.Count; i++)
        {
            if (bullets[i]==null)
            {
                continue;
            }

            Vector3 currentPosition = bullets[i].transform.position;

            bullets[i].transform.position = new Vector3(currentPosition.x, 
                currentPosition.y + .1f, currentPosition.z);

            if (currentPosition.y>6.5)
            {
                Destroy(bullets[i].gameObject);
                bullets.Remove(bullets[i]);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementController.SetDirection(context.ReadValue<Vector2>());
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SpriteRenderer bullet = Instantiate(bulletPrefab);
            Vector3 shipPos = GetComponentInParent<Transform>().position;

            bullet.transform.position = new Vector3(shipPos.x, shipPos.y + 1, shipPos.z);

            bullets.Add(bullet);

            manager.Collidables.Add(bullet.GetComponentInParent<SpriteInfo>());
        }    
        
    }

}
