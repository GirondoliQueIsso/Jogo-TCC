using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Cinemachine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [Header("Referências")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 1f;

    private CinemachineConfiner2D cameraConfiner;
    private PlayerController playerController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: mantém o gerente entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Encontra os componentes necessários na cena
        cameraConfiner = FindFirstObjectByType<CinemachineConfiner2D>();
        playerController = FindFirstObjectByType<PlayerController>();
    }

    public void StartRoomTransition(Transform playerTransform, Vector3 destination, PolygonCollider2D newConfiner)
    {
        StartCoroutine(TransitionCoroutine(playerTransform, destination, newConfiner));
    }

    private IEnumerator TransitionCoroutine(Transform playerTransform, Vector3 destination, PolygonCollider2D newConfiner)
    {
        // 1. Congela o jogador
        if (playerController != null) playerController.enabled = false;

        // 2. Fade para preto
        yield return Fade(1f); // Fade Out

        // 3. Teletransporte e troca do confiner
        playerTransform.position = destination;
        if (cameraConfiner != null && newConfiner != null)
        {
            cameraConfiner.BoundingShape2D = newConfiner;
        }

        // 4. Fade para transparente
        yield return Fade(0f); // Fade In

        // 5. Descongela o jogador
        if (playerController != null) playerController.enabled = true;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        Color currentColor = fadeImage.color;
        float startAlpha = currentColor.a;
        float elapsedTime = 0f;

        while (elapsedTime < 1f / fadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime * fadeSpeed);
            fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }
        fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }
}