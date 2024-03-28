using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{
    //public PassiveItem[] GetPassiveItems;

    private Dictionary<PassiveItem, int> getPassiveItems;

    [SerializeField]
    private PlayerController _playerController;
    [SerializeField]
    private StatusUI _playerStatus;
    [SerializeField]
    private Hand _handController;

    private bool getPassiveItem = false;


    // Start is called before the first frame update
    void Start()
    {
        getPassiveItems = new Dictionary<PassiveItem, int>();
    }

    // Update is called once per frame
    void Update()
    {
        //PassiveItemPowerUp();
    }

    private void PassiveItemPowerUp(PassiveItem _passiveItem)
    {
        if (getPassiveItem)
        {
            foreach (var pair in getPassiveItems)
            {
                switch (_passiveItem.PassiveItemName)
                {
                    case "Cherry":
                        //Debug.Log("Ã¼¸® ¸Ô¾ú¾î?");
                        _playerStatus.IncreaseMaxHp(10f);
                        getPassiveItem = false;
                        break;
                    case "Carrot":
                        _playerStatus.IncreaseHpRecover(5f);
                        getPassiveItem = false;
                        break;
                    case "Orange":
                        _playerStatus.IncreaseHungerDecreaseDelay(20f);
                        getPassiveItem = false;
                        break;
                    case "SweetPotato":
                        _playerController.IncreaseJumpForce(1f);
                        getPassiveItem = false;
                        break;
                    case "Banana":
                        _playerController.IncreaseMoveSpeed(2f);
                        getPassiveItem = false;
                        break;
                    case "Pea":
                        _playerController.DecreaseStemiaUse(3f);
                        getPassiveItem = false;
                        break;
                    case "Grape":
                        //_playerStatus.IncreaseHpRecover(10f);
                        getPassiveItem = false;
                        break;
                    case "Garlic":
                        //_playerStatus.IncreaseHpRecover(10f);
                        getPassiveItem = false;
                        break;
                    case "Lemon":
                        _playerStatus.IncreaseSpRecover(3f);
                        getPassiveItem = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void AddPassiveItem(PassiveItem _passiveItem)
    {
        getPassiveItem = true;
        if (getPassiveItems.ContainsKey(_passiveItem))
        {
            getPassiveItems[_passiveItem]++;
        }
        else
        {
            getPassiveItems.Add(_passiveItem, 1);
        }


        foreach (var pair in getPassiveItems)
        {
            Debug.Log(pair.Key + ": " + pair.Value);
        }

        PassiveItemPowerUp(_passiveItem);
    }
}
