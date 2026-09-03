using UnityEngine;

public class TopPlayer : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float springStrength = 25f;
    [SerializeField] private float damping = 4f;

    [SerializeField] private GameObject[] ingredients;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private GameObject spawn;
    [SerializeField] private float maxThrowForce = 10f;
    [SerializeField] private float throwChargeSpeed = 10f;

    private int itemHeld = -1;
    private GameObject currentIngredient;
    private Vector2 spawnPos;
    private float throwForce;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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

    private void FixedUpdate()
    {
        if (rb != null)
        {
            FollowMouseX();
        }
    }

    private void Inputs()
    {
        if (Input.GetMouseButtonDown(0) && itemHeld == -1 && cooldown <= 0)
        {
            GrabItem();
        }
        
        if (Input.GetMouseButtonDown(1) && itemHeld != -1 && cooldown <= 0)
        {
            throwForce = 0f;
        }
        
        if (Input.GetMouseButton(1) && itemHeld != -1 && cooldown <= 0)
        {
            throwForce += throwChargeSpeed * Time.deltaTime;

            if (throwForce > maxThrowForce)
            {
                throwForce = maxThrowForce;
            }
        }
        
        if (Input.GetMouseButtonUp(1) && itemHeld != -1 && cooldown <= 0)
        {
            ItemDrop();
        }
    }

    private void FollowMouseX()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float targetX = mousePosition.x;
        float currentX = rb.position.x;
        
        float distance = targetX - currentX;
        
        float springForce = distance * springStrength;
        
        float dampingForce = -rb.linearVelocity.x * damping;
        
        float totalForce = springForce + dampingForce;

        rb.AddForce(Vector2.right * totalForce);
        
        float clampedVelocityX = Mathf.Clamp(rb.linearVelocity.x, -maxSpeed, maxSpeed);

        rb.linearVelocity = new Vector2(clampedVelocityX, rb.linearVelocity.y);
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
        GameObject thrownIngredient = Instantiate(ingredients[itemHeld], spawnPos, Quaternion.identity );

        Rigidbody2D ingredientRb = thrownIngredient.GetComponent<Rigidbody2D>();

        if (ingredientRb != null)
        {
            ingredientRb.AddForce(Vector2.up * throwForce, ForceMode2D.Impulse);
        }

        itemHeld = -1;
        throwForce = 0f;
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