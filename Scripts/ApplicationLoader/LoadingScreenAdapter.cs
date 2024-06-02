using _Fly_Connect.Scripts.ApplicationLoader;
using UnityEngine;

public sealed class LoadingScreenAdapter : MonoBehaviour
{
    private readonly ApplicationLoader _loader;

    public void OnFailed(string message)
    {
        LoadingScreen.ReportError(message);
    }

    private void OnCompleted()
    {
        LoadingScreen.Hide();
    }
}