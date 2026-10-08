using UnityEngine;

public class GoalPost : MonoBehaviour
{
    public System.Action OnGoalEvent;

    private const int LAYER_PLAYER = 7;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤー以外は何もしない
        if (collision.gameObject.layer != LAYER_PLAYER) return;

        OnGoalEvent?.Invoke();
    }
}
