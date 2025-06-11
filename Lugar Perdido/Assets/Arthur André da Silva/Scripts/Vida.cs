using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private GameObject Morte;

    [Header("Visual Feedback")]
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;
    [SerializeField] private Image fillImage;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = currentHealth / maxHealth;
        fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, currentHealth / maxHealth);
    }

    private void Die()
    {
        // Desativa o controle do jogador ou executa animação de morte
        Debug.Log("Você morreu!");

        // Exemplo: desativar o script do jogador
        if (healthSlider != null)
        {
            Morte.SetActive(true);
        }
        // LINHA NOVA E CORRIGIDA
        FindFirstObjectByType<PlayerController>().enabled = false;
    }
}
