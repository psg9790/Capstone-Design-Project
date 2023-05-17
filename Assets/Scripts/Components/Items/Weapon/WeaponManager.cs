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
   public GameObject weaponObject;
   private GameObject equipweapon;
   public RuntimeAnimatorController BaseAnimator;

   public Item item;
   
   public Vector3 atk_pos = Vector3.zero;
   
   // Weapon Coll Time
   
   public float[] CoolTime = {0,0,0,0};
   
   
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
      CoolTime[0] = weaponInfo.QCoolTime;
      CoolTime[1] = weaponInfo.WCoolTime;
      CoolTime[2] = weaponInfo.ECoolTime;
      CoolTime[3] = weaponInfo.RCoolTime;
      equipweapon = weapon;
      weapon.SetActive(false);
      
   }

   public void UnRegisterWeapon()
   {
      if (weaponObject != null)
      {
         Object.Destroy(weaponObject);
      }

      Weapon = null;
      SetBase();
   }

   public void SetWeapon(GameObject weapon)
   {
      
      if (weaponObject != null)
      {
         UnRegisterWeapon();
      }
      GameObject BaseWeapon = Object.Instantiate(weapon);
      RegisterWeapon(BaseWeapon);
      weaponObject = BaseWeapon;
      
      Weapon = BaseWeapon.GetComponent<BaseWeapon>();
      weaponObject.SetActive(true);
      Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
   }

   public void SetBase()
   {
      GameObject weapon = Player.Instance.baseWeapon;
      GameObject BaseWeapon = Object.Instantiate(weapon);
      RegisterWeapon(BaseWeapon);
      weaponObject = BaseWeapon;
      
      Weapon = BaseWeapon.GetComponent<BaseWeapon>();
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
