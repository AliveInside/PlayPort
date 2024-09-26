using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayPort.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        private static int _moneyCount;

        public void DestroyObject()
        {
            Destroy(_objectToDestroy);
        }

        //Временное решение пока не настроили систему экономики в отдельном скрипте
        public void MoneyCountMessage()
        {
            if (_objectToDestroy.tag == "GoldenCoin")
            {
                _moneyCount += 10;
            }
            else if (_objectToDestroy.tag == "SilverCoin")
            {
                _moneyCount += 1;
            }

            Debug.Log("You have: " + _moneyCount + " coins");
        }

        public void OnLevelWasLoaded(int level)
        {
            _moneyCount = 0; // Сбрасываем _moneyCount при загрузке нового уровня
        }
    }
}
