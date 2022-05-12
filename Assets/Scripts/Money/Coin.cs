﻿using UnityEngine;
using DG.Tweening;

namespace Money
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private AudioSetting _audioSetting;
        private AudioSource _audioSource;
        private MeshRenderer _mesh;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _mesh = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            _audioSource.mute = _audioSetting.IsMute;
            transform.DORotate(new Vector3(0,360f,0), 1.5f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }

        private void OnEnable()
        {
            _audioSetting.Clicked += OnClicked;
        }

        private void OnDisable()
        {
            _audioSetting.Clicked -= OnClicked;
        }

        private void OnClicked(bool isMute)
        {
            _audioSource.mute = !isMute;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerMovement player))
            {
                _audioSource.Play();
                _mesh.enabled = false;
            }
        }
    }
}