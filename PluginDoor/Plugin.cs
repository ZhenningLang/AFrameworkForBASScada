using PluginDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PluginDoor
{
    [Export(typeof(AbstractPlugin))]
    public class Plugin : AbstractPlugin
    {
        // view const
        private static readonly String pluginId = "Door Plugin";
        private static readonly String menuID = null;
        private static readonly String[] menuItemsID = null;
        private static readonly String viewSwitchMenuId = "Door Module";
        private static readonly String[] viewSwitchMenuItemsId = { "Door Page" };
        // event enum
        public enum ReceivableEventsIdEnum
        {
            FasFireAlarmAppear,
            FasFireAlarmDisappear
        }
        public enum EmittedEventsIdEnum{}
        // constructor
        private DoorPage doorPage = new DoorPage();
        public Plugin()
        {
            observers.Add(doorPage);
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
        // Dictionary<string, Page> id2thread = new Dictionary<string, Page>();
        override public Dictionary<String, Page> getViewSwitchPages()
        {
            if (id2page.Count == 0)
            {
                id2page.Add(viewSwitchMenuItemsId[0], doorPage);
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

        public List<Observer> observers = new List<Observer>();
        override protected void eventHandler(BasEvent e)
        {
            if (e.eventName.Equals(ReceivableEventsIdEnum.FasFireAlarmAppear.ToString())) 
            {
                foreach (var obs in observers) 
                {
                    obs.action("open");
                }
            }
        }
    }

    public interface Observer
    {
        void action(Object obj);
    }
}
