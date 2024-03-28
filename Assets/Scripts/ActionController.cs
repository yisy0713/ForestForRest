using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;        // 획득 가능한 최대 거리

    private bool pickupActivated = false;           // 아이템 획득 가능할 시 true
    private bool dealActivated = false;             // npc와 상호작용 가능할 시 true
    private bool spawnActivated = false;

    private RaycastHit hitInfo;                     // 충돌체 정보 저장

    // 필요한 컴포넌트
    [SerializeField]
    private LayerMask ItemLayerMask;                // Item 레이어에만 반응하도록 레이어 마스크를 설정
    [SerializeField]
    private LayerMask NpcLayerMask;                 // Npc 레이어에만 반응하도록 레이어 마스크를 설정
    [SerializeField]
    private LayerMask EnemySpawnLayerMask;                 // Npc 레이어에만 반응하도록 레이어 마스크를 설정
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;

    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        CheckForward();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForward();
            CanInteraction();
        }
    }

    private void CheckForward()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, ItemLayerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, NpcLayerMask))
        {
            if (hitInfo.transform.tag == "Npc")
            {
                NpcController npc = hitInfo.transform.GetComponent<NpcController>();
                if (!npc.dealCompleted)
                {
                    NpcInfoAppear();
                }
            }
        }else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, EnemySpawnLayerMask))
        {
            if (hitInfo.transform.tag == "EnemySpawn" && !hitInfo.transform.GetComponent<EnemySpawner>().isSpawned)
            {
                EnemySpawnerInfoAppear();
            }
        }
        else
        {
            InfoDisappear();
        }
    }

    private void CanInteraction()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null && hitInfo.transform.tag == "Item")
            {
                //Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다 ");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }

        if (dealActivated)
        {
            if (hitInfo.transform != null && hitInfo.transform.tag == "Npc")
            {
                //Debug.Log(hitInfo.transform.name + "와 거래 했습니다. ");
                NpcController npc = hitInfo.transform.GetComponent<NpcController>();
                npc.Deal();
                InfoDisappear();
            }
        }

        if (spawnActivated)
        {
            if (hitInfo.transform != null && hitInfo.transform.tag == "EnemySpawn")
            {
                EnemySpawner spawner = hitInfo.transform.GetComponent<EnemySpawner>();
                spawner.EnemySpawn();
                spawner.particleActive();
                InfoDisappear();
            }
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = "<b><size=30>" + hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "</size></b>" + "\nPress " + "<color=yellow>" + "E" + "</color>" + " to Pick up ";
    }

    private void NpcInfoAppear()
    {
        dealActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = "<b><size=30>" + "Press " + "<color=yellow>" + "E" + "</color>" + " to get a reward " + "</size></b>";
    }

    private void EnemySpawnerInfoAppear()
    {
        spawnActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = "<b><size=30>" + "Press " + "<color=yellow>" + "E" + "</color>" + " to spawn Enemy " + "</size></b>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        dealActivated = false;
        spawnActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
