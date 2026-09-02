using UnityEngine;

public class TopPlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject[] ingredients;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private GameObject spawn;

    private int itemHeld = -1;
    private GameObject currentIngredient;
    private Vector2 spawnPos;

    private void Update()
    {
        spawnPos = spawn.transform.position;
        
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        if (cooldown < 0)
        {
            cooldown = 0;
        }

        Inputs();
    }

    private void Inputs()
    {
        if (Input.GetMouseButtonDown(0) && itemHeld == -1 && cooldown <= 0)
        {
            GrabItem();
        }

        if (Input.GetMouseButtonDown(1) && itemHeld != -1)
        {
            ItemDrop();
        }
    }

    private void GrabItem()
    {
        if (currentIngredient == null)
        {
            return;
        }

        if (currentIngredient.CompareTag("Tomato"))
        {
            itemHeld = 0;
        }
        else if (currentIngredient.CompareTag("Cheese"))
        {
            itemHeld = 1;
        }
        else if (currentIngredient.CompareTag("Shrooms"))
        {
            itemHeld = 2;
        }
        else if (currentIngredient.CompareTag("Pepperoni"))
        {
            itemHeld = 3;
        }

        cooldown = 1f;
    }

    private void ItemDrop()
    {
        Instantiate(ingredients[itemHeld], spawnPos, Quaternion.identity);

        itemHeld = -1;
        cooldown = 1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tomato") || other.CompareTag("Cheese") || other.CompareTag("Shrooms") || other.CompareTag("Pepperoni"))
        {
            currentIngredient = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == currentIngredient)
        {
            currentIngredient = null;
        }
    }
}