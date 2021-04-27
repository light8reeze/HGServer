using System;
using System.Collections.Generic;
using System.Text;

namespace HGServer.Utility
{
    /// <summary>
    /// Exception Event Handler
    /// </summary>
    /// <param name="sender">exception occured object</param>
    /// <param name="e">occured exception</param>
    public delegate void ExceptionEventHandler(object sender, Exception e);
}
