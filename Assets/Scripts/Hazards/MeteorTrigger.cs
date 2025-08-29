using UnityEngine;

public class MeteorTrigger : BaseTrigger
{
    [Header("Meteor Settings")]
    [SerializeField] private GameObject meteorShadowPrefab;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private float horizontalOffset = 2f;
    [SerializeField] private float meteorStartHeight = 50f;

    [SerializeField] private AudioManager audioManager;

    protected override void OnTriggered()
    {
        audioManager = FindObjectOfType<AudioManager>();
        StartMeteorStrike();
        audioManager.PlaySFX("Meteor Fall");
    }

    private void StartMeteorStrike()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Transform gameplayScreen = transform.parent;

        if (player != null)
        {
            Vector3 targetPosition = player.position + Vector3.right * horizontalOffset;

            Vector3 shadowPosition = new Vector3(targetPosition.x, -8.5342f, targetPosition.z);
            GameObject shadow = Instantiate(meteorShadowPrefab, shadowPosition, Quaternion.identity);


            Vector3 meteorSpawnPos = new Vector3(targetPosition.x, meteorStartHeight, targetPosition.z);
            GameObject meteor = Instantiate(meteorPrefab, meteorSpawnPos, Quaternion.identity);

            if (gameplayScreen != null)
            {
                shadow.transform.SetParent(gameplayScreen);
                meteor.transform.SetParent(gameplayScreen);
            }

            MeteorStrikeHandler meteorStrike = meteor.GetComponent<MeteorStrikeHandler>();
            if (meteorStrike != null)
            {
                meteorStrike.Initialize(shadow, targetPosition);
            }
        }
    }
}