using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField]
    float AttackRange;

    [SerializeField]
    float AttackRate;

    private CharacterController2D _character;

    private void Awake()
    {
        _character = GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            _character.Punch();
        }
    }
}

// <>
