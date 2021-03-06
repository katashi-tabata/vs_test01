﻿using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace vs_test01.Dao
{
    using vs_test01.Models;
    public class OracleDao
    {
        private const string SqlGetSchemaName = "select distinct(owner) as OWNER " +
                                                 " from dba_tables " +
                                                " where owner not in ('ADM_PARALLEL_EXECUTE_TASK','ANONYMOUS','APEX_030200','APEX_ADMINISTRATOR_ROLE', " +
                                                                     "'APEX_PUBLIC_USER','APPQOSSYS','AQ_ADMINISTRATOR_ROLE','AQ_USER_ROLE', " +
                                                                     "'AUTHENTICATEDUSER','CONNECT','CSW_USR_ROLE','CTXAPP','CTXSYS', " +
                                                                     "'CWM_USER','DATAPUMP_EXP_FULL_DATABASE','DATAPUMP_IMP_FULL_DATABASE','DBA', " +
                                                                     "'DBFS_ROLE','DBSNMP','DELETE_CATALOG_ROLE','DIP','EJBCLIENT', " +
                                                                     "'EXECUTE_CATALOG_ROLE','EXFSYS','EXP_FULL_DATABASE','FLOWS_030000', " +
                                                                     "'FLOWS_FILES','GATHER_SYSTEM_STATISTICS','GLOBAL_AQ_USER_ROLE', " +
                                                                     "'HS_ADMIN_EXECUTE_ROLE','HS_ADMIN_ROLE','HS_ADMIN_SELECT_ROLE', " +
                                                                     "'IMP_FULL_DATABASE','JAVA_ADMIN','JAVA_DEPLOY','JAVADEBUGPRIV', " +
                                                                     "'JAVAIDPRIV','JAVASYSPRIV','JAVAUSERPRIV','JMXSERVER','LBAC_DBA', " +
                                                                     "'LOGSTDBY_ADMINISTRATOR','MDDATA','MDSYS','MGMT_USER','MGMT_VIEW', " +
                                                                     "'OEM_ADVISOR','OEM_ADVISOR','OEM_MONITOR','OLAP_DBA','OLAP_USER', " +
                                                                     "'OLAP_XS_ADMIN','OLAPSYS','ORACLE_OCM','ORDADMIN','ORDDATA', " +
                                                                     "'ORDPLUGINS','ORDSYS','OUTLN','OWB$CLIENT','OWBSYS','OWBSYS_AUDIT','PUBLIC', " +
                                                                     "'RECOVERY_CATALOG_OWNER','RECOVERY_CATALOG_OWNER','RESOURCE', " +
                                                                     "'SCHEDULER_ADMIN','SELECT_CATALOG_ROLE','SI_INFORMTN_SCHEMA', " +
                                                                     "'SPATIAL_CSW_ADMIN','SPATIAL_CSW_ADMIN_USR','SPATIAL_WFS_ADMIN', " +
                                                                     "'SPATIAL_WFS_ADMIN_USR','SYS','SYSMAN','SYSTEM','TEST', " +
                                                                     "'TSMSYS','WFS_USR_ROLE','WK_TEST','WKPROXY','WKSYS', " +
                                                                     "'WM_ADMIN_ROLE','WMSYS','XDB','XDB_SET_INVOKER','XDB_WEBSERVICES', " +
                                                                     "'XDB_WEBSERVICES_OVER_HTTP','XDB_WEBSERVICES_WITH_PUBLIC','XDBADMIN', " +
                                                                     "'XS$NULL','PERFSTAT')" +
                                                 "order by 1";
        private const string BindStringOwner = "OWNER";
        private const string SqlGetTableName = "select table_name as TABLENAME " +
                                                " from dba_tables " +
                                               " where owner = :" + BindStringOwner +
                                               " order by 1";
        private const string SqlCountTable   = "select count(1) as COUNT from ";

        public static List<SchemaModel> GetUserName (string sid)
        {
            List<SchemaModel> schemas = new List<SchemaModel>();
            using (var Connection = new OracleConnection()){

                Connection.ConnectionString = ConfigurationManager.ConnectionStrings[sid].ConnectionString;
                Connection.Open();

                List<dynamic> records = Connection.Query(SqlGetSchemaName).ToList();
                foreach (dynamic schema in records)
                {
                    SchemaModel model = new SchemaModel();
                    model.SchemaName = schema.OWNER;
                    schemas.Add(model);
                }
            }
            return schemas;
        }

        public static List<TableModel> GetTableName(string sid, string ownername)
        {
            List<TableModel> tables = new List<TableModel>();
            using (var Connection = new OracleConnection())
            {
                Connection.ConnectionString = ConfigurationManager.ConnectionStrings[sid].ConnectionString;
                Connection.Open();

                OracleCommand cmd = new OracleCommand(SqlGetTableName, Connection);
                cmd.Parameters.Add(new OracleParameter(
                    BindStringOwner, OracleDbType.Varchar2,
                    ownername, ParameterDirection.Input));
                OracleDataReader records = cmd.ExecuteReader();
                while ( records.Read() )
                {
                    TableModel model = new TableModel();
                    model.TableName = records.GetOracleString(0).ToString();
                    tables.Add(model);
                }
            }
            return tables;
        }

        public static int CountTable(string sid, string ownername, string tablename)
        {
            List<TableModel> tables = new List<TableModel>();
            int cnt;
            using (var Connection = new OracleConnection())
            {
                Connection.ConnectionString = ConfigurationManager.ConnectionStrings[sid].ConnectionString;
                Connection.Open();


                List<dynamic> records = Connection.Query(
                    SqlCountTable + ownername + "."  + tablename).ToList();
                cnt = (int)records[0].COUNT;
            }
            return cnt;
        }
    }
}
