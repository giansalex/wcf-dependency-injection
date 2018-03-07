using Elmah;
using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace WcfService.Handler
{
    public class ElmahErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            return false;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error == null || HttpContext.Current == null)
            {
                return;
            }

            ErrorSignal.FromCurrentContext().Raise(error);
        }
    }
}