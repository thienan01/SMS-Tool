﻿using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_Speed.Utility
{
    class DBConenection
    {
        private  static OdbcConnection _instance = null;
        private DBConenection() {
        }

        public static OdbcConnection getInstance()   
        {
            if(_instance == null)
            {
                _instance = new OdbcConnection("DSN=PixelSqlbase;UID=DBA;ENP=28f3cd0c3ddcfc32");
            }
            return _instance;
        }
    }
}
