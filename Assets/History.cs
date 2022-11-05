using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class History : SingletonMonoBehaviour<History>
{
    string timestampFile;
    string totalTimeFile;

    public Dictionary<string, string> taskTotalTimes;
    public List<string> savedTaskNames;
    
    string recordingTaskName;
    string taskStartedDate;


    private void Awake()
    {
        timestampFile = Application.dataPath + "/history.csv";
        totalTimeFile = Application.dataPath + "/total.csv";
        LoadTotalTime();
    }
    //total���Ԃ�ۑ�����t�@�C�� {�^�X�N��},{h:mm:ss.ss}
    //�^�C���X�^���v��ۑ�����t�@�C��
    //        yyyy.mm.dd, hh:mm, TASK_NAME, h:mm:ss
    //�ۑ���: 2022.11.03, 09:05, Unity, 1:00:02
    //�ϐ�����time�͑S��hh:mm:dd��string�`��
    public void LoadTotalTime()
    {
        taskTotalTimes = new Dictionary<string, string>();
        savedTaskNames = new List<string>();
        if (!File.Exists(totalTimeFile))
        {
            AddNewTask("New Task");
            return;
        }
        string data = File.ReadAllText(totalTimeFile);
        string[] lines = data.Split("\n");
        foreach(string i in lines)
        {
            if (i == "") return;
            string[] columns = i.Split(",");
            taskTotalTimes[columns[0]] = columns[1];
            savedTaskNames.Add(columns[0]);
        }
        foreach (KeyValuePair<string, string> kvp in taskTotalTimes) print($"{kvp.Key}: {kvp.Value}");
    }

    public void AddNewTask(string taskName)
    {
        taskTotalTimes[taskName] = "00:00:00";
        savedTaskNames.Add(taskName);
    }

    public string GetTotalTimeByIndex(int index)
    {
        if (index >= savedTaskNames.Count) return "";
        return taskTotalTimes[savedTaskNames[index]];
    }

    void AddTotalTime(string taskName, string timeString="00:00:00")
    {
        string[] columns = timeString.Split(":");
        string[] savedColumns;
        if (taskTotalTimes.ContainsKey(taskName)) savedColumns = taskTotalTimes[taskName].Split(":");
        else savedColumns = "00:00:00".Split(":");
        int hour = int.Parse(savedColumns[0]) + int.Parse(columns[0]);
        int minute = int.Parse(savedColumns[1]) + int.Parse(columns[1]);
        float second = float.Parse(savedColumns[2]) + float.Parse(columns[2]);
        taskTotalTimes[taskName] = $"{hour}:{minute}:{second}";
    }

    public void SaveTotalTimes()
    {
        string saveText = "";
        foreach (KeyValuePair<string, string> kvp in taskTotalTimes) saveText += $"{kvp.Key}, {kvp.Value}\n";
        File.WriteAllText(totalTimeFile, saveText);
    }

    string ConvertTimeToText(float time)
    {
        //���Ԃ��Ȃ����͎��Ԃ��ȗ�
        int hour = Mathf.FloorToInt(time / 3600);
        int minute = Mathf.FloorToInt(time % 3600 / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int lessThanSeconds = Mathf.FloorToInt((time % 1) * 100);
        return $"{hour:d2}:{minute:d2}:{seconds:d2}.{lessThanSeconds:d2}";
    }

    public void StartRecoding(string taskName)
    {
        recordingTaskName = taskName;
        taskStartedDate = DateTime.Now.ToString();
    }

    public void FinishRecording(float time)
    {
        //Total���Ԃ̕ۑ�
        //�^�C���X�^���v�̕ۑ�
        if (recordingTaskName == "") return;
        string timeString = ConvertTimeToText(time);
        File.AppendAllText(timestampFile, $"{taskStartedDate}, {recordingTaskName}, {timeString}\n");
        AddTotalTime(recordingTaskName, timeString);
        SaveTotalTimes();
        recordingTaskName = "";
        taskStartedDate = "";
    }
}
