﻿using System;
using Npgsql;
using NpgsqlTypes;

namespace Greatbone.Core
{
    /// <summary>
    /// A wrapper of db parameter collection.
    /// </summary>
    public class DbParameters : ISink<DbParameters>
    {
        static string[] Defaults = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };

        readonly NpgsqlParameterCollection coll;

        internal DbParameters(NpgsqlParameterCollection coll)
        {
            this.coll = coll;
        }

        int index; // current parameter index

        internal void Clear()
        {
            coll.Clear();
            index = 0;
        }

        public DbParameters Put(bool v)
        {
            coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Boolean)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(short v)
        {
            coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Smallint)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(int v)
        {
            coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Integer)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(long v)
        {
            coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Bigint)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(decimal v)
        {
            coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Money)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(DateTime v)
        {
            NpgsqlDbType dt = (v.Hour == 0 && v.Minute == 0 && v.Second == 0 && v.Millisecond == 0) ?
                NpgsqlDbType.Date : NpgsqlDbType.Timestamp;

            coll.Add(new NpgsqlParameter(Defaults[index++], dt)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(char[] v)
        {
            coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Text)
            {
                Value = (v != null) ? (object)v : DBNull.Value
            });
            return this;
        }

        public DbParameters Put(string v)
        {
            coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Text)
            {
                Value = (v != null) ? (object)v : DBNull.Value
            });
            return this;
        }

        public DbParameters Put(byte[] v)
        {
            throw new NotImplementedException();
        }

        public DbParameters Put<T>(T v, int x = -1) where T : IPersist
        {
            throw new NotImplementedException();
        }

        public DbParameters Put(ArraySegment<byte> v)
        {
            throw new NotImplementedException();
        }

        public DbParameters Put(JObj v)
        {
            throw new NotImplementedException();
        }

        public DbParameters Put(JArr v)
        {
            JStrBuild jt = new JStrBuild();
            jt.Put(v);
            string strv = jt.ToString();

            coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Jsonb)
            {
                Value = (v != null) ? (object)strv : DBNull.Value
            });
            return this;
        }

        public DbParameters Put(short[] v)
        {
            coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Array | NpgsqlDbType.Smallint)
            {
                Value = (v != null) ? (object)v : DBNull.Value
            });
            return this;
        }

        public DbParameters Put(int[] v)
        {
            coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Array | NpgsqlDbType.Integer)
            {
                Value = (v != null) ? (object)v : DBNull.Value
            });
            return this;
        }

        public DbParameters Put(long[] v)
        {
            coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Array | NpgsqlDbType.Bigint)
            {
                Value = (v != null) ? (object)v : DBNull.Value
            });
            return this;
        }

        public DbParameters Put(string[] v)
        {
            coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Array | NpgsqlDbType.Varchar)
            {
                Value = (v != null) ? (object)v : DBNull.Value
            });
            return this;
        }

        public DbParameters Put<T>(T[] v, int x = -1) where T : IPersist
        {
            throw new NotImplementedException();
        }

        public DbParameters PutNull()
        {
            throw new NotImplementedException();
        }

        // SINK

        public DbParameters Put(string name, bool v)
        {
            coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Boolean)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(string name, short v)
        {
            coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Smallint)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(string name, int v)
        {
            coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Integer)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(string name, long v)
        {
            coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Bigint)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(string name, decimal v)
        {
            coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Money)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(string name, DateTime v)
        {
            coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Timestamp)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(string name, char[] v)
        {
            coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Varchar, v.Length)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(string name, string v)
        {
            coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Varchar, v.Length)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put(string name, byte[] v)
        {
            coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Bytea, v.Length)
            {
                Value = v
            });
            return this;
        }

        public DbParameters Put<T>(string name, T v, int x = -1) where T : IPersist
        {
            throw new NotImplementedException();
        }

        public DbParameters Put(string name, JObj v)
        {
            throw new NotImplementedException();
        }

        public DbParameters Put(string name, JArr v)
        {
            throw new NotImplementedException();
        }

        public DbParameters Put(string name, short[] v)
        {
            throw new NotImplementedException();
        }

        public DbParameters Put(string name, int[] v)
        {
            coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Array | NpgsqlDbType.Integer)
            {
                Value = (v != null) ? (object)v : DBNull.Value
            });
            return this;
        }

        public DbParameters Put(string name, long[] v)
        {
            coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Array | NpgsqlDbType.Bigint)
            {
                Value = (v != null) ? (object)v : DBNull.Value
            });
            return this;
        }

        public DbParameters Put(string name, string[] v)
        {
            coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Array | NpgsqlDbType.Text)
            {
                Value = (v != null) ? (object)v : DBNull.Value
            });
            return this;
        }

        public DbParameters Put<T>(string name, T[] v, int x = -1) where T : IPersist
        {
            if (v == null)
            {
                coll.Add(new NpgsqlParameter(name, NpgsqlDbType.Jsonb) { Value = DBNull.Value });
            }
            else
            {
                JStrBuild jsb = new JStrBuild();
                jsb.Put(v);
                string strv = jsb.ToString();

                coll.Add(new NpgsqlParameter(Defaults[index++], NpgsqlDbType.Jsonb)
                {
                    Value = strv
                });
            }
            return this;
        }

        public DbParameters PutNull(string name)
        {
            coll.AddWithValue(name, DBNull.Value);
            return this;
        }

    }
}