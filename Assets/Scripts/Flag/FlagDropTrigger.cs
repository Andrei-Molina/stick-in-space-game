using UnityEngine;

public class FlagDropTrigger : KeyTrigger
{
    [SerializeField] private Transform player;
    [SerializeField] private FlagDropper flagDropper;
    [SerializeField] private float dropY = -5.2313f;
    [SerializeField] private GameManager gameManager;

    public override void TriggerEffect()
    {
        if (player == null || flagDropper == null)
        {
            Debug.LogError("FlagDropTrigger: Missing references!");
            return;
        }

        Vector3 dropPosition = new Vector3(player.position.x, dropY, 0f);
        flagDropper.DropFlag(dropPosition);

        gameManager.Die("FlagDrop");
    }
}
