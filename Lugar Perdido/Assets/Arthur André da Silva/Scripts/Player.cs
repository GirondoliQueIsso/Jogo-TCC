using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("References")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject balaoFala;
    [SerializeField] private GameObject pressPText; // Texto para pegar o kit
    [SerializeField] private GameObject letterF;     // Texto para falar com NPC
    [SerializeField] private Animator balaoAnimator;

    // Componentes e Vari�veis Internas
    private Rigidbody2D rb;
    private bool canJump = true;
    public int Bastao;

    // Vari�veis de estado para intera��es
    private bool canTalk = false;       // Flag para saber se pode conversar
    private bool canPickUpKit = false;  // Flag para saber se pode pegar o kit
    private GameObject kitToPickUp;     // Armazena a refer�ncia do kit pr�ximo

    // Controle de Anima��o do Bal�o
    private bool isBalaoOpen = false;
    private bool isAnimating = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Busca autom�tica de refer�ncias se n�o foram setadas no Inspector
        if (healthBar == null)
        {
            healthBar = FindFirstObjectByType<HealthBar>();
        }

        // Desativa elementos da UI no in�cio
        if (balaoFala != null) balaoFala.SetActive(false);
        if (pressPText != null) pressPText.SetActive(false);
        if (letterF != null) letterF.SetActive(false);
    }

    void Update()
    {
        // Organiza as chamadas de fun��o no Update
        HandleMovement();
        HandleJump();
        HandleInteraction(); // Nova fun��o que agrupa as intera��es com F e P
    }

    // --- MOVIMENTA��O ---

    private void HandleMovement()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
        }
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        // Uma forma mais confi�vel de checar o ch�o � usando um Raycast ou um CircleCast,
        // mas para um jogo simples, checar a velocidade vertical funciona.
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        if (Input.GetKeyDown(KeyCode.W) && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // --- INTERA��ES ---

    private void HandleInteraction()
    {
        // L�gica para falar (Tecla F)
        if (canTalk && Input.GetKeyDown(KeyCode.F) && !isAnimating)
        {
            ToggleBalaoFala();
        }

        // L�gica para pegar o Kit (Tecla P)
        if (canPickUpKit && Input.GetKeyDown(KeyCode.P))
        {
            PickUpKit();
        }
    }

    private void PickUpKit()
    {
        if (kitToPickUp != null)
        {
            Destroy(kitToPickUp);
            Bastao++;

            if (pressPText != null) pressPText.SetActive(false);

            // Voc� pode decidir se quer mostrar o "F" para outra coisa aqui
            // if (letterF != null) letterF.SetActive(true);

            // Resetar flags para n�o tentar pegar de novo
            canPickUpKit = false;
            kitToPickUp = null;
        }
    }

    private void ToggleBalaoFala()
    {
        if (balaoFala == null || balaoAnimator == null) return;

        isAnimating = true;
        if (!isBalaoOpen)
        {
            balaoFala.SetActive(true);
            balaoAnimator.Play("AbrirBalao");
            isBalaoOpen = true;
            StartCoroutine(ResetAnimationFlag());
        }
        else
        {
            balaoAnimator.Play("FecharBalao");
            StartCoroutine(DeactivateAfterAnimation());
        }
    }

    // --- COROUTINES PARA ANIMA��O DO BAL�O ---

    private IEnumerator ResetAnimationFlag()
    {
        yield return new WaitForSeconds(balaoAnimator.GetCurrentAnimatorStateInfo(0).length);
        isAnimating = false;
    }

    private IEnumerator DeactivateAfterAnimation()
    {
        yield return new WaitForSeconds(balaoAnimator.GetCurrentAnimatorStateInfo(0).length);
        balaoFala.SetActive(false);
        isBalaoOpen = false;
        isAnimating = false;
    }

    // --- F�SICA E COLIS�ES ---

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hazard"))
        {
            TakeDamage(27f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Hazard"))
        {
            TakeDamage(10f);
        }
        else if (other.CompareTag("PlayerMid")) // NPC
        {
            canTalk = true;
            if (letterF != null) letterF.SetActive(true);
        }
        else if (other.CompareTag("Kit"))
        {
            canPickUpKit = true;
            kitToPickUp = other.gameObject;
            if (pressPText != null) pressPText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMid")) // NPC
        {
            canTalk = false;
            if (letterF != null) letterF.SetActive(false);
            if (isBalaoOpen)
            {
                ToggleBalaoFala(); // Fecha o bal�o se o jogador se afastar
            }
        }
        else if (other.CompareTag("Kit"))
        {
            canPickUpKit = false;
            kitToPickUp = null;
            if (pressPText != null) pressPText.SetActive(false);
        }
    }

    // --- FUN��ES P�BLICAS ---

    public void TakeDamage(float damage)
    {
        if (healthBar != null)
        {
            healthBar.TakeDamage(damage);
        }
    }
}