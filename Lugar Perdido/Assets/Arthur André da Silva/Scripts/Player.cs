using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject balaoFala; // Referência para o balão de fala
    [SerializeField] private GameObject pressPText; // Referência para o texto "PRESSIONE P"
    [SerializeField] private GameObject letterF; // Referência para a letra "F"

    private Rigidbody2D rb;
    private bool canJump = true;
    public int Bastao;
    private bool canTalk = false; // Pode interagir com outro player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Se a healthBar não estiver atribuída no Inspector, tenta encontrar automaticamente
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<HealthBar>();
        }

        // Esconde o balão de fala no início
        if (balaoFala != null)
        {
            balaoFala.SetActive(false);
        }

        // A letra "F" fica oculta no início
        if (letterF != null)
        {
            letterF.SetActive(false);
        }

        // O texto "PRESSIONE P" permanece visível (não desativamos aqui)
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

        // Interação com F (mostrar/ocultar balão de fala)
        if (canTalk && Input.GetKeyDown(KeyCode.F))
        {
            if (balaoFala != null)
            {
                balaoFala.SetActive(!balaoFala.activeSelf);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Hazard")
        {
            TakeDamage(10f); // Valor de dano ajustável
        }
        else if (collision.gameObject.tag == "PlayerMid") // Verifica se é o player do meio
        {
            canTalk = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Hazard")
        {
            TakeDamage(10f); // Valor de dano ajustável
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerMid") // Saiu do alcance do player do meio
        {
            canTalk = false;
            if (balaoFala != null)
            {
                balaoFala.SetActive(false); // Garante que o balão some ao sair
            }
        }
        else if (collision.gameObject.tag == "Kit") // Saiu da área do kit
        {
            // Esconde o texto "PRESSIONE P" quando sai do kit
            if (pressPText != null)
            {
                pressPText.SetActive(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Kit")
        {
            if (Input.GetKey(KeyCode.P))
            {
                Destroy(collision.gameObject);
                Bastao++;

                // Esconde o texto "PRESSIONE P" após coletar
                if (pressPText != null)
                {
                    pressPText.SetActive(false);
                }

                // Mostra a letra "F" após coletar o item
                if (letterF != null)
                {
                    letterF.SetActive(true);
                }
            }
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