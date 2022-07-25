using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
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

    List<string> taskNames;
    List<float> totalTimes;
    int pageIndex;

    

    private void Awake()
    {
        Load(out taskNames, out totalTimes);
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
        if(nameInput != null && nameInput.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Return))
        {
            nameInput.gameObject.SetActive(false);
            if(nameInput.text != "")
            {
                taskNames[pageIndex] = nameInput.text;
                ShowTaskDetail(pageIndex);
                Save(taskNames, totalTimes);
            }
        }

        if (!isCounting) return;
        countingTime += Time.deltaTime;
        timeText.text = ConvertTimeToText(countingTime);
    }

    string ConvertTimeToText(float time)
    {
        //ŽžŠÔ‚ª‚È‚¢Žž‚ÍŽžŠÔ‚ðÈ—ª
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
            totalTimes[pageIndex] += countingTime;
            ShowTaskDetail(pageIndex);

            countingTime = 0;
            timeText.text = ConvertTimeToText(0);          
            Save(taskNames, totalTimes);
        }
        else
        {
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
        rightArrow.SetActive(index < taskNames.Count - 1);
        title.text = taskNames[index];
        pause.SetActive(false);
        startButton.SetActive(true);
        //ŽžŠÔ‚Ü‚Ås‚Á‚Ä‚È‚©‚Á‚½‚ç•ª’PˆÊ
        if (totalTimes[index] < 3600)
        {
            spentTime.text = $"{totalTimes[index] / 60:F1} minutes";
        }
        else spentTime.text = $"{totalTimes[index] / 3600:F1} hours";
    }

    public void OnClickNewTask()
    {
        taskNames.Add($"New Task{pageIndex}");
        totalTimes.Add(0);
        ShowTaskDetail(taskNames.Count - 1);
        Save(taskNames, totalTimes);
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
            taskNames.RemoveAt(pageIndex);
            totalTimes.RemoveAt(pageIndex);
            if (taskNames.Count == 0)
            {
                taskNames.Add("New Task0");
                totalTimes.Add(0);
            }
            else if (pageIndex == taskNames.Count) pageIndex--;
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
                print(savedTimes[i]);
                print(savedNames[i]);
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
