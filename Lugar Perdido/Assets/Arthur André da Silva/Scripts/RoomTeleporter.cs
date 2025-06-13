using UnityEngine;

public class RoomTeleporter : MonoBehaviour
{
    [Header("Destino")]
    [Tooltip("O ponto para onde o jogador será teletransportado.")]
    public Transform destinationPoint;

    [Header("Nova Configuração da Câmera")]
    [Tooltip("O collider que definirá os limites da nova sala.")]
    public PolygonCollider2D newConfinerShape;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Desativa o collider da porta para evitar múltiplas chamadas
            GetComponent<Collider2D>().enabled = false;

            // Chama o Gerente de Transição para iniciar o processo
            TransitionManager.Instance.StartRoomTransition(collision.transform, destinationPoint.position, newConfinerShape);
        }
    }
}