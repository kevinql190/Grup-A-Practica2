using UnityEngine.InputSystem;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public bool isPaused = false;

    public void SetPause(bool willPause)
    {
        isPaused = willPause;
        PlayerInputHandler.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap(willPause ? "UI" : "Gameplay");
        AudioListener.pause = willPause;
        Time.timeScale = willPause ? 0 : 1;
        if (willPause) PlayerInputHandler.Instance.LockInputs();
        else PlayerInputHandler.Instance.EnableInputs();
    }
}
