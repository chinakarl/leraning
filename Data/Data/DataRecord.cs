using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
namespace Data
{
    public interface IRecord
    {
        T Get<T>(int i);
        //T GetOrDefault<T>(int i,int n);
        T Get<T>(string parameterName);
        T GetOrDefault<T>(string parameterName, T defaultName);

        //T GerOrNull<T>(int fieldIndex) where T : class;
    }
    public interface IRecordSet : IDisposable, IRecord
    {
        #region Custom Calls



        #endregion

        int Depth { get; }
        bool IsClosed { get; }


        int RecordsAffected { get; }
        /// <summary>
        /// Returns the database connection to the pool, or closes a non-pooled connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Returns information on the underlying schema.
        /// </summary>
        /// <returns></returns>
        DataTable GetSchemaTable();

        /// <summary>
        /// Move to the next result set.
        /// </summary>
        /// <returns></returns>
        bool NextResult();
        /// <summary>
        /// Read in the next record (or read the 1st record if the result is fresh.)
        /// </summary>
        /// <returns></returns>
        bool Read();
    }
    internal class DataRecord : IRecord, IRecordSet
    {
        private IDataReader wr;

        internal DataRecord(IDataReader wrappedReader)
        {
            wr = wrappedReader;
        }

        #region IDataRecord Members

        public int FieldCount
        {
            get { return wr.FieldCount; }
        }

        public bool GetBoolean(int i)
        {
            return wr.GetBoolean(i);
        }

        public bool GetBooleanFromByte(int i)
        {
            return (wr.GetByte(i) == 1);
        }

        public byte GetByte(int i)
        {
            return wr.GetByte(i);
        }

        public byte GetByteOrDefault(int i, byte defaultValue)
        {
            return (IsDBNull(i)) ? defaultValue : wr.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return wr.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return wr.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return wr.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
            return wr.GetData(i);
        }

        public string GetDataTypeName(int i)
        {
            return wr.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            return wr.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            return wr.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            return wr.GetDouble(i);
        }

        public Type GetFieldType(int i)
        {
            return wr.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            return wr.GetFloat(i);
        }

        public Guid GetGuid(int i)
        {
            return wr.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            return wr.GetInt16(i);
        }

        public int GetInt32FromByte(int i)
        {
            return Convert.ToInt32(wr.GetByte(i));
        }

        public int GetInt32(int i)
        {
            return wr.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            return wr.GetInt64(i);
        }

        public string GetName(int i)
        {
            return wr.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return wr.GetOrdinal(name);
        }

        public string GetString(int i)
        {
            return wr.GetString(i);
        }

        public object GetValue(int i)
        {
            return wr.GetValue(i);
        }

        public int GetValues(object[] values)
        {
            return wr.GetValues(values);
        }

        public bool IsDBNull(int i)
        {
            return wr.IsDBNull(i);
        }

        public object this[string name]
        {
            get { return wr[name]; }
        }

        public object this[int i]
        {
            get { return wr[i]; }
        }

        #endregion

        #region IDataReader Members

        public void Close()
        {
            wr.Close();
        }

        public int Depth
        {
            get { return wr.Depth; }
        }

        public DataTable GetSchemaTable()
        {
            return wr.GetSchemaTable();
        }

        public bool IsClosed
        {
            get { return wr.IsClosed; }
        }

        public bool NextResult()
        {
            return wr.NextResult();
        }

        public bool Read()
        {
            return wr.Read();
        }

        public int RecordsAffected
        {
            get { return wr.RecordsAffected; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            wr.Dispose();
        }

        #endregion

        #region IRecord String Members


        public string GetStringOrDefault(int i, string defaultValue)
        {
            return (IsDBNull(i)) ? defaultValue : GetString(i);
        }

        public string GetStringOrEmpty(int i)
        {
            return GetStringOrDefault(i, String.Empty);
        }

        public string GetStringOrNull(int i)
        {
            return (IsDBNull(i)) ? null : GetString(i);
        }

        public string GetStringOrNullFromCompressedByteArray(int fieldIndex)
        {
            //Byte[] compressedString = GetByteArrayOrNull(fieldIndex);
            //if (compressedString == null || compressedString.Length == 0)
            //{
            //    return null;
            //}
            //else
            //{
            //    return UnicodeStringCompressor.Decompress(compressedString);
            //}

            return null;
        }


        #endregion

        #region IRecord Members

        public short? GetInt16Nullable(int fieldIndex)
        {
            if (IsDBNull(fieldIndex)) return null;

            return GetInt16(fieldIndex);
        }

        public int? GetInt32Nullable(int fieldIndex)
        {
            if (IsDBNull(fieldIndex)) return null;

            return GetInt32(fieldIndex);
        }

        public int GetInt32OrDefault(int fieldIndex, int defaultValue)
        {
            return (IsDBNull(fieldIndex)) ? defaultValue : GetInt32(fieldIndex);
        }

        public int GetInt32OrEmpty(int fieldIndex)
        {
            return GetInt32OrDefault(fieldIndex, Int32.MinValue);
        }

        public int GetInt32OrDefaultFromByte(int fieldIndex, int defaultValue)
        {
            return (IsDBNull(fieldIndex)) ? defaultValue : GetInt32FromByte(fieldIndex);
        }

        public int GetInt32OrEmptyFromByte(int fieldIndex)
        {
            return GetInt32OrDefaultFromByte(fieldIndex, Int32.MinValue);
        }

        public short GetInt16OrDefault(int fieldIndex, short defaultValue)
        {
            return (IsDBNull(fieldIndex)) ? defaultValue : GetInt16(fieldIndex);
        }

        public Byte[] GetByteArrayOrNull(int fieldIndex)
        {
            if (IsDBNull(fieldIndex))
            {
                return null;
            }
            Byte[] bytes = new Byte[(GetBytes(fieldIndex, 0, null, 0, int.MaxValue))];
            GetBytes(fieldIndex, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public DateTime GetDateTimeOrEmpty(int fieldIndex)
        {
            return (IsDBNull(fieldIndex)) ? DateTime.MinValue : GetDateTime(fieldIndex);
        }

        public SqlXml GetSqlXml(int fieldIndex)
        {
            if (IsDBNull(fieldIndex))
                return null;

            SqlDataReader sdr = wr as SqlDataReader;
            if (sdr == null)
                return null;

            return sdr.GetSqlXml(fieldIndex);
        }

        #endregion

        #region IRecord Members

        public T Get<T>(int i)
        {
            if (typeof(string) == typeof(T))
            {
                object defaultvalue = "";
                return IsDBNull(i) ? (T)defaultvalue : (T)this[i];
            }

            return (T)this[i];
        }

        public T Get<T>(string fieldName)
        {
            if (typeof(string) == typeof(T))
            {
                object defaultvalue = "";
                return (IsDBNull(this.GetOrdinal(fieldName))) ? (T)defaultvalue : (T)this[fieldName];
            }
            return (T)this[fieldName];
        }


        public T GetOrDefault<T>(int i, T defaultValue)
        {
            return (IsDBNull(i)) ? defaultValue : Get<T>(i);
        }

        public T GetOrDefault<T>(string fieldName, T defaultValue)
        {

            return (IsDBNull(this.GetOrdinal(fieldName))) ? defaultValue : Get<T>(fieldName);
        }

        public T GetOrNull<T>(int fieldIndex) where T : class
        {
            return (IsDBNull(fieldIndex)) ? null : Get<T>(fieldIndex);
        }

        public T GetOrNull<T>(string fieldName) where T : class
        {
            return (IsDBNull(this.GetOrdinal(fieldName))) ? null : Get<T>(fieldName);
        }


        #endregion


        #region IRecord Members


        public bool GetBooleanOrFalse(int fieldIndex)
        {
            return (IsDBNull(fieldIndex)) ? false : GetBoolean(fieldIndex);
        }

        #endregion
    }
}
