using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlictusGD
{
    public class HealthBar : MonoBehaviour
    {
        public GameObject player;

        private void Update()
        {
            this.transform.position = new Vector3(player.transform.position.x, 6, player.transform.position.z);
        }
    }
}