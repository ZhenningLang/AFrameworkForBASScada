using PluginDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;

namespace PluginFAS
{
    [Export(typeof(AbstractPlugin))]
    public class Plugin : AbstractPlugin, Observer
    {
        // view const
        private static readonly String pluginId = "Fas Plugin";
        private static readonly String menuID = "Fas";
        private static readonly String[] menuItemsID = { "Report Fas Alarm" };
        private static readonly String viewSwitchMenuId = "Fas Module";
        private static readonly String[] viewSwitchMenuItemsId = { "Fas Alarm Page", "Fas Temperature Page" };
        // event enum
        public enum ReceivableEventsIdEnum
        {
            ReportFasAlarm
        }
        public enum EmittedEventsIdEnum
        {
            FasFireAlarmAppear,
            FasFireAlarmDisappear
        }
        // constructor
        FakeTemperature temp1 = null;
        FakeTemperature temp2 = null;
        FasAlarm alarm = null;
        AlarmPage alarmPage = null;
        TempPage tempPage = null;
        public Plugin()
        {
            temp1 = new FakeTemperature("LeftTop");
            temp2 = new FakeTemperature("RightBottom");
            temp1.easyAlarm = true;
            tempPage = new TempPage();
            temp1.addObserver(tempPage);
            temp2.addObserver(tempPage);

            alarm = new FasAlarm();
            temp1.addObserver(alarm);
            temp2.addObserver(alarm);
            alarmPage = new AlarmPage();
            alarm.observers.Add(alarmPage);
            alarm.observers.Add(this);
        }

        public void action(Object obj)
        {
            if (sendEvent != null)
            {
                if (((FasAlarm)obj).alarm)
                {
                    sendEvent(new BasEvent() 
                    {
                        eventSource = this.getPluginId(),
                        eventDestination = new List<string>() { "Door Plugin" },
                        eventName = EmittedEventsIdEnum.FasFireAlarmAppear.ToString(),
                        eventParas = null
                    });
                }
                else
                {
                    sendEvent(new BasEvent()
                    {
                        eventSource = this.getPluginId(),
                        eventDestination = new List<string>() { "Door Plugin" },
                        eventName = EmittedEventsIdEnum.FasFireAlarmDisappear.ToString(),
                        eventParas = null
                    });
                }
            }
        }

        // override
        override public String getPluginId()
        {
            return Plugin.pluginId;
        }
        override public String getMenuId()
        {
            return Plugin.menuID;
        }
        override public List<String> getMenuItemsId()
        {
            return new List<string>(Plugin.menuItemsID);
        }
        override public String getViewSwitchMenuId()
        {
            return Plugin.viewSwitchMenuId;
        }

        override public List<String> getViewSwitchMenuItemsId()
        {
            return new List<string>(Plugin.viewSwitchMenuItemsId);
        }

        Dictionary<string, Page> id2page = new Dictionary<string, Page>();

        override public Dictionary<String, Page> getViewSwitchPages()
        {
            if (id2page.Count == 0)
            {
                id2page.Add(viewSwitchMenuItemsId[0], alarmPage);
                id2page.Add(viewSwitchMenuItemsId[1], tempPage);
            }
            return id2page;
        }
        /*override public Dictionary<String, Thread> getViewSwitchThreads() 
        { 
            return null;
        }*/
        override public void confirmAlarm(BasAlarm basAlarm)
        {
        }
        override protected void eventHandler(BasEvent e)
        {
            if (e.eventName.Trim().ToLower().Equals(ReceivableEventsIdEnum.ReportFasAlarm.ToString().Trim().ToLower())) 
            {
                this.alarm.alarm = true;
            }
        }
    }

    public interface Observer 
    {
        void action(Object obj);
    }

    public class FakeTemperature 
    {
        public String temperature = "28.0";
        public String showTemp 
        { 
            get 
            {
                return this.temperature + "℃";
            }
        }
        public String pos = "";
        public List<Observer> observers = new List<Observer>();
        public void addObserver(Observer obs)
        {
            this.observers.Add(obs);
        }
        public void removeObserver(Observer obs)
        {
            this.observers.Remove(obs);
        }
        public FakeTemperature(String pos) 
        {
            this.pos = pos;
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.generateTemperature);
            timer.Start();
        }
        public Boolean easyAlarm = false;
        private void generateTemperature(object sender, System.Timers.ElapsedEventArgs e) 
        {
            if (easyAlarm)
            {
                if (double.Parse(temperature) > 50.0)
                {
                    temperature = (new Random().NextDouble() * 20 + 48).ToString("F2");
                }
                else
                {
                    temperature = (new Random().NextDouble() * 20 + 32).ToString("F2");
                }
            }
            else
            {
                temperature = (new Random().NextDouble() * 2 + 26).ToString("F2");
            }
            foreach (var obs in observers) 
            {
                obs.action(this);
            }
        }
    }
    public class FasAlarm : Observer
    {
        public volatile Boolean alarm = false;
        public List<Observer> observers = new List<Observer>();

        public void setAlarm(Boolean alarm)
        {
            this.alarm = alarm;
            foreach (var obs in observers)
            {
                obs.action(this);
            }
        }
        public void action(Object obj) 
        {
            if (((FakeTemperature)obj).pos.Equals("RightBottom")) 
            {
                return;
            }
            if (alarm)
            {
                if (double.Parse(((FakeTemperature)obj).temperature) < 50.0)
                {
                    alarm = false;
                    foreach (var obs in observers)
                    {
                        obs.action(this);
                    }
                }
            }
            else
            {
                if (double.Parse(((FakeTemperature)obj).temperature) > 50.0)
                {
                    alarm = true;
                    foreach (var obs in observers)
                    {
                        obs.action(this);
                    }
                }
            }
        }
    }

}
