using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AlictusGD
{
    public class BoomerangController : MonoBehaviour
    {
        public float duration = 1; // in seconds

        public Vector3 _enemyPosition;
        public bool startAgain = false;
        void Start()
        {
            StartCoroutine(DestroyBoomerang());
        }
        //yok etme
        IEnumerator DestroyBoomerang()
        {
            yield return new WaitForSeconds(2);
            Destroy(this);
        }
        //Hareket etme
        void Update()
        {
            transform.DOMove(new Vector3( _enemyPosition.x,2,_enemyPosition.z), 1);
            transform.DORotate(new Vector3(0,180,0), 1,RotateMode.WorldAxisAdd);
        }
        //Düþmaný öldürme
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<SkeletonController>().DestroyEnemy();
                Destroy(this);

            }
        }
    }
}