using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmsController : MonoBehaviour
{
    [SerializeField] private Transform armParent;
    [SerializeField] private SpriteRenderer[] characterArms; //normal front = 0, back = 1
    [SerializeField] private Transform[] armRigs; //normal front = 0, back = 1

    SpriteRenderer characterRend;
    Sprite frontHand, backHand;
    bool flipped = false;

    bool armsSet = false;
    bool ArmsSetCorrectly
    {
        get
        {
            for (int i = 0; i < characterArms.Length; i++)
            {
                if(characterArms[i] == null)
                    return false;
                if (armRigs[i] == null)
                    return false;
            }
            return true;
        }
    }
    
    private void OnValidate()
    {
        if (characterArms.Length != 2 || armRigs.Length != 2)
        {
            Debug.LogWarning("Character Arms / Rigs should only ever have 2!");
            System.Array.Resize(ref characterArms, 2);
            System.Array.Resize(ref armRigs, 2);
        }
    }

    private void Start()
    {
        GetRequiredComponents();
    }

    void GetRequiredComponents()
    {
        characterRend = GetComponent<SpriteRenderer>();
        GetSpritesFromArms();
    }

    void GetSpritesFromArms()
    {
        if (!(armsSet = ArmsSetCorrectly)) return;
        frontHand = characterArms[0].sprite;
        backHand = characterArms[1].sprite;
    }

    private void Update()
    {
        if (!armsSet) return;
        if (flipped != characterRend.flipX)
        {
            flipped = characterRend.flipX;
            characterArms[0].sortingOrder = (!flipped) ? 2 : 0;
            characterArms[0].sprite = (!flipped) ? frontHand : backHand;

            characterArms[1].sortingOrder = (!flipped) ? 0 : 2;
            characterArms[1].sprite = (!flipped) ? backHand : frontHand;

            armParent.localScale = new Vector3((!flipped) ? 1 : -1, 1, 1);
            /*
            characterArms[0].transform.SetParent((!flipped) ? armRigs[0] : armRigs[1]);
            characterArms[1].transform.SetParent((!flipped) ? armRigs[1] : armRigs[0]);
            characterArms[0].transform.localPosition = Vector3.zero;
            characterArms[1].transform.localPosition = Vector3.zero;*/
        }
    }
}
