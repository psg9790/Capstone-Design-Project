using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Monsters
{

    public class BasicMagic : MonoBehaviour
    {
        private float elapsedTime;
        private float durationTime;

        public void Init(Vector3 _startPos, Vector3 _dir, float _duration)
        {
            durationTime = _duration;
            transform.position = _startPos;
            transform.rotation = Quaternion.LookRotation(_dir, Vector3.up);
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            transform.position += transform.forward * Time.deltaTime * 10f;
            if (elapsedTime > durationTime)
            {
                Terminate();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer.CompareTo(LayerMask.NameToLayer("Player")) == 0)
            {
                Terminate();
            }
        }

        public void Terminate()
        {
            Destroy(this.gameObject);
        }
    }
}
