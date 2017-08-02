using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PluginDefinition
{

    public abstract class AbstractPlugin
    {
        protected int stationCode;
        protected ConcurrentQueue<BasEvent> basEventQueue = new ConcurrentQueue<BasEvent>();

        public AbstractPlugin()
        {
            pluginInitial();
        }
        public AbstractPlugin(int stationCode)
        {
            this.stationCode = stationCode;
            pluginInitial();
        }

        public void pluginInitial()
        {
            // 监听线程初始化
            Thread listeningThread = new Thread(new ThreadStart(this.observeBasEventQueue));
            listeningThread.Name = "Thread: " + getPluginId();
            listeningThread.Start();
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
        }

        #endregion

        #region 界面显示内容获取

        public abstract String getPluginId(); // 获取插件 id
        public abstract String getMenuId(); // 获取菜单 id
        public abstract List<String> getMenuItemsId(); // 获取菜单子项 id
        public abstract String getViewSwitchMenuId(); // 获取界面切换菜单 id
        public abstract List<String> getViewSwitchMenuItemsId(); // 获取界面切换菜单子项 id
        public abstract Dictionary<String, Page> getViewSwitchPages(); // 获取具体每个页面的引用 (界面切换菜单子项 id --- Page)
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

        public delegate void BasEventHandler(BasEvent basEvent);
        public event BasEventHandler sendEvent;

        #endregion

        #region 报警消息处理相关

        public delegate void BasAlarmHandler(BasAlarm basAlarm);
        public event BasAlarmHandler sendAlarm;
        public abstract void confirmAlarm(BasAlarm basAlarm);
        public delegate void BasAlarmSolvedHandler(BasAlarm basAlarm);
        public event BasAlarmSolvedHandler sendSolvedAlarm;

        #endregion
    }

    public class BasAlarm
    {
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
        public String eventSource { get; set; }
        public String eventDestination { get; set; }
        public String eventName { get; set; }
        public List<Object> eventParas { get; set; }
    }

    public enum AlarmLevelEnum
    {
        I = 1,
        II = 2,
        III = 3
    }
}
