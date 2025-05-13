using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private HealthBar healthBar;

    private Rigidbody2D rb;
    private bool canJump = true;
    public int item;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Se a healthBar não estiver atribuída no Inspector, tenta encontrar automaticamente
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<HealthBar>();
        }
    }

    void Update()
    {
        // Movimento horizontal
        if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        // Verifica se pode pular
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        // Pulo com W
        if (Input.GetKeyDown(KeyCode.W) && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Destroy(collision.gameObject);
            item++;
        }
        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Hazard")
        {
            TakeDamage(10f); // Valor de dano ajustável
        }
    }

    public void TakeDamage(float damage)
    {
        if (healthBar != null)
        {
            healthBar.TakeDamage(damage);
        }
    }
}