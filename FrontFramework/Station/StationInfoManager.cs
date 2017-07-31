using FrontFramework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontFramework.Station
{
    class StationInfoManager
    {
        private static StationInfoManager instance = new StationInfoManager() { };
        private StationInfoManager()
        {
            Dictionary<String, Dictionary<String, String>> stationDic = XmlBasedPropUtil.getInstance().getListStrProp("stations");
            foreach (var station in stationDic)
            {
                String id = station.Key;
                int stationCode = int.Parse(station.Value["stationCode"]);
                Boolean isTransfer = false;
                List<String> transLines = null;
                if (station.Value.ContainsKey("transfer"))
                { 
                    isTransfer = true;
                    String[] tranferList = station.Value["transfer"].Split(' ');
                    transLines = new List<string>(tranferList);
                }
                stationInfoList.Add(new StationInfo(){
                    id = id,
                    stationCode = stationCode,
                    isTransfer = isTransfer,
                    transLines = transLines
                });
            }
        }
        public static StationInfoManager getInstance() 
        {
            return instance;
        }

        private List<StationInfo> stationInfoList = new List<StationInfo>();
        public List<StationInfo> getStationInfoList() 
        {
            return this.stationInfoList;
        }
    }

    class StationInfo
    {
        public String id { get; set; }
        public int stationCode { get; set; }
        public Boolean isTransfer { get; set; }
        public List<String> transLines { get; set; }
    }

}
