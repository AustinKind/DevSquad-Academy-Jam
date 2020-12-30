using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //up, down, right, left
    [SerializeField] private Gun[] weaponCache;

    [SerializeField, Range(1, 4)]
    private int currentWeapon = 1;
    public delegate void WeaponChangeAction();
    public WeaponChangeAction onWeaponChange;
    int previousWeapon = 1;

    [Header("Weapon Wheel")]
    [SerializeField] private Animator wheelUI;
    [SerializeField] private AnimationCurve timeCurve;
    [SerializeField] private float timeAdjustSpeed = 4f;
    float timeScalerTimer = 0f;
    int activatedTimeScaler = -1;
    bool wheelOpen = false;


    Gun currentGun;
    Vector2 previousInput = Vector2.zero;
    Vector2 oldInput = Vector2.zero;
    bool semiShot = true;

    private void OnValidate()
    {
        if (weaponCache.Length > 4)
        {
            Debug.LogWarning("Do not go over 4 weapons!");
            Array.Resize(ref weaponCache, 4);
        }

        AdjustWeapon(currentWeapon);
    }

    void AdjustWeapon(int w)
    {
        currentWeapon = Mathf.Clamp(w, 1, weaponCache.Length);
        if(previousWeapon != currentWeapon)
        {
            Debug.Log("Weapon Changed!");
            if(onWeaponChange != null)
                onWeaponChange.Invoke();
            previousWeapon = currentWeapon;
        }
    }

    private void Start()
    {
        onWeaponChange = OnWeaponChange;
        OnWeaponChange();
    }

    private void OnWeaponChange()
    {
        for (int i = 0; i < weaponCache.Length; i++)
        {
            bool isCurrent = (i == currentWeapon - 1);
            Gun indexedGun = weaponCache[i];
            indexedGun.gameObject.SetActive(isCurrent);

            GrappleHook hook = null;
            if (!isCurrent && (hook = indexedGun as GrappleHook) != null)
                hook.Unhook(false);
        }

        currentGun = weaponCache[currentWeapon - 1];
    }

    public void ShootInput(Vector2 input)
    {
        Vector2 singleInput = GetNewestInput(input);
        if (!semiShot && oldInput != singleInput)
        {
            oldInput = previousInput;
            semiShot = true;
        }

        if (wheelOpen)
        {
            if (singleInput.y > 0) AdjustWeapon(1);
            else if (singleInput.y < 0) AdjustWeapon(2);
        }
        else if(singleInput.magnitude > 0)
        {
            bool fullAuto = (currentGun.gunShootType == GunType.auto);
            bool shoot = fullAuto ? true : semiShot;

            if (shoot)
            {
                if (!fullAuto && semiShot) semiShot = false;
                currentGun.Shoot(singleInput);
            }
        }
    }

    Vector2 GetNewestInput(Vector2 input)
    {
        if (input.magnitude > 1f)
        {
            if (previousInput == Vector2.zero) input.y = 0;
            else input -= previousInput;
        }
        else
            previousInput = input;

        return input;
    }

    public void OpenWeaponWheel(bool open)
    {
        if (wheelUI == null) return;
        wheelUI.SetBool("opened", wheelOpen = open);
        wheelUI.SetInteger("selectedWeapon", currentWeapon);

        if (activatedTimeScaler < 0) return;

        if(!open)
        {
            if (timeScalerTimer > 0)
            {
                TimeScaleManager.Instance.AdjustScaler(activatedTimeScaler, 1f);
                timeScalerTimer = 0f;
            }

            return;
        }

        timeScalerTimer = Mathf.Clamp(timeScalerTimer + Time.unscaledDeltaTime * timeAdjustSpeed, 0f, 1f);
        TimeScaleManager.Instance.AdjustScaler(activatedTimeScaler, timeCurve.Evaluate(timeScalerTimer));
    }

    public void ActivateTimeScaler(BoolAction checkBool)
    {
        TimeAdjuster adjuster = new TimeAdjuster(1f, checkBool);
        activatedTimeScaler = TimeScaleManager.Instance.AdjustTimeScale(adjuster);
    }
}
