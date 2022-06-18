using System;
using System.Data;
using System.Data.SqlClient;

namespace CyberGameManage
{
    internal class SqlDataApdapter
    {
        private SqlCommand command;

        public SqlDataApdapter(SqlCommand command)
        {
            this.command = command;
        }
    }
}