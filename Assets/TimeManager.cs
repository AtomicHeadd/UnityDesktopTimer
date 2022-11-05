using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeManager : SingletonMonoBehaviour<TimeManager>
{
    [SerializeField] GameObject canvas;

    TextMeshProUGUI timeText;
    TextMeshProUGUI title;
    TextMeshProUGUI spentTime;

    bool isCounting;
    float countingTime;

    GameObject leftArrow;
    GameObject rightArrow;
    GameObject startButton;
    GameObject pause;
    TMP_InputField nameInput;

    int pageIndex;

    private void Start()
    {
        isCounting = false;
        countingTime = 0;
        pageIndex = 0;

        timeText = canvas.transform.Find("Time/Time").GetComponent<TextMeshProUGUI>();
        title = canvas.transform.Find("Detail/Title").GetComponent<TextMeshProUGUI>();
        spentTime = canvas.transform.Find("Detail/Total").GetComponent<TextMeshProUGUI>();
        rightArrow = canvas.transform.Find("Detail/Right").gameObject;
        leftArrow = canvas.transform.Find("Detail/Left").gameObject;
        nameInput = canvas.transform.Find("Detail/Input").GetComponent<TMP_InputField>();
        startButton = canvas.transform.Find("Start/Start").gameObject;
        pause = canvas.transform.Find("Start/Pause").gameObject;
        pause.SetActive(false);

        timeText.text = ConvertTimeToText(0);

        ShowTaskDetail(0);
    }
    private void Update()
    {
        //名前変更
        if(nameInput != null && nameInput.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Return))
        {
            nameInput.gameObject.SetActive(false);
            if(nameInput.text != "")
            {
                string previousName = History.Instance.savedTaskNames[pageIndex];
                History.Instance.savedTaskNames[pageIndex] = nameInput.text;
                History.Instance.taskTotalTimes[nameInput.text] = History.Instance.taskTotalTimes[previousName];
                History.Instance.taskTotalTimes.Remove(previousName);
                ShowTaskDetail(pageIndex);
                History.Instance.SaveTotalTimes();
            }
        }
        //カウント
        if (!isCounting) return;
        countingTime += Time.deltaTime;
        timeText.text = ConvertTimeToText(countingTime);
    }

    string ConvertTimeToText(float time)
    {
        //時間がない時は時間を省略
        int hour = Mathf.FloorToInt(time / 3600);
        int minute = Mathf.FloorToInt(time % 3600 / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int lessThanSeconds = Mathf.FloorToInt((time % 1)*100);
        return $"{hour:d2}:{minute:d2}:{seconds:d2}.{lessThanSeconds:d2}";
    }

    public void StartCounting(bool start)
    {
        isCounting = start;
        if (!start)
        {           
            History.Instance.FinishRecording(countingTime);
            ShowTaskDetail(pageIndex);
            countingTime = 0;
            timeText.text = ConvertTimeToText(0);                    
        }
        else
        {
            History.Instance.StartRecoding(History.Instance.savedTaskNames[pageIndex]);
            rightArrow.SetActive(false);
            leftArrow.SetActive(false);
            pause.SetActive(true);
            startButton.SetActive(false);
        }
    }

    void ShowTaskDetail(int index)
    {
        pageIndex = index;
        leftArrow.SetActive(index > 0);
        rightArrow.SetActive(index < History.Instance.savedTaskNames.Count - 1);
        title.text = History.Instance.savedTaskNames[index];
        pause.SetActive(false);
        startButton.SetActive(true);
        string totalTime = History.Instance.GetTotalTimeByIndex(index);
        string[] hhmmss = totalTime.Split(":");
        //時間まで行ってなかったら分単位
        if (int.Parse(hhmmss[0]) <= 0)
        {
            float seconds = float.Parse(hhmmss[1]) * 60f + float.Parse(hhmmss[2]);
            spentTime.text = $"{seconds / 60:F1} minutes";
        }
        else
        {
            float minutes = float.Parse(hhmmss[0]) * 60 + float.Parse(hhmmss[1]);
            spentTime.text = $"{minutes / 3600:F1} hours";
        }
    }

    public void OnClickNewTask()
    {
        History.Instance.AddNewTask($"New Task{pageIndex}");
        ShowTaskDetail(History.Instance.savedTaskNames.Count - 1);
        History.Instance.SaveTotalTimes();
    }

    public void TogglePause()
    {
        isCounting = !isCounting;
    }

    public void OnClickArrow(bool right)
    {
        if (right) pageIndex++;
        else pageIndex--;
        ShowTaskDetail(pageIndex);
    }

    public void OnClickDelete()
    {
        canvas.transform.Find("DeleteConfirm").gameObject.SetActive(true);
    }

    public void OnClickConfirmDelte(bool delete)
    {
        canvas.transform.Find("DeleteConfirm").gameObject.SetActive(false);
        if (delete)
        {
            History.Instance.taskTotalTimes.Remove(History.Instance.savedTaskNames[pageIndex]);
            History.Instance.savedTaskNames.RemoveAt(pageIndex);
            if (History.Instance.savedTaskNames.Count == 0) History.Instance.AddNewTask("New Task");
            else if (pageIndex == History.Instance.savedTaskNames.Count) pageIndex--;
            ShowTaskDetail(pageIndex);           
        }
    }

    public void ToggleEdit()
    {
        nameInput.gameObject.SetActive(!nameInput.gameObject.activeInHierarchy);
    }
    void Load(out List<string> names, out List<float> times)
    {
        names = new List<string>();
        times = new List<float>();
        if (PlayerPrefs.HasKey("Names"))
        {
            string[] savedNames = PlayerPrefs.GetString("Names").Split(",");
            string[] savedTimes = PlayerPrefs.GetString("Times").Split(",");
            print(PlayerPrefs.GetString("Times"));
            for(int i=0; i<savedNames.Length-1; i++)
            {
                names.Add(savedNames[i]);
                print($"{savedNames[i]}:{savedTimes[i]}");
                times.Add(float.Parse(savedTimes[i]));
            }
        }
        else
        {
            names.Add("New Task");
            times.Add(0);
        }
    }

    void Save(List<string> names, List<float> times)
    {
        string saveName = "";
        string saveTime = "";
        foreach (string name in names) saveName += name + ",";
        foreach (float time in times) saveTime += time.ToString() + ",";
        print(saveName);
        print(saveTime);
        PlayerPrefs.SetString("Names", saveName);
        PlayerPrefs.SetString("Times", saveTime);
        PlayerPrefs.Save();
    }
    [ContextMenu("Reset")]
    public void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
