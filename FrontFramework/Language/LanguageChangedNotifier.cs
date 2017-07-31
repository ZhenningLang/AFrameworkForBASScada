using FrontFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontFramework.Language
{
    /// <summary>
    /// 一个监听者模式，所有监听语言状态改变的空间在语言发生切换时，将自动刷新空间显示
    /// </summary>
    class LanguageChangedNotifier
    {
        private static LanguageChangedNotifier instance = new LanguageChangedNotifier();
        private LanguageChangedNotifier() { }
        public static LanguageChangedNotifier getInstance() 
        {
            return instance;
        }

        private LinkedList<ComponentDynamicTranslate> listenerList = new LinkedList<ComponentDynamicTranslate>();

        public void addListener(ComponentDynamicTranslate listener)
        {
            lock (this)
            {
                try
                {
                    listenerList.AddLast(listener);
                }
                catch (Exception er)
                {
                    Console.WriteLine(er.ToString());
                }
            }
        }
        public void removeListener(ComponentDynamicTranslate listener)
        {
            lock (this)
            {
                try
                {
                    listenerList.Remove(listener);
                }
                catch (Exception er)
                {
                    Console.WriteLine(er.ToString());
                }
            }
        }

        public void notifyAll() 
        {
            foreach (ComponentDynamicTranslate component in listenerList) 
            {
                try 
                {
                    component.initializeComponentContents();
                }
                catch (Exception er) 
                {
                    Console.WriteLine(er.ToString());
                }
            }
        }

    }
}
