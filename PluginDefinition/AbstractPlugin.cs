using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Communication.FCMP;
using Communication.Redis;
using Enumeration;
using Translation;

namespace PluginDefinition
{

    public abstract class AbstractPlugin : ComponentDynamicTranslate
    {
        protected int stationCode;
        protected String userName = "";
        protected OperationLevelEnum operationLevel = OperationLevelEnum.OBSERVER;
        protected ConcurrentQueue<BasEvent> basEventQueue = new ConcurrentQueue<BasEvent>();
        protected Communication.IComm fcmpComm = FCMPCommunication.getFCMPCommInstance();
        protected Communication.IComm redisComm = RedisCommunication.getRedisCommInstance();
        private Thread listeningThread = null;

        public AbstractPlugin()
        {
            pluginInitial();
        }
        public AbstractPlugin(int stationCode, String userName, OperationLevelEnum operationLevel)
        {
            this.stationCode = stationCode;
            this.userName = userName;
            this.operationLevel = operationLevel;
            pluginInitial();
        }

        public void pluginInitial()
        {
            // 监听线程初始化
            listeningThread = new Thread(new ThreadStart(this.observeBasEventQueue));
            listeningThread.Name = "Thread: " + getPluginId();
            listeningThread.Start();
            // 
            LanguageChangedNotifier.getInstance().addListener(this);
        }
        public abstract void initializeComponentContents(); // a hook
        ~AbstractPlugin()
        {
            LanguageChangedNotifier.getInstance().removeListener(this);
        }

        private volatile Boolean observerControl = true;
        private void observeBasEventQueue()
        {
            while (observerControl)
            {
                try
                {
                    if (basEventQueue.IsEmpty == false)
                    {
                        BasEvent eventTemp;
                        basEventQueue.TryDequeue(out eventTemp);
                        eventHandler(eventTemp);
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                catch (Exception er)
                {
                    Console.WriteLine(er.ToString());
                }
            }
        }
        public void closeListeningThread()
        {
            observerControl = false;
        }

        #region 全局属性设置

        public void setStationCode(int stationCode) // 设置站码
        {
            this.stationCode = stationCode;
            stationCodeChanged();
        }
        public void setUserName(String userName) // 设置站码
        {
            this.userName = userName;
        }
        public void setOperationLevel(OperationLevelEnum operationLevel) // 设置站码
        {
            this.operationLevel = operationLevel;
        }
        protected abstract void stationCodeChanged();

        #endregion

        #region 界面显示内容获取

        /// <summary>
        /// 获取插件 id
        /// </summary>
        /// <returns></returns>
        public abstract String getPluginId();

        /// <summary>
        /// 获取菜单树的根节点
        /// 每个菜单触发点击事件时，框架会以 menuID 的事件 ID 发送给插件自行处理
        /// </summary>
        /// <returns></returns>
        public abstract MenuNode getMenuRoot();

        /// <summary>
        /// 获取界面切换菜单 id
        /// 被点击时，会通知插件
        /// </summary>
        /// <returns></returns>
        public abstract String getViewSwitchMenuId();

        /// <summary>
        /// 获取界面切换菜单子项 id
        /// 页面切换时，框架会以 switchMenuItemId 的事件 ID 发送给插件自行处理
        /// </summary>
        /// <returns></returns>
        public abstract List<String> getViewSwitchMenuItemsId();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<String, Page> getViewSwitchPages(); // 获取具体每个页面的引用 (switchMenuItemId --- Page)
        //public abstract Dictionary<String, Thread> getViewSwitchThreads(); // 获取具体每个页面线程 (界面切换菜单子项 id --- 线程)

        #endregion

        #region 事件触发接口

        /// <summary>
        /// 主界面调用这个接口通知插件有事件发生，包括菜单点击事件和跨插件事件通知
        /// BasEvent 中主界面的 eventSource 值为 "MainWindow"
        /// 其他 eventName、eventParas 等等均由<b>开发者之间通过文档协定</b>
        /// </summary>
        /// <param name="e"></param>
        public void trigger(BasEvent e)
        {
            basEventQueue.Enqueue(e);
        }
        /// <summary>
        /// 处理 trigger 函数传进来的 BasEvent 的处理函数，子类需要实现其具体内容
        /// </summary>
        /// <param name="e"></param>
        protected abstract void eventHandler(BasEvent e);

        #endregion

        #region 监听注册

        public delegate void SystemEventHandler(BasEvent basEvent);
        public SystemEventHandler systemEventHandler;

        #endregion

        #region 报警消息处理相关

        public delegate void BasAlarmHandler(BasAlarm basAlarm);
        public BasAlarmHandler basAlarmHandler;
        public abstract void confirmAlarm(BasAlarm basAlarm);
        public delegate void BasAlarmSolvedHandler(BasAlarm basAlarm);
        public BasAlarmSolvedHandler basAlarmSolvedHandler;

        #endregion
        #region 报警消息处理相关
        public void setLanguage() { }
        #endregion
    }

    public class BasAlarm
    {
        public String pluginId { get; set; }
        public String alarmId { get; set; }
        public String alarmType { get; set; }
        public AlarmLevelEnum alarmLevel { get; set; }
        public String alarmTime { get; set; }
        public String alarmLocation { get; set; }
        public String alarmSystem { get; set; }
        public String alarmDescription { get; set; }

    }
    public class BasEvent
    {
        public BasEvent() { }
        public String eventSource { get; set; }
        public Boolean isBroadcast { get; set; }
        public List<String> eventDestination { get; set; }
        public String eventName { get; set; }
        public List<Object> eventParas { get; set; }
        override public String ToString() 
        {
            String str = "from: " + eventSource;
            if (isBroadcast)
            {
                str += "\n Broadcast ";
            }
            else
            {
                foreach (var toName in eventDestination)
                {
                    str += "\n to: " + toName.ToString();
                }
            }
            return str + "\n event name: " + eventName + "\n";
        }
    }

    public enum AlarmLevelEnum
    {
        I = 1,
        II = 2,
        III = 3
    }

    public class MenuNode 
    {
        private String menuID;
        private List<MenuNode> childrenMenus = new List<MenuNode>();
        public String getMenuID()
        {
            return this.menuID;
        }
        public void setMenuID(String menuID) 
        {
            this.menuID = menuID;
        }
        public List<MenuNode> getChildrenMenus()
        {
            return this.childrenMenus;
        }
    }

}
