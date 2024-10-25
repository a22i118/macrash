using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public FadeScene fadescene;
    [SerializeField] private GameObject ConfigPanel;

    public void StartButton()
    {
        Debug.Log("success");
        fadescene.FadeToScene("GameScene");

    }

    public void EndButton()
    {
        Application.Quit(); //ゲーム終了

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //エディタ終了
#endif
    }

    public void OnConfigPanel()
    {
        ConfigPanel.SetActive(true);
    }

    public void OffConfigPanel()
    {
        ConfigPanel.SetActive(false);
    }
}
