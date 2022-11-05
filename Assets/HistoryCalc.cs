using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HistoryCalc : SingletonMonoBehaviour<HistoryCalc>
{
    public List<Detail> GetHistoryDatails(string taskName)
    {
        //最初に始まった月を得る
        List<Detail> retDetail = new List<Detail>();
        string timestampFile = Application.dataPath + "/history.csv";
        if (!File.Exists(timestampFile)) return retDetail;
        string data = File.ReadAllText(timestampFile);
        string[] lines = data.Split("\n");
        Dictionary<string, float> minuteAday = new Dictionary<string, float>();
        string lastDay = "";
        foreach(string i in lines)
        {
            if (!i.Contains(taskName)) continue;
            string[] columns = i.Split(",");
            if (columns.Length != 3) continue;
            string day = columns[0].Split(" ")[0];
            float minutes = stringToMinute(columns[2]);
            //print($"{columns[2]} -> {minutes}");
            if (lastDay == day) minuteAday[day] += minutes;
            else minuteAday[day] = minutes;
            lastDay = day;
        }
        if(minuteAday.Count == 0) return retDetail;
        foreach (KeyValuePair<string, float> kvp in minuteAday) print($"{kvp.Key}: {kvp.Value}");
        //この段階でOverallは作成可能
        //次に月ごとにminutesAdayを分割し、それぞれでDetail作成      
        string currentMonth = "";
        Detail currentDetail = null;
        foreach (KeyValuePair<string, float> kvp in minuteAday)
        {
            string[] date = kvp.Key.Split("/");
            string month = date[0] + "/" + date[1];
            if (currentMonth != month)
            {
                currentDetail = new Detail(month);
                retDetail.Add(currentDetail);
                currentMonth = month;
            }
            currentDetail.totalMinutes += kvp.Value;
            currentDetail.workingDay += 1;
            if(kvp.Value > currentDetail.LongestTime)
            {
                currentDetail.LongestTime = kvp.Value;
                currentDetail.LongestDay = kvp.Key;
            }
            if (kvp.Value < currentDetail.ShortestTime || currentDetail.ShortestTime < 0)
            {
                currentDetail.ShortestTime = kvp.Value;
                currentDetail.ShortestDay = kvp.Key;
            }
        }
        foreach (Detail i in retDetail) print($"{i.month}: {i.totalMinutes}");
        //最後にOverall作成・各averageTime計算
        Detail overall = new Detail("Overall");
        foreach(Detail d in retDetail)
        {
            d.averageTime = d.totalMinutes / d.workingDay;
            overall.totalMinutes += d.totalMinutes;
            overall.workingDay += d.workingDay;
            if (d.LongestTime > overall.LongestTime)
            {
                overall.LongestDay = d.LongestDay;
                overall.LongestTime = d.LongestTime;
            }
            if (d.ShortestTime < overall.ShortestTime || overall.ShortestTime <= 0)
            {
                overall.ShortestDay = d.ShortestDay;
                overall.ShortestTime = d.ShortestTime;
            }
        }
        overall.averageTime = overall.totalMinutes / overall.workingDay;
        retDetail.Insert(0, overall);
        return retDetail;
    }

    public string GetWhenTaskStarted(string taskName)
    {
        string timestampFile = Application.dataPath + "/history.csv";
        if (!File.Exists(timestampFile)) return "None";
        string data = File.ReadAllText(timestampFile);
        string[] lines = data.Split("\n");
        foreach(string i in lines)
        {
            if (!i.Contains(taskName)) continue;
            return i.Split(",")[0].Split(" ")[0];
        }
        return "None";
    }

    float stringToMinute(string time)
    {
        //形式は"hh:mm:ss.ss"
        string[] columns = time.Split(":");
        return 60f * float.Parse(columns[0]) + float.Parse(columns[1]) + (float.Parse(columns[2]) / 60f);
    }        
    
}

public class Detail
{
    public string month;
    public float totalMinutes = 0;
    public int workingDay = 0;
    public float averageTime = 0;
    public string LongestDay = "";
    public float LongestTime = 0;
    public string ShortestDay = "";
    public float ShortestTime = -1;

    public Detail(string monthString)
    {
        month = monthString;
    }
}