using UnityEngine;
using System.Collections;
using Cinemachine;

public class BiscuitRewardManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerController player;
    [SerializeField] private CinemachineVirtualCamera cutsceneCamera;
    [SerializeField] private CinemachineVirtualCamera cookieCamera;

    [SerializeField] private GameObject distanceText;
    [SerializeField] private GameObject inventorySlots;

    public IEnumerator PlayCutscene(string rank)
    {
        inventorySlots.SetActive(false);
        distanceText.SetActive(false);
        gameManager.SetCutsceneState(true);

        switch (rank)
        {
            case "F": yield return StartCoroutine(PlayRankF()); break;
        }

        cutsceneCamera.Priority = 0;

        gameManager.SetCutsceneState(false);
    }

    private IEnumerator PlayRankF()
    {
        
        
        Vector3 spawnPos = player.transform.position + new Vector3(3f, 5f, 0f);
        GameObject biscuit = Instantiate(Resources.Load<GameObject>("Prefabs/Biscuit"), spawnPos, Quaternion.identity);

        cookieCamera.gameObject.SetActive(true);
        cookieCamera.Follow = biscuit.transform;

        yield return new WaitForSeconds(3f);

        cookieCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);

        player.SetSadExpression(true);

        yield return new WaitForSeconds(5f);
        cutsceneCamera.gameObject.SetActive(false);
    }
}
