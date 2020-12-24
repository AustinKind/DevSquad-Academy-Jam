using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerInput : MonoBehaviour
{
    public Vector2 Movement => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    public bool Jump => Input.GetButtonDown("Jump");
    public bool Shoot => Input.GetButtonDown("Fire");

}
