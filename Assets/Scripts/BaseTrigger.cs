using UnityEngine;

public abstract class BaseTrigger : MonoBehaviour
{
    public enum TriggerEndAction { None, Disable, Destroy };
    [SerializeField] protected TriggerEndAction action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanTrigger(collision))
        {
            OnTriggered();

            if (action == TriggerEndAction.Disable)
                gameObject.SetActive(false);
            else if (action == TriggerEndAction.Destroy)
                Destroy(gameObject);
        }
    }

    protected virtual bool CanTrigger(Collider2D collider)
    {
        return collider.CompareTag("Player");
    }

    protected abstract void OnTriggered();
}
