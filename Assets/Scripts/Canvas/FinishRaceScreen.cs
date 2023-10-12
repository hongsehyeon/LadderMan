using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FinishRaceScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _winnerNickText = new TextMeshProUGUI[3];
    [SerializeField] private Image[] _winnerImage = new Image[3];
    [SerializeField] private TextMeshProUGUI _heightText;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void SetWinner(string nick, Color color, int place)
    {
        _winnerNickText[place].text = nick;
        _winnerImage[place].color = color;

        _winnerImage[place].gameObject.SetActive(true);
        _winnerNickText[place].gameObject.SetActive(true);
    }

    public void SetHeight(float height) => _heightText.text = $"{Math.Truncate(height * 10) / 10}m";

    public void FadeIn()
    {
        _anim.Play("FadeIn");
    }

    public void FadeOut()
    {
        _anim.Play("FadeOut");
    }
}