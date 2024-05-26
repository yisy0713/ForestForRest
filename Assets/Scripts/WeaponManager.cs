using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon = false;  // 무기 중복 교체 방지

    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    [SerializeField]
    private string currentWeaponType;

    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;

    [SerializeField]
    private Hand[] meleeWeapons;
    [SerializeField]
    private Hand[] rangedWeapons;

    [SerializeField]
    private HandController meleeWeaponController;
    [SerializeField]
    private HandController rangedWeaponController;

    private Dictionary<string, Hand> meleeWeaponDictionary = new Dictionary<string, Hand>();
    private Dictionary<string, Hand> rangedWeaponDictionary = new Dictionary<string, Hand>();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i< meleeWeapons.Length; i++)
        {
            meleeWeaponDictionary.Add(meleeWeapons[i].handName, meleeWeapons[i]);
        }
        for (int i = 0; i < rangedWeapons.Length; i++)
        {
            rangedWeaponDictionary.Add(rangedWeapons[i].handName, rangedWeapons[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("1누름!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                StartCoroutine(ChangeWeaponCoroutine("meleeWeapon", "Rock"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(ChangeWeaponCoroutine("meleeWeapon", "Rocket"));
            }
                
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("먼가");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CanclePreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void CanclePreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "meleeWeapon":
                break;
            case "rangedWeapon":
                break;
        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if (_type == "meleeWeapon")
        {
            meleeWeaponController.HandChange(meleeWeaponDictionary[_name]);
        }
        else if(_type == "rangedWeapon")
        {
            rangedWeaponController.HandChange(rangedWeaponDictionary[_name]);
        }

    }
}
