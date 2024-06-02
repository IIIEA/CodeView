using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivatePolicityButton : MonoBehaviour
{
    public void OpenPolicityURL()
    {
      Application.OpenURL("https://madpixel.dev/privacy.html");  
    }
}
