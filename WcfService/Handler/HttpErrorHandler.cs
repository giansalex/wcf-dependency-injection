using Elmah;
using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace WcfService.Handler
{
    public class HttpErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            return false;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error == null) return;

            ErrorLog.GetDefault(null).Log(new Error(error));
        }
    }
}