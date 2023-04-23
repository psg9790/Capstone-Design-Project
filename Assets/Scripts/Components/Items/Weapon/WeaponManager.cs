using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
   public BaseWeapon Weapon { get; private set; }
   public Action<GameObject> unRegisterWeapon { get; set; }
   private Transform handPostion;
   private GameObject weaponObject;
   private GameObject equipweapon;

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
         Destroy(weaponObject);
      }
   }

   public void SetWeapon(GameObject weapon)
   {
      
      if (Weapon != null)
      {
         UnRegisterWeapon();
      }

      RegisterWeapon(weapon);
      weaponObject = weapon;
      Weapon = weapon.GetComponent<BaseWeapon>();
      weaponObject.SetActive(true);
      Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
      

      
   }
}
