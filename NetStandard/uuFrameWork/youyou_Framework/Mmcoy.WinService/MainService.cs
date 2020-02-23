using Mmcoy.Framework;
using Mmcoy.Framework.Interface;
using Mmcoy.Framework.WinService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mmcoy.WinService
{
    partial class MainService : ServiceBase
    {
        #region private
        private MFIAppLog log = MFAppLoggerManager.GetLogger(typeof(MainService));
        private System.Timers.Timer timerMain;
        private WinServiceConfigDAL ConfigDAL;
        #endregion

        public MainService()
        {
            InitializeComponent();
            timerMain = new System.Timers.Timer();
            timerMain.Elapsed += new System.Timers.ElapsedEventHandler(timerMain_Elapsed);
            ConfigDAL = WinServiceConfigDAL.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "/WinServiceConfig.xml");
        }

        protected override void OnStart(string[] args)
        {
            log.Info("开始运行");
            this.timerMain.Interval = 1000;
            this.timerMain.Enabled = true;
        }

        protected override void OnStop()
        {
            log.Info("停止运行");
            this.timerMain.Enabled = false;
        }

        #region timerMain_Elapsed
        /// <summary>
        /// timerMain_Elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerMain_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IList<WinServiceConfigEntity> lst = ConfigDAL.GetConfigList();

            foreach (WinServiceConfigEntity entity in lst)
            {
                if (entity.Enabled)
                {
                    IWinService service = entity.ServiceInstance;
                    if (service != null)
                    {
                        if (entity.IsAppointTime)
                        {
                            if (!service.IsBusy && entity.AppointTime.Equals(DateTime.Now.ToString("HH:mm:ss")))
                            {
                                if (service.Config.IsOnce && service.Config.IsAlreadyDoService)
                                {

                                    return;
                                }
                                this.DoServiceInNewThread(service);
                            }
                        }
                        else
                        {
                            if (!service.IsBusy && service.CanDoService())
                            {
                                if (service.Config.IsOnce && service.Config.IsAlreadyDoService)
                                {
                                    log.Info("{0}已经运行过了 不再运行".FormatWith(service.Config.Name));
                                    return;
                                }
                                this.DoServiceInNewThread(service);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region DoServiceInNewThread 在新线程中执行服务
        /// <summary>
        /// 在新线程中执行服务
        /// </summary>
        /// <param name="service"></param>
        private void DoServiceInNewThread(IWinService service)
        {
            try
            {
                service.Config.IsAlreadyDoService = true;
                new Thread(new ThreadStart(service.DoService)).Start();
            }
            catch
            {

            }
        }
        #endregion
    }
}
