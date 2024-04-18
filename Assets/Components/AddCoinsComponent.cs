using System.Collections;
using System.Collections.Generic;
using PlayPort;
using UnityEngine;
using PlayPort.Creatures;

public class AddCoinsComponent : MonoBehaviour
{
    [SerializeField] private int _amountCoins;
    private Hero _hero;
    private void Start()
    {
        _hero = FindObjectOfType<Hero>();
    }

    public void Add()
    {
        _hero.AddCoins(_amountCoins);
    }
}
