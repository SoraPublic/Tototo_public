using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MasterSlider : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private AudioMixer audioMixer;
    private Slider slider;

    public const string mastrtVol = "MasterVol";

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { ChangeValue(slider.value); });

        //セーブデータから読み込んでスライダーへ
        DataManger dataManger = new DataManger();
        slider.value = dataManger.LoadGameData().masterVolume;
    }
    public float ConvertVolumeToDb(float volume)
    {
        return Mathf.Clamp(Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)) * 20f, -80f, 0f);
    }
    private void ChangeValue(float volume)
    {
        audioMixer.SetFloat(mastrtVol, ConvertVolumeToDb(volume));
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //スライダーから指を離したとき
        SeManager.instance.ButtonSound();

        //セーブする
        DataManger dataManger = new DataManger();
        GameData gameData = dataManger.LoadGameData();
        gameData.masterVolume = slider.value;
        dataManger.SaveGameData(gameData);
    }

}
