using System;
using UnityEngine;
using UnityEngine.UI;

public class DigitalClock : MonoBehaviour
{
    public Text hour;
    public Text minute;
    public Button incrementBtn;
    public Button modeBtn;
    public Button setAlarmBtn;
    public Button snoozeBtn;

    private Clock clock;

    void Start()
    {
        this.incrementBtn.onClick.AddListener(OnIncrementClick);
        this.modeBtn.onClick.AddListener(OnModeClick);
        this.modeBtn.onClick.AddListener(OnSetAlarmClick);
        this.modeBtn.onClick.AddListener(OnSnoozeBtnClick);

        this.clock = new Clock();
        DateTime now = DateTime.Now;
        Tiempo newTime = new Tiempo(now.Hour, now.Minute);
        clock.TimeMgr.CurrentTime = newTime;

        clock.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetDisplayTime(this.clock.Display.DisplayTime);
    }

    private void OnIncrementClick()
    {
        if (this.clock.ModeMgr.CurrentMode == Mode.SET_TIME)
        {
            this.clock.TimeMgr.IncrementCurrentMinute();
        }
        else if (this.clock.ModeMgr.CurrentMode == Mode.SET_ALARM)
        {
            this.clock.TimeMgr.IncrementAlarmMinute();
        }
    }

    private void OnModeClick()
    {
        var mode = (int)this.clock.ModeMgr.CurrentMode;
        if(mode + 1 < 3)
        {
            this.clock.ModeMgr.CurrentMode++;
        }
        else
        {
            this.clock.ModeMgr.CurrentMode = Mode.CLOCK_ON;
        }

        SetBtnText(this.modeBtn, this.clock.ModeMgr.CurrentMode.ToString());
    }

    private void OnSetAlarmClick()
    {
        if (this.clock.Alarm.alarmEnabled)
        {
            this.clock.Alarm.Off();
            SetBtnText(this.setAlarmBtn, "ALARM_OFF");
        }
        else
        {
            this.clock.Alarm.On();
            SetBtnText(this.setAlarmBtn, "ALARM_ON");
        }
    }

    private void OnSnoozeBtnClick()
    {
        this.clock.TimeMgr.Snooze();
    }

    private void SetBtnText(Button btn, string btnText)
    {
        btn.GetComponentInChildren<Text>().text = btnText;
    }


    private void SetDisplayTime(Tiempo time)
    {
        if (time == null) { time = new Tiempo(12, 0); }

        var hour = time.Hour.ToString();
        var hourText = hour.PadLeft(2, '0');

        var minute = time.Minute.ToString();
        var minuteText = minute.PadLeft(2, '0');

        this.hour.text = hourText + " : ";
        this.minute.text = minuteText;

        this.minute.text += time.TimeOfday.ToString();
    }
}
