using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombHandler : MonoBehaviour
{
    [SerializeField] private Transform buttonHelper;
    [SerializeField] private Vector2 buttonOffset = Vector2.up;

    List<Bomb> bombs;

    private void Start()
    {
    }

    public void AddBomb(Bomb b)
    {
        if(bombs == null)
            ResetBombs();
        bombs.Add(b);
    }

    public void ResetBombs()
    {
        bombs = new List<Bomb>();
    }

    public void ResetInputHelper()
    {
        buttonHelper.SetParent(transform);
        buttonHelper.localPosition = Vector2.zero;
    }

    public bool SendDefuseToBombs()
    {
        Bomb curBomb = null;
        if ((curBomb = GetDefuseableBomb) != null)
            curBomb.DefuseBomb();
        return CompletedLevel;
    }

    Bomb GetDefuseableBomb
    {
        get {
            if (bombs == null) return null;

            foreach (Bomb b in bombs)
            {
                if (b.CanDefuse) return b;
            }

            return null;
        }
    }

    private void Update()
    {
        Bomb curBomb = GetDefuseableBomb;
        bool canDefuse = (curBomb != null);
        if (canDefuse)
        {
            buttonHelper.SetParent(curBomb.transform);
            buttonHelper.localPosition = buttonOffset;
        }
        else ResetInputHelper();

        buttonHelper.gameObject.SetActive(canDefuse);
    }

    public bool CompletedLevel
    {
        get
        {
            foreach (Bomb bomb in bombs)
            {
                if (!bomb.Defused) 
                    return false;
            }

            Debug.Log("All bombs defused!");
            return true;
        }
    }
}
