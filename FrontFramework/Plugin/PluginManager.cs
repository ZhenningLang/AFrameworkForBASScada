using FrontFramework.Utils;
using PluginDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FrontFramework.Plugin
{
    public class PluginManager
    {
        private static PropertyUtilInterface propUtil = PropUtilFactory.getPropUtil();

        private static PluginManager manager = new PluginManager();
        private PluginManager()
        {
            loadPlugins();
        }
        public static PluginManager getPluginManager()
        {
            return manager;
        }

        // plugin configs
        public List<String> loadingPaths = new List<string>();
        public List<PluginInfo> pluginInfos = new List<PluginInfo>();
        public void loadPlugins()
        {
            Dictionary<String, Dictionary<String, String>> pathsTemp = propUtil.getListStrProp("pluginLoadingPaths");
            loadingPaths.Clear();
            foreach (var item in pathsTemp) 
            {
                loadingPaths.Add(item.Key.ToString().Trim());
            }
            // 组装
            Compose();
            // 数据组合
            Dictionary<String, Dictionary<String, String>> infosTemp = propUtil.getListStrProp("pluginInfos");
            // shut down original threads
            foreach (PluginInfo pluginInfo in pluginInfos)
            {
                pluginInfo.plugin.closeListeningThread();
            }
            // 清除原有数据
            pluginInfos.Clear();
            foreach (var plugin in plugins)
            {
                plugin.setUserName(MainWindow.getUserName());
                plugin.setOperationLevel(MainWindow.getOperationLevel());
                String pluginID = plugin.getPluginId();
                PluginInfo pluginInfo = new PluginInfo(pluginID, 0, true, plugin);
                if (infosTemp.ContainsKey(pluginID))
                {
                    pluginInfo.position = int.Parse(infosTemp[pluginID]["position"]);
                    pluginInfo.isActive = infosTemp[pluginID]["isActived"].Trim().ToLower().Equals("true") ? true : false;
                }
                pluginInfos.Add(pluginInfo);
            }
            // 排序
            pluginInfos.Sort((info1, info2) => {
                if (info1.position > info2.position) return 1;
                if (info1.position < info2.position) return -1;
                return 0;
            });
        }

        public PluginInfo getPluginInfoByID(String pluginID) 
        {
            return pluginInfos.FindLast((info) =>
            {
                return (info.pluginID.Equals(pluginID));
            });
        }
        public void addLoadingPath(String path) 
        {
            if (!loadingPaths.Contains(path))
            {
                loadingPaths.Add(path);
                storePaths();
                loadPlugins();
            }
        }
        public void deleteLoadingPath(String path)
        {
            if (loadingPaths.Contains(path))
            {
                loadingPaths.Remove(path);
                storePaths();
                loadPlugins();
            }
        }
        private void storePaths() 
        {
            Dictionary<String, Dictionary<String, String>> temp = new Dictionary<String, Dictionary<String, String>>();
            foreach (String path in loadingPaths) 
            {
                temp.Add(path, new Dictionary<String, String>());
            }
            propUtil.setListStrProp("pluginLoadingPaths", temp);
        }
        public void setPluginInfo(String pluginID, int position, bool isActive) 
        {
            PluginInfo findResult = pluginInfos.FindLast((info) =>
            {
                return (info.pluginID.Equals(pluginID));
            });
            findResult.position = position;
            findResult.isActive = isActive;

            Dictionary<String, Dictionary<String, String>> temp = new Dictionary<String, Dictionary<String, String>>();
            foreach (PluginInfo info in pluginInfos) 
            {
                temp.Add(info.pluginID, new Dictionary<String, String>());
                temp[info.pluginID].Add("position", info.position.ToString());
                temp[info.pluginID].Add("isActived", info.isActive.ToString().ToLower());
            }
            propUtil.setListStrProp("pluginInfos", temp);
            loadPlugins();
        }



        [ImportMany(typeof(AbstractPlugin))]
        private List<AbstractPlugin> plugins { get; set; } // 插件

        /// <summary>
        /// 加载插件
        /// </summary>
        private void Compose()
        {
            if (plugins != null)
            {
                plugins.Clear();
            }
            var catalog = new AggregateCatalog();
            foreach (var path in loadingPaths)
            {
                catalog.Catalogs.Add(new DirectoryCatalog(path));
            }

            var container = new CompositionContainer(catalog);
            //将部件（part）和宿主程序添加到组合容器
            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }
    }

    public class PluginInfo 
    {
        public String pluginID;
        public int position;
        public bool isActive;
        public AbstractPlugin plugin;

        public PluginInfo(String pluginID, int position, bool isActive, AbstractPlugin plugin)
        {
            this.pluginID = pluginID;
            this.position = position;
            this.isActive = isActive;
            this.plugin = plugin;
        }

    }

}
