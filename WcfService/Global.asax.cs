using Autofac.Integration.Wcf;
using System;
using WcfService.Config;

namespace WcfService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            AutofacHostFactory.Container = Bootstrap.Execute();
        }
    }
}