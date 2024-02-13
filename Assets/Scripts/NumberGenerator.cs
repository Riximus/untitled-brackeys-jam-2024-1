using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;

public class NumberGenerator : MonoBehaviour, IInteractable
{
    public int number;
    public void Interact()
    {
        number = Random.Range(0, 100);
        Debug.Log($"The number is {number}");
    }
}
