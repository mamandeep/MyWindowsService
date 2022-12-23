using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;
using System.Threading;

namespace CUPBWindowsService
{
    public partial class Service1 : ServiceBase
    {
        int ScheduleTime = Convert.ToInt32(ConfigurationSettings.AppSettings["ThreadTime"]);
        public Thread Worker = null;
        private static String RootFolder = @"C:\Users\Administrator\AppData\Local\Microsoft\Windows";

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ThreadStart start = new ThreadStart(Working);
                Worker = new Thread(start);
                Worker.Start();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Working()
        {
            while (true)
            {
                using (StreamWriter writer = new StreamWriter(RootFolder, true))
                {
                    writer.WriteLine(string.Format("Backup Scheduled on"+DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")+""));
                    writer.Close();
                }

                Thread.Sleep(ScheduleTime * 60 * 1000);

                /*private void btnLock_Click(object sender, EventArgs e)
                {
                    try
                    {

                        string folderPath = textBox1.Text;
                        string adminUserName = Environment.UserName;// getting your adminUserName
                        DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                        FileSystemAccessRule fsa = new FileSystemAccessRule(adminUserName, FileSystemRights.FullControl, AccessControlType.Deny)
                        ds.AddAccessRule(fsa);
                        Directory.SetAccessControl(folderPath, ds);
                        MessageBox.Show("Locked");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                private void btnUnLock_Click(object sender, EventArgs e)
                {
                    try
                    {
                        string folderPath = textBox1.Text;
                        string adminUserName = Environment.UserName;// getting your adminUserName
                        DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                        FileSystemAccessRule fsa = new FileSystemAccessRule(adminUserName, FileSystemRights.FullControl, AccessControlType.Deny)
                      ds.RemoveAccessRule(fsa);
                        Directory.SetAccessControl(folderPath, ds);
                        MessageBox.Show("UnLocked");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }*/
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (Worker != null && Worker.IsAlive)
                {
                    Worker.Abort();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
