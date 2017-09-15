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

        public override void initializeComponentContents()
        {
            Console.WriteLine("Language changed");
        }
        // override
        override public String getPluginId()
        {
            return Plugin.pluginId;
        }
        override public MenuNode getMenuRoot()
        {
            MenuNode menu0 = new MenuNode();
            menu0.setMenuID("Door Menu Root");
            MenuNode menu11 = new MenuNode();
            menu11.setMenuID("Door Menu Level 1-1");
            MenuNode menu12 = new MenuNode();
            menu12.setMenuID("Door Menu Level 1-2");
            MenuNode menu111 = new MenuNode();
            menu111.setMenuID("Door Menu Level 2-1");
            MenuNode menu112 = new MenuNode();
            menu112.setMenuID("Door Menu Level 2-2");
            menu0.getChildrenMenus().Add(menu11);
            menu0.getChildrenMenus().Add(menu12);
            menu11.getChildrenMenus().Add(menu111);
            menu11.getChildrenMenus().Add(menu112);
            return menu0;
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
        override protected void stationCodeChanged()
        {
            Console.WriteLine("Station code is changed to " + this.stationCode);
        }
    }

    public interface Observer
    {
        void action(Object obj);
    }
}
