using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] GameObject historyWindow;
    public void ShowHistory(bool show)
    {
        historyWindow.SetActive(show);
        if (show) historyWindow.GetComponent<HistoryWindow>().ShowTask(0);
    }
}
