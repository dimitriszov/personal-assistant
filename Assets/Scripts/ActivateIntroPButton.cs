using UnityEngine;

public class ActivateIntroPButton : MonoBehaviour
{
    public GameObject introPObject;
    public void ActivateIntroP()
    {
        introPObject.SetActive(true);
    }
}

