using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
namespace AlictusGD
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public TextMeshProUGUI _coinText,_enemyText;
        [SerializeField] GameObject _coinGo, _enemyGo,_player,ParentCoin,ParentSkeleton;
        [SerializeField] List<GameObject> SkeletonsArr = new List<GameObject>();
        [SerializeField] int _coinValue,_enemyValue;
        public int _collectedCoin, _killedEnemy;
        //UI
        [SerializeField] GameObject GameOverPanel;
        Vector3 skPos;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            Time.timeScale = 1;
            _collectedCoin = PlayerPrefs.GetInt("coin");
            _coinText.text = _collectedCoin.ToString();
            CoinCreate();
            EnemyCreate(_enemyValue);
            ParentCoin = GameObject.FindGameObjectWithTag("Coins");
            ParentSkeleton = GameObject.FindGameObjectWithTag("ParentSkeleton");
        }
        //Coin Oluþturma yenilenmez
        private void CoinCreate()
        {
            for (int i = 0; i < _coinValue; i++)
            {
                Instantiate(_coinGo,new Vector3(Random.Range(249, -249), -0.619f, Random.Range(249, -249)), Quaternion.identity, ParentCoin.transform);

            }
        }

        //Düþman Oluþturma düþman öldürdükçe yerine yenisi gelir
        public void EnemyCreate(int val)
        {
            
            for (int i = 0; i < val; i++)
            {
                while (findPerfectPosition() == Vector3.zero)
                {
                    findPerfectPosition();
                }
              GameObject skeleton=  Instantiate(_enemyGo, skPos, Quaternion.identity, ParentSkeleton.transform);
                Debug.Log(skeleton+"skeleton");
              SkeletonsArr.Add(skeleton.gameObject);
            }
        }
        //Ekranda görünmeyen konumlarda düþman oluþur
        Vector3 findPerfectPosition()
        {
            skPos = new Vector3(Random.Range(249, -249),4f, Random.Range(249, -249));
            if (skPos.x > _player.transform.position.x + 30 || skPos.x < _player.transform.position.x - 30)
            {
                return skPos;
            }
            else if (skPos.z > _player.transform.position.z + 30 || skPos.z < _player.transform.position.z - 30)
            {
                return skPos;
            }
            else
                return Vector3.zero;
        }

        public void GameOver()
        {
            Time.timeScale=0;
            GameOverPanel.SetActive(true);
        }
        //yeniden baþlatma
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}