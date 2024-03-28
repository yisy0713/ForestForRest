using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    ParticleSystem npcParticle;
    [SerializeField]
    private Inventory theInventory;
    [SerializeField]
    private Item trashItem;
    [SerializeField]
    private GameObject[] PassiveItem;


    public bool dealCompleted = false;

    private int trashCountToDeal = 3;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        theInventory = FindObjectOfType<Inventory>();
        animator.SetBool("success1", false);
        animator.SetBool("success2", false);
    }

    public void Deal()
    {
        if (!dealCompleted)
        {
            if(trashCountToDeal <= theInventory.CountItem(trashItem))
            {
                Reward();
                theInventory.DecreaseItemCount(trashItem, -trashCountToDeal);
                animator.SetTrigger("success1");
                animator.SetTrigger("success2");
                npcParticle.Play();
                dealCompleted = true;
            }
        }
    }

    private void Reward()
    {        
        GameObject obj = PassiveItem[Random.Range(0, PassiveItem.Length)];         
        Vector3 dropPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);//_transform.position;

        Instantiate(obj, dropPos, Quaternion.identity);
    }

}
