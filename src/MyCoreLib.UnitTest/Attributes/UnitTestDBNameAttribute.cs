﻿using System;

namespace MyCoreLib.UnitTest.Attributes
{
    public class UnitTestDBNameAttribute: Attribute
    {
        private string _dbName;

        public UnitTestDBNameAttribute(string databaseName)
        {
            _dbName = databaseName;
        }

        /// <summary>
        /// the database name the unit test will be connecting to
        /// </summary>
        public string DBName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }
    }
}
