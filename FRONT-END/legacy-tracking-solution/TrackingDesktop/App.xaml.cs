using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TrackingUxLib.Code.API;
using TrackingUxLib.Code.API.Interfaces;
using TrackingUxLib.Code.Utils;

namespace TrackingDesktop
{
    public partial class App : Application
    {
        #region Props:
        public static UnityContainer Container { get; private set; }
        #endregion

        #region Overrides:

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                #region Register Unity Types:
                Container = new UnityContainer();

                Container.RegisterType<ICustomersEndpoint, CustomersEndpoint>();
                Container.RegisterType<ITerminalsEndpoint, TerminalsEndpoint>();
                #endregion

                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }

        #endregion
    }
}
