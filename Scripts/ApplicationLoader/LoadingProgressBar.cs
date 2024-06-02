using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public sealed class LoadingProgressBar : MonoBehaviour
{
    [FormerlySerializedAs("text")] [SerializeField]
    private TMP_Text _text;

    [FormerlySerializedAs("fill")] [SerializeField]
    private Image _fill;

    public void SetProgress(float progress)
    {
        _fill.fillAmount = progress;
        _text.text = $"{(progress * 100):F0}%";
    }
}