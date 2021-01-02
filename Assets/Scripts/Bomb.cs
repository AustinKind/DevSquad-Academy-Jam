using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private bool isActivated = true;
    private bool isTriggered = false;

    public bool CanDefuse => (isActivated && isTriggered);
    public bool Defused => (!isActivated);

    bool addedToBombList = false;
    private AudioController audioController;
    private Animator anim;

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
        audioController = AudioController.Instance;
        anim = GetComponent<Animator>();
        addedToBombList = false;

        isActivated = true;
        isTriggered = false;
    }

    private void Update()
    {
        if (!addedToBombList) TryToAddBomb();
    }

    void TryToAddBomb()
    {
        PlayerBombHandler bombHandler = null;
        if((bombHandler = FindObjectOfType<PlayerBombHandler>()) != null)
        {
            bombHandler.AddBomb(this);
            addedToBombList = true;
        }
    }

    public void DefuseBomb()
    {
        isActivated = false;
        anim.SetTrigger("Defused");
        audioController.PlaySound("deactivate");
    }

}
