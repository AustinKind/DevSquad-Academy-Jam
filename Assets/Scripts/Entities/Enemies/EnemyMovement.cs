using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMovementStatus { patrol, idle, follow };

public class EnemyMovement : RigidbodyMovement
{
    public EnemyMovementStatus status;

}
