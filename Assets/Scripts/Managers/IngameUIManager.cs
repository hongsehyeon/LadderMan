using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{

    #region singleton

    static IngameUIManager _instance;
    public static IngameUIManager Instance { get { return _instance; } }
    #endregion

    public Image cooltimeImg;
    public TMP_Text ladderCount_text;
    void Start()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public void UseLadder(float coolTime)
    {
        StartCoroutine(coolTimeUpdateCor(coolTime));
        ChangeLadderCount(Int32.Parse(ladderCount_text.text)-1);
    }

    IEnumerator coolTimeUpdateCor(float cooltime)
    {
        cooltimeImg.fillAmount = 1;

        float starttime = Time.time;
        while(cooltimeImg.fillAmount > 0)
        {
            yield return null;

            cooltimeImg.fillAmount = 1 - Time.time - starttime;
        }

        cooltimeImg.fillAmount = 0;
    }

    public void ChangeLadderCount(int count)
    {
        ladderCount_text.text = count.ToString();
    }
}
