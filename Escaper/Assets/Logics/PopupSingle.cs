using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

using DigitalRuby.SoundManagerNamespace;
using System;

public class PopupSingle : MonoBehaviour
{
    public Text textTitle;
    public Text textDesc;

    public float hideXAxis = 900f;

    public struct popupBase
    {
        public string title;
        public string desc;
        public Action closeCallback;
    }

    private Queue<popupBase> popupList = new Queue<popupBase>();

    private void Start()
    {
        DestroyAllPopup();
    }

    public void ShowPopup(string title, string desc, Action closeCallback = null)
    {
        if (popupList == null) popupList = new Queue<popupBase>();

        popupBase newPopup = new popupBase();
        newPopup.title = title;
        newPopup.desc = desc;
        newPopup.closeCallback = closeCallback;

        popupList.Enqueue(newPopup);

        if (popupList.Count == 1) ShowInPopupList();
    }

    private void ShowInPopupList()
    {
        popupBase showingPopup = popupList.Peek();
        textTitle.text = showingPopup.title;
        textDesc.text = showingPopup.desc;

        this.transform.localPosition = Vector3.zero;

        SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select].clip);
    }

    public void OnClickClose()
    {
        popupBase closePopup = popupList.Dequeue();
        closePopup.closeCallback?.Invoke();

        if (popupList.Count > 0)
        {
            // if popup exist
            ShowInPopupList();
        }
        else
        {
            SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select].clip);
            this.transform.localPosition = new Vector3(hideXAxis, 0, 0);
        }
    }

    public void DestroyAllPopup()
    {
        //SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select].clip);
        this.transform.localPosition = new Vector3(hideXAxis, 0, 0);

        if (popupList != null) popupList.Clear();

        textTitle.text = string.Empty;
        textDesc.text = string.Empty;
    }
}
