using UnityEngine;

[CreateAssetMenu(menuName = "GameEvents/MeteorStrike")]
public class MeteorStrikeEvent : GameEvent
{
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private float horizontalOffset = 2.5f;
    [SerializeField] private float spawnHeight = 8f;
    [SerializeField] private float fallSpeed = 10f;
    [SerializeField] private float warningTime = 1.5f;

    public override void Trigger(GameManager gameManager, PlayerController player)
    {
        if (meteorPrefab == null || shadowPrefab == null)
        {
            Debug.LogError("MeteorStrikeEvent: Prefabs missing!");
            return;
        }

        Vector3 strikePos = new Vector3(
            player.transform.position.x + horizontalOffset,
            -8.5342f,
            player.transform.position.z
        );

        GameObject shadow = Instantiate(shadowPrefab, strikePos, Quaternion.identity);

        ShadowController shadowCtrl = shadow.GetComponent<ShadowController>();
        if (shadowCtrl != null)
        {
            shadowCtrl.Initialize(
                new Vector3(103.6346f, 40.80102f, 40.80102f),  
                Quaternion.Euler(105.635f, 0f, 0f),             
                warningTime                                     
            );
        }

        Vector3 meteorPos = new Vector3(
            strikePos.x,
            strikePos.y + spawnHeight,
            strikePos.z
        );

        GameObject meteor = Instantiate(meteorPrefab, meteorPos, Quaternion.identity);
        meteor.SetActive(true);

        // Hand off control to Meteor script
        Meteor meteorScript = meteor.GetComponent<Meteor>();
        if (meteorScript != null)
        {
            meteorScript.Initialize(fallSpeed, gameManager, shadow, warningTime);
        }
    }
}
