using UnityEngine;
using UnityEngine.UI;

public class NicknameText : MonoBehaviour
{
    private Text _text;

    public void SetupNick(string nick)
    {
        if (_text == null)
            _text = GetComponent<Text>();
        _text.text = nick;
    }
}