using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager
{
   public BaseWeapon Weapon { get; private set; }
   public Action<GameObject> unRegisterWeapon { get; set; }
   private Transform handPostion;
   private GameObject weaponObject;
   private List<GameObject> weapons = new List<GameObject>();

   public WeaponManager(Transform hand)
   {
      handPostion = hand;
   }

   public void RegisterWeapon(GameObject weapon)
   {
      if (!weapons.Contains(weapon))
      {
         BaseWeapon weaponInfo = weapon.GetComponent<BaseWeapon>();
         weapon.transform.SetParent(handPostion);
         weapon.transform.localPosition = weaponInfo.HandleData.localPosition;
         weapon.transform.localEulerAngles = weaponInfo.HandleData.localRotation;
         weapon.transform.localScale = weaponInfo.HandleData.localScale;
         weapons.Add(weapon);
         weapon.SetActive(false);
      }
   }

   public void UnRegisterWeapon(GameObject weapon)
   {
      if (weapons.Contains(weapon))
      {
         weapons.Remove(weapon);
         unRegisterWeapon.Invoke(weapon);
      }
   }

   public void SetWeapon(GameObject weapon)
   {
      if (Weapon == null)
      {
         weaponObject = weapon;
         Weapon = weapon.GetComponent<BaseWeapon>();
         weaponObject.SetActive(true);
         Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
         return;
      }

      for (int i = 0; i < weapons.Count; i++)
      {
         if (weapons[i].Equals(Weapon))
         {
            weaponObject = weapon;
            weaponObject.SetActive(true);
            Weapon = weapon.GetComponent<BaseWeapon>();
            Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
            continue;
         }
         weapons[i].SetActive(false);
      }
   }
}
