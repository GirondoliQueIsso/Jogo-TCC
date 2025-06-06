using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject balaoFala;
    [SerializeField] private GameObject pressPText;
    [SerializeField] private GameObject letterF;
    [SerializeField] private Animator balaoAnimator;

    private Rigidbody2D rb;
    private bool canJump = true;
    public int Bastao;
    private bool canTalk = false;
    private bool isBalaoOpen = false;
    private bool isAnimating = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (healthBar == null)
        {
            healthBar = FindFirstObjectByType<HealthBar>();
        }

        if (balaoFala != null)
        {
            balaoFala.SetActive(false);
        }

        if (letterF != null)
        {
            letterF.SetActive(false);
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleBalaoAnimation();
    }

    private void HandleMovement()
    {
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
    }

    private void HandleJump()
    {
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

    private void HandleBalaoAnimation()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.F) && !isAnimating)
        {
            if (balaoFala != null && balaoAnimator != null)
            {
                if (!isBalaoOpen)
                {
                    balaoFala.SetActive(true);
                    balaoAnimator.Play("AbrirBalao");
                    isBalaoOpen = true;
                    isAnimating = true;
                    StartCoroutine(ResetAnimationFlag());
                }
                else
                {
                    balaoAnimator.Play("FecharBalao");
                    isAnimating = true;
                    StartCoroutine(DeactivateAfterAnimation());
                }
            }
        }
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Hazard")
        {
            TakeDamage(10f);
        }
        else if (collision.gameObject.tag == "PlayerMid")
        {
            canTalk = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Hazard")
        {
            TakeDamage(27f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerMid")
        {
            canTalk = false;
            if (balaoFala != null && isBalaoOpen)
            {
                balaoAnimator.Play("FecharBalao");
                StartCoroutine(DeactivateAfterAnimation());
            }
        }
        else if (collision.gameObject.tag == "Kit")
        {
            if (pressPText != null)
            {
                pressPText.SetActive(false);
                letterF.SetActive(true);
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

                if (pressPText != null)
                {
                    pressPText.SetActive(false);
                }

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