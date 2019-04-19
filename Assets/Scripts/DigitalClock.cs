using System;
using UnityEngine;
using UnityEngine.UI;

public class DigitalClock : MonoBehaviour
{
    public Camera camera;
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
        this.setAlarmBtn.onClick.AddListener(OnSetAlarmClick);
        this.snoozeBtn.onClick.AddListener(OnSnoozeBtnClick);

        this.clock = new Clock();
        DateTime now = DateTime.Now;
        Tiempo newTime = new Tiempo(now.Hour, now.Minute);
        clock.TimeMgr.CurrentTime = newTime;
        this.clock.Display.DisplayTime = newTime;
        SetDisplayTime(clock.TimeMgr.CurrentTime);

        this.clock.Alarm.alarmEvent += Alarm_alarmEvent;
        clock.Start();
    }

    void FixedUpdate()
    {
        SetDisplayTime(this.clock.Display.DisplayTime);
    }

    #region Event Handlers
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
            this.incrementBtn.GetComponentInChildren<Image>().gameObject.SetActive(true);
        }
        else
        {
            this.clock.ModeMgr.CurrentMode = Mode.CLOCK_ON;
            this.incrementBtn.GetComponentInChildren<Image>().gameObject.SetActive(false);
        }

        SetBtnText(this.modeBtn, this.clock.ModeMgr.CurrentMode.ToString());
    }

    private void OnSetAlarmClick()
    {
        if (this.clock.Alarm.alarming)
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
        this.snoozeBtn.GetComponentInChildren<Image>().gameObject.SetActive(false);
        camera.backgroundColor = Color.blue;
    }

    private void Alarm_alarmEvent(object sender, EventArgs e)
    {
        this.snoozeBtn.GetComponentInChildren<Image>().gameObject.SetActive(true);
        camera.backgroundColor = Color.red;
    }
    #endregion

    //Set Btn Text
    private void SetBtnText(Button btn, string btnText)
    {
        btn.GetComponentInChildren<Text>().text = btnText;
    }

    // Set Text Component Values
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
