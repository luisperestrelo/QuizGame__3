using UnityEngine;
using UnityEngine.UI;

public class RemoveOneOracle : MonoBehaviour
{
    private Image _image;
    private Button _btn;
    private Animator _anim;

    private void Start()
    {
        _image = GetComponentInChildren<Image>(); 
        _btn = GetComponentInChildren<Button>();
        _anim = GetComponentInChildren<Animator>();
    }

    public void ClickAnimation(bool wasSuccessful)
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsTag("Busy") || _anim.GetNextAnimatorStateInfo(0).IsTag("Busy")) return;
        if (wasSuccessful)
        {
            _anim.SetTrigger("TriggerSpin");
        }
        else
        {
            _anim.SetTrigger("TriggerFail");
        }
    }
}
