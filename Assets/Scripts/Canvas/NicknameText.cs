using UnityEngine;
using TMPro;

public class NicknameText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    public void SetupNick(string nick)
    {
        if (_text == null)
            _text = GetComponent<TextMeshProUGUI>();
        _text.text = nick;
    }
}