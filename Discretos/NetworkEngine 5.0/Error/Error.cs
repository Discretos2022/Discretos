using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkEngine_5._0.Error
{
    public class ConnectionError : SystemException
    {

        public ConnectionError()
            : base("Connection failed")
        {
            
        }

    }

    public class FullError : SystemException
    {

        public FullError()
            : base("Server is full !")
        {

        }

    }
    public class RefuseError : SystemException
    {

        public RefuseError()
            : base("Server not accept connection !")
        {

        }

    }

    public class LostConnectionError : SystemException
    {

        public LostConnectionError()
            : base("Server is full !")
        {

        }

    }

}
