using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XmlNode parentNode;
        XmlDocument xml;
        List<Job> jobs;
        public MainWindow()
        {
            InitializeComponent();
            xml = new XmlDocument();
            xml.Load(@".\DateFile.xml");
            parentNode = xml.SelectSingleNode("data");
            ChangeList();
        }

        private void ChangeList()
        {
            XmlNodeList xmlList = parentNode.ChildNodes;
            DateTime date = DateTime.Now;
            jobs = new List<Job>();
            List<Job> jobF = new List<Job>();
            List<String> data = new List<string>();
            foreach (XmlNode chil in xmlList)
            {
                Job job = new Job();
                XmlNodeList chilNodeList = chil.ChildNodes;
                if (chilNodeList.Count > 0)
                {
                    job.Name = chilNodeList.Item(0).InnerText;
                    job.TimeStr = chilNodeList.Item(1).InnerText;
                    job.xmlchil = chil;
                    job.isFinsh = int.Parse(chilNodeList.Item(2).InnerText);
                    job.Time = Convert.ToDateTime(job.TimeStr);
                    if (job.isFinsh == 0)
                    {
                        TimeSpan time1 = new TimeSpan(date.Ticks);
                        TimeSpan time2 = new TimeSpan(job.Time.Ticks);
                        TimeSpan time3 = time2.Subtract(time1);
                        job.TimeDiff = time3.Days;
                        jobs.Add(job);
                    }
                    else if (job.Time > date.AddDays(-2))
                    {
                        jobF.Add(job);
                    }
                }
            }
            jobs.Sort((a,b)=>a.TimeDiff.CompareTo(b.TimeDiff));
            jobs.AddRange(jobF);
            jobs.ForEach(x =>
            {
                String isFinsh = x.isFinsh == 0 ? "未完成" : "已完成";
                data.Add(x.Name+"  "+x.TimeStr+" 差"+x.TimeDiff+ "天  "+ isFinsh);
            });
            ListView1.ItemsSource = data;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox1.IsEnabled = true;
            DatePicker1.IsEnabled = true;
            saveButton.IsEnabled = true;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox1.IsEnabled = false;
            DatePicker1.IsEnabled = false;
            saveButton.IsEnabled = false;
            String jobName = TextBox1.Text;
            if (DatePicker1.SelectedDate == null)
            {
                DatePicker1.SelectedDate = DateTime.Now;
            }
            DateTime dateTime = (DateTime)DatePicker1.SelectedDate;
            XmlElement xmlElementP = xml.CreateElement("job");
            XmlElement xmlElementName = xml.CreateElement("Name");
            XmlElement xmlElementTime = xml.CreateElement("Time");
            XmlElement xmlElementIsFinsh = xml.CreateElement("isFinsh");
            xmlElementName.InnerText = jobName;
            xmlElementTime.InnerText = dateTime.ToShortDateString();
            xmlElementIsFinsh.InnerText = "0";
            xmlElementP.AppendChild(xmlElementName);
            xmlElementP.AppendChild(xmlElementTime);
            xmlElementP.AppendChild(xmlElementIsFinsh);
            parentNode.AppendChild(xmlElementP);
            xml.Save(@".\DateFile.xml");
            
            ChangeList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String selectStr = ListView1.SelectedItem.ToString();
            Job j = jobs.Find(x=> selectStr.Equals(x.Name + "  " + x.TimeStr + " 差" + x.TimeDiff + "天  " + (x.isFinsh == 0 ? "未完成" : "已完成")));
            parentNode.RemoveChild(j.xmlchil);
            xml.Save(@".\DateFile.xml");
            ChangeList();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            String selectStr = ListView1.SelectedItem.ToString();
            Job j = jobs.Find(x => selectStr.Equals(x.Name + "  " + x.TimeStr + " 差" + x.TimeDiff + "天  " + (x.isFinsh == 0 ? "未完成" : "已完成")));
            j.xmlchil.ChildNodes.Item(2).InnerText = "1";
            xml.Save(@".\DateFile.xml");
            ChangeList();
        }
    }
}
