using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerLadderMovement : PlayerMovementType
{
    [SerializeField] private Vector2 climbSpeed = new Vector2(4f, 2.5f);

    float ladderXPos = 0f;


    public bool CanClimbLadder
    {
        get
        {
            foreach (LadderHelper ladder in currentLadders)
            {
                if (ladder.HasPlayerInside)
                {
                    ladderXPos = ladder.transform.position.x;
                    return true;
                }
            }
            return false;
        }
    }
    private List<LadderHelper> currentLadders = new List<LadderHelper>();

    public bool AddLadder(LadderHelper helper)
    {
        if (!currentLadders.Contains(helper))
        {
            currentLadders.Add(helper);
            return true;
        }
        else
            return false;
    }

    public override bool ShouldUseMovement()
    {
        return CanClimbLadder;
    }

    public override void Movement(Vector2 input, ref Vector2 moveDirection, bool grounded)
    {
        moveDirection.y = input.y * climbSpeed.y;
        if (!grounded)
        {
            if (Mathf.Abs(input.x) < 0.02f)
                moveDirection.x = (ladderXPos - playerMovement.transform.position.x);
            else
                moveDirection.x = input.x;
            moveDirection.x *= climbSpeed.x;
        }
        else
            moveDirection.x = input.x * playerMovement.MoveSpeed;
    }

    public override void FixedMovement(ref Vector2 moveDirection, bool grounded)
    {
    }

    public override void Jump(ref Vector2 moveDirection, bool grounded)
    {
    }
}
