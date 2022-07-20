using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiseButton : MonoBehaviour
{
    MultiPlayerInput myInput;

    public void SetButton(MultiPlayerInput _myInput)
    {
        myInput = _myInput;
    }

    public void ButtonClick(string choise)
    {
        myInput.ButtonClick(choise);
    }
}
