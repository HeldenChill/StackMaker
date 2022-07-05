using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys
{
    using System;
    public class CheckCollide : MonoBehaviour
    {
        public event Action OnPlayerReachDes;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Destination")
            {
                OnPlayerReachDes?.Invoke();
            }
        }
    }
}