using UnityEngine;
using Unity.Cinemachine;

public class CameraZoneSwitcher : MonoBehaviour
{
    // Arraste aqui a C�mera Virtual que esta zona deve ativar.
    [SerializeField] private CinemachineCamera virtualCamera;

    void Start()
    {
        // Garante que a c�mera come�a desativada para n�o haver conflitos.
        if (virtualCamera != null)
        {
            virtualCamera.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Quando o jogador entra na zona
        if (collision.CompareTag("Player")) // Certifique-se que seu jogador tem a tag "Player"
        {
            if (virtualCamera != null)
            {
                virtualCamera.enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Quando o jogador sai da zona
        if (collision.CompareTag("Player"))
        {
            if (virtualCamera != null)
            {
                virtualCamera.enabled = false;
            }
        }
    }
}