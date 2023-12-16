using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AlictusGD
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [SerializeField] GameObject _coinGo, _enemyGo;
        [SerializeField] int _coinValue;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            CoinCreate();
        }

        private void CoinCreate()
        {
            for (int i = 0; i < _coinValue; i++)
            {
                Instantiate(_coinGo,new Vector3(Random.Range(520, -479),1.85f, Random.Range(486, -512)), Quaternion.identity,GameObject.FindGameObjectWithTag("Coins").transform);

            }
        }
        public void GameOver()
        {

        }
    }
}