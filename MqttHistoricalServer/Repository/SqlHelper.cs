using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MqttHistoricalServer.Repository
{
    internal static class SqlHelper
    {
        public static string GetString(SqlDataReader dr, string sFieldName)
        {
            int iOrdinal = dr.GetOrdinal(sFieldName);
            return dr.IsDBNull(iOrdinal) ? null : dr.GetString(iOrdinal);
        }

        public static byte[] GetBinary(SqlDataReader dr, string sFieldName)
        {
            int iOrdinal = dr.GetOrdinal(sFieldName);
            if (dr.IsDBNull(iOrdinal)) return null;
            return dr.GetSqlBinary(iOrdinal).Value;
        }

        public static decimal? GetDecimal(SqlDataReader dr, string sFieldName)
        {
            int iOrdinal = dr.GetOrdinal(sFieldName);
            return dr.IsDBNull(iOrdinal) ? (decimal?)null : dr.GetDecimal(iOrdinal);
        }

        public static int? GetInt(SqlDataReader dr, string sFieldName)
        {
            int iOrdinal = dr.GetOrdinal(sFieldName);
            return dr.IsDBNull(iOrdinal) ? (int?)null : dr.GetInt32(iOrdinal);
        }

        public static long? GetLong(SqlDataReader dr, string sFieldName)
        {
            int iOrdinal = dr.GetOrdinal(sFieldName);
            return dr.IsDBNull(iOrdinal) ? (long?)null : dr.GetInt64(iOrdinal);
        }

        public static bool? GetBool(SqlDataReader dr, string sFieldName)
        {
            int iOrdinal = dr.GetOrdinal(sFieldName);
            return dr.IsDBNull(iOrdinal) ? (bool?)null : (dr.GetInt32(iOrdinal) == 0 ? false : true);
        }

        public static DateTime? GetDateTime(SqlDataReader dr, string sFieldName)
        {
            int iOrdinal = dr.GetOrdinal(sFieldName);
            return dr.IsDBNull(iOrdinal) ? (DateTime?)null : dr.GetDateTime(iOrdinal);
        }
        
        public static SqlParameter CreateParam(string name, string value, bool nullable = false)
        {
            var param = new SqlParameter(name, SqlDbType.NVarChar);
            param.IsNullable = true;
            param.Value = nullable ? (string.IsNullOrEmpty(value) ? DBNull.Value : value as object) : value;
            return param;
        }

        public static SqlParameter CreateParam(string name, byte[] value, bool nullable = false)
        {
            var param = new SqlParameter(name, SqlDbType.VarBinary);
            param.IsNullable = true;
            param.Value = nullable ? (value==null ? DBNull.Value : value as object) : value;
            return param;
        }

        public static SqlParameter CreateParam(string name, int? value, bool nullable = false)
        {
            var param = new SqlParameter(name, SqlDbType.Int);
            param.IsNullable = true;
            param.Value = nullable ? (!value.HasValue ? DBNull.Value : value as object) : value;
            return param;
        }

        public static SqlParameter CreateParam(string name, long? value, bool nullable = false)
        {
            var param = new SqlParameter(name, SqlDbType.BigInt);
            param.IsNullable = true;
            param.Value = nullable ? (!value.HasValue ? DBNull.Value : value as object) : value;
            return param;
        }

        public static SqlParameter CreateParam(string name, bool? value, bool nullable = false)
        {
            return CreateParam(name, value.HasValue ? (value.Value ? 1 : 0) : (int?)null, nullable);
        }

        public static SqlParameter CreateParam(string name, DateTime? value, bool nullable = false)
        {
            var param = new SqlParameter(name, SqlDbType.DateTime);
            param.IsNullable = true;
            param.Value = nullable ? (!value.HasValue ? DBNull.Value : value as object) : value;
            return param;
        }

        public static SqlParameter CreateParam(string name, decimal? value, bool nullable = false)
        {
            var param = new SqlParameter(name, SqlDbType.Decimal);
            param.IsNullable = true;
            param.Precision = 10;
            param.Scale = 6;
            param.Value = nullable ? (!value.HasValue ? DBNull.Value : value as object) : value;
            return param;
        }
    }
}
