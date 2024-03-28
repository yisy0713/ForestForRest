using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemPickUp : MonoBehaviour
{
    public PassiveItem passiveItem;

    private PassiveItemManager passiveItemManager;

    // Start is called before the first frame update
    private void Start()
    {
        passiveItemManager = FindObjectOfType<PassiveItemManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 360f * Time.deltaTime * 0.1f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            passiveItemManager.AddPassiveItem(passiveItem);

            Destroy(gameObject);
        }
    }
}
