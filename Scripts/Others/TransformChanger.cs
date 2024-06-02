using System;
using UnityEngine;

public class TransformChanger : MonoBehaviour
{
   [SerializeField] private GameObject _booster1;
   [SerializeField] private GameObject _booster2;
   [SerializeField] private GameObject _upgradeContainer;
   [SerializeField] private GameObject _upgradeVerticalContainer;

   [SerializeField] private GameObject _ad;
   [SerializeField] private GameObject _shop;
   [SerializeField] private GameObject _adUpgradeContainer;
   [SerializeField] private GameObject _adUpgradeVerticalContainer;

   public void Update()
   {
      float aspectRatio = (float) Screen.width / Screen.height;

      if (aspectRatio > 1.0f)
      {
         if (_booster1.transform.parent == _upgradeContainer.transform)
            return;

         _booster1.transform.SetParent(_upgradeContainer.transform);
         _booster2.transform.SetParent(_upgradeContainer.transform);
         _booster1.transform.SetAsFirstSibling();
         _booster2.transform.SetAsFirstSibling();

         _ad.transform.SetParent(_adUpgradeContainer.transform);
         _shop.transform.SetParent(_adUpgradeContainer.transform);
      }
      else
      {
         if (_booster1.transform.parent == _upgradeVerticalContainer.transform)
            return;

         _booster1.transform.SetParent(_upgradeVerticalContainer.transform);
         _booster2.transform.SetParent(_upgradeVerticalContainer.transform);
         _ad.transform.SetParent(_adUpgradeVerticalContainer.transform);
         _shop.transform.SetParent(_adUpgradeVerticalContainer.transform);
      }
   }
}
