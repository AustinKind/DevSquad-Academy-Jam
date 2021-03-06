﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerInput : MonoBehaviour
{
    public Vector2 Movement => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    public Vector2 Shoot => new Vector2(Input.GetAxisRaw("HorizontalShoot"), Input.GetAxisRaw("VerticalShoot"));
    public bool Jump => Input.GetButtonDown("Jump");
    public bool SelectWeapon => Input.GetButton("SelectWeapon");
    public bool Defuse => Input.GetButtonDown("Defuse");
}
