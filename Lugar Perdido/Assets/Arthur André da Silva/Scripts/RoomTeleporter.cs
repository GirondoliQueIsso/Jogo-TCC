using UnityEngine;

public class RoomTeleporter : MonoBehaviour
{
    [Header("Destino")]
    [Tooltip("O ponto para onde o jogador ser� teletransportado.")]
    public Transform destinationPoint;

    [Header("Nova Configura��o da C�mera")]
    [Tooltip("O collider que definir� os limites da nova sala.")]
    public PolygonCollider2D newConfinerShape;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Desativa o collider da porta para evitar m�ltiplas chamadas
            GetComponent<Collider2D>().enabled = false;

            // Chama o Gerente de Transi��o para iniciar o processo
            TransitionManager.Instance.StartRoomTransition(collision.transform, destinationPoint.position, newConfinerShape);
        }
    }
}