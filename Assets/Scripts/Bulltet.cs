using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulltet : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 5f;
    public float rotationSpeed = 200f;

    private Transform player;
    private StatusUI playerStatus;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStatus = Object.FindObjectOfType<StatusUI>();
    }

    private void Update()
    {
        if (player != null)
        {
            // �÷��̾ ���� ȸ��
            Vector3 direction = (player.position - transform.position).normalized;
            float rotateStep = rotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotateStep, 0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            // �÷��̾ ���� �̵�
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ����!");
            playerStatus.DecreaseHp(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Hill"))
        {
            Destroy(gameObject, 3f);
        }
    }
}
