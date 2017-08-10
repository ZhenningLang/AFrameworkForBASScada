using FrontFramework.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FrontFramework.Interfaces
{
    /// <summary>
    /// 一个模板类，所有想要动态改变中英文的组件都需要继承这个抽象类
    /// 这个模板在创建的时候将将自身添加进 LanguageChangedNotifier
    /// 销毁时从 LanguageChangedNotifier 中移除
    /// LanguageChangedNotifier 通过调用子类的具体的 initializeComponentContents 方法来改变组件文字
    /// </summary>
    public interface ComponentDynamicTranslate
    {
        void initializeComponentContents();
    }
}
