using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownMenu : MonoBehaviour
{
    public Button dropdownBtn;
    public GameObject dropdownWindow;

    public void DropdownAction()
    {
        dropdownBtn.gameObject.SetActive(false);
        dropdownWindow.SetActive(true);
    }
}
