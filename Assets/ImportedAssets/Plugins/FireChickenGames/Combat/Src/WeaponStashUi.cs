namespace FireChickenGames.Combat
{
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Fire Chicken Games/Combat/Weapon Stash UI")]
    public class WeaponStashUi : MonoBehaviour
    {
        [Header("Weapon")]
        public Text weaponNameText;
        public Text weaponDescriptionText;

        [Header("Ammo")]
        public Text ammoNameText;
        public Text ammoDescriptionText;

        [Header("Clip")]
        public Text ammoInClipText;
        public Text ammoMaxClipText;
        public Text ammoInStorageText;

        [Header("Weapon Settings")]
        public Text fireModeText;

        public void SetWeapon(string weaponName, string weaponDescription = null)
        {
            if (weaponNameText != null)
                weaponNameText.text = weaponName;

            if (weaponDescriptionText != null && weaponDescription != null)
                weaponDescriptionText.text = weaponDescription;
        }

        public void RemoveAmmoClipAndStorage()
        {
            if (ammoNameText != null)
                ammoNameText.text = "";

            if (ammoDescriptionText != null)
                ammoDescriptionText.text = "";

            if (ammoInClipText != null)
                ammoInClipText.text = "";

            if (ammoMaxClipText != null)
                ammoMaxClipText.text = "";

            if (ammoInStorageText != null)
                ammoInStorageText.text = "";
        }

        public void SetAmmo(string name, string description)
        {
            if (ammoNameText != null)
                ammoNameText.text = name;

            if (ammoDescriptionText != null)
                ammoDescriptionText.text = description;
        }

        public void SetAmmoInClip(string ammoID, int amount)
        {
            if (ammoInClipText != null)
                ammoInClipText.text = amount.ToString();
        }

        public void SetAmmoInStorage(string ammoID, int amount)
        {
            if (ammoInStorageText != null)
                ammoInStorageText.text = amount.ToString();
        }

        public void SetAmmoMaxClipText(string ammoMaxClip)
        {
            if (ammoMaxClipText != null)
                ammoMaxClipText.text = ammoMaxClip;
        }

        public void SetFireMode(string fireModeName)
        {
            if (fireModeText != null)
                fireModeText.text = fireModeName;
        }
    }
}
