using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace WpfApp
{
    public class Job
    {
        
        /// <summary>
        /// 任务名
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 时间字符串
        /// </summary>
        public String TimeStr { get; set; }
        /// <summary>
        /// 任务时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 完成为1 未完成为0
        /// </summary>
        public int isFinsh { get; set; }
        /// <summary>
        /// 对应节点
        /// </summary>
        public XmlNode xmlchil { get; set; }
        /// <summary>
        /// 时间差
        /// </summary>
        public int TimeDiff { get; set; }

        public Job() { }
        
    }
}
