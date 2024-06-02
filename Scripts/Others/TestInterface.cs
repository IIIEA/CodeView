using System;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using TMPro;
using UnityEngine;

public class TestInterface : MonoBehaviour, IGameStartListener, IGameFinishListener
{
   [SerializeField] private TMP_Text _text;
    private PathTapObserver _pathTapObserver;

    [Inject]
    private void Construct(PathTapObserver inputSystem)
    {
        _pathTapObserver = inputSystem;
    }

    public void OnStartGame()
    {
        // _pathTapObserver.OnTapped += OnHandleTap;
    }

    public void OnFinishGame()
    {
        // _pathTapObserver.OnTapped += OnHandleTap;
    }

    private void OnHandleTap(string arg)
    {
        Debug.Log(arg);
        _text.text = arg;
    }
}