using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class WeaponManager
{
   public BaseWeapon Weapon { get; private set; }
   public Action<GameObject> unRegisterWeapon { get; set; }
   private Transform handPostion;
   private GameObject weaponObject;
   private GameObject equipweapon;
   public RuntimeAnimatorController BaseAnimator;

   public Item item;
   
   public WeaponManager(Transform hand)
   {
      handPostion = hand;
   }

   public void RegisterWeapon(GameObject weapon)
   {
      
      BaseWeapon weaponInfo = weapon.GetComponent<BaseWeapon>();
      weapon.transform.SetParent(handPostion);
      weapon.transform.localPosition = weaponInfo.HandleData.localPosition;
      weapon.transform.localEulerAngles = weaponInfo.HandleData.localRotation;
      weapon.transform.localScale = weaponInfo.HandleData.localScale;
      
      equipweapon = weapon;
      weapon.SetActive(false);
      
   }

   public void UnRegisterWeapon()
   {
      if (weaponObject != null)
      {
         Object.Destroy(weaponObject);
      }

      Player.Instance.animator.runtimeAnimatorController = Player.Instance.BaseAnimator;
   }

   public void SetWeapon(GameObject weapon)
   {
      
      if (weaponObject != null)
      {
         UnRegisterWeapon();
      }

      RegisterWeapon(weapon);
      weaponObject = weapon;
      Weapon = weapon.GetComponent<BaseWeapon>();
      weaponObject.SetActive(true);
      Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
   }
   
   // [Button]
   // public void DEBUG_SETWEAPON()
   // {
   //    if (Weapon != null)
   //    {
   //       UnRegisterWeapon();
   //    }
   //
   //    RegisterWeapon(testData.weapon_gameObject);
   //    weaponObject = testData.weapon_gameObject;
   //    Weapon = testData.weapon_gameObject.GetComponent<BaseWeapon>();
   //    weaponObject.SetActive(true);
   //    Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
   // }
}
