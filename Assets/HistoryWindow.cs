using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HistoryWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI taskName;
    [SerializeField] TextMeshProUGUI month;
    [SerializeField] TextMeshProUGUI totalTime;
    [SerializeField] TextMeshProUGUI totalDay;
    [SerializeField] TextMeshProUGUI workingDay;
    [SerializeField] TextMeshProUGUI averageTime;
    [SerializeField] TextMeshProUGUI longestDay;
    [SerializeField] TextMeshProUGUI shortestDay;
    [SerializeField] TextMeshProUGUI startingDates;
    [SerializeField] GameObject taskRight;
    [SerializeField] GameObject taskLeft;
    [SerializeField] GameObject detailRight;
    [SerializeField] GameObject detailLeft;
    // Start is called before the first frame update

    List<Detail> currentDetails;
    List<string> taskNames;
    int taskPage;
    int detailPage;

    void Start()
    {  
        taskPage = 0;
        detailPage = 0;
    }

    public void ShowTask(int page)
    {
        taskNames = History.Instance.savedTaskNames;
        if (page < 0 || page >= taskNames.Count) return;
        currentDetails = HistoryCalc.Instance.GetHistoryDatails(taskNames[page]);
        if (currentDetails.Count == 0) return;
        taskRight.SetActive(page != taskNames.Count - 1);
        taskLeft.SetActive(page != 0);
        taskPage = page;
        taskName.text = taskNames[page];
        ShowMonth(0);
        startingDates.text = HistoryCalc.Instance.GetWhenTaskStarted(taskNames[page]);
    }

    void ShowMonth(int page)
    {
        detailPage = page;
        detailRight.SetActive(page != currentDetails.Count - 1);
        detailLeft.SetActive(page != 0);
        Detail showingDetail = currentDetails[page];
        float totalHour = showingDetail.totalMinutes / 60f;
        float totalDays = totalHour / 24f;
        //•\Ž¦
        month.text = showingDetail.month;
        totalTime.text = $"{totalHour:F1}h";
        totalDay.text = $"{totalDays:F1}days";
        workingDay.text = $"{showingDetail.workingDay}";
        averageTime.text = $"{showingDetail.averageTime:F1}h";
        longestDay.text = $"{showingDetail.LongestDay} {(showingDetail.LongestTime / 60f):F1}h";
        shortestDay.text = $"{showingDetail.ShortestDay} {(showingDetail.ShortestTime / 60f):F1}h";
    }

    public void OnClickTaskNext(bool next)
    {
        if (next) ShowTask(taskPage + 1);
        else ShowTask(taskPage - 1);
    }

    public void OnClickDetailNext(bool next)
    {
        if (next) ShowMonth(detailPage + 1);
        else ShowMonth(detailPage - 1);
    }

    
}
