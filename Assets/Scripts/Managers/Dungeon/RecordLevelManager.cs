using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordLevelManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform bossMap;
    public CameraController camController;

    private void Start()
    {
        PlayerSpawn();
        EquipLocalWeaponAndArtifacts();
    }

    void PlayerSpawn()
    {
        Instantiate(playerPrefab, bossMap);
        camController.AttachPlayerInstance();
    }

    void EquipLocalWeaponAndArtifacts()
    {
        JItemsList itemsList = GameManager.Instance.GetJItems();

        ItemData weaponItemData = new ItemData();
        for (int i = 0; i < ItemGenerator.Instance.weaponDatas.Length; i++)
        {
            if (ItemGenerator.Instance.weaponDatas[i].itemName == itemsList.weapon.itemName)
            {
                weaponItemData = ItemGenerator.Instance.weaponDatas[i];
                break;
            }
        }

        Dictionary<WeaponKey, float> weaponOptions = new Dictionary<WeaponKey, float>();
        weaponOptions.Add(WeaponKey.ATK, itemsList.weapon.atk);
        weaponOptions.Add(WeaponKey.ATKSPEED, itemsList.weapon.atkspeed);
        weaponOptions.Add(WeaponKey.CRIT_RATE, itemsList.weapon.critrate);
        weaponOptions.Add(WeaponKey.CRIT_DAMAGE, itemsList.weapon.critdamage);
        weaponOptions.Add(WeaponKey.SOCKET, 6);

        Weapon newWeapon = new Weapon(weaponItemData, 0, 0, weaponOptions);
        newWeapon.itemName = weaponItemData.itemName;
        Inventory.instance.AddItem(newWeapon);

        // 무기 장착
        // WeaponEquip(newWeapon);


        for (int i = 0; i < itemsList.items.Count; i++)
        {
            ItemData artiItemData = ItemGenerator.Instance.artifactDatas[
                Math.Clamp((itemsList.items[i].itemTier / 5), 0, ItemGenerator.Instance.artifactDatas.Length - 1)];

            Artifact newArtifact = new Artifact(artiItemData, (ulong)(i + 1), (short)itemsList.items[i].itemTier,
                itemsList.items[i].itemOptions);
            newArtifact.itemName = itemsList.items[i].itemName;
            Inventory.instance.AddItem(newArtifact);
        }

        Player.Instance.heart.PlayerItemEquip();
    }

    void WeaponEquip(Weapon newWeapon)
    {
        
        
    }
}