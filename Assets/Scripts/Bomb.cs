using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] public Sprite[] bombStates;
    public bool isActivated = true;
    private bool isTriggered = false;
    private BombController controller;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            isTriggered = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            isTriggered = false;
    }

    void Start ()
    {
        controller = transform.parent.gameObject.GetComponent<BombController>();
    }

    void Update()
    {
        if (isTriggered && Input.GetKeyDown(KeyCode.E) == true)
        {
            isActivated = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = bombStates[1];
            controller.CheckCompletion();
        }
    }
}
