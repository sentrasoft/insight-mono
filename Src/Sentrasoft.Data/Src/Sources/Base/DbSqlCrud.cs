/*
 * 
 * INSIGHT-MONO
 * Copyright (c) 2016 Sentrasoft (http://www.sentrasoft.com)
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * Authored by: Fahmi <kontak@fahmi.id>
 * 
 */

using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

namespace Sentrasoft.Data
{

	/// <summary>
	/// SQL Helper (CURD) class used to add, delete, and update data using basic SQL functions with a single command line.
	/// </summary>
	public sealed class DbSqlCrud
	{

		#region Init and Construct
		/// <summary>
		/// The default connection is used.
		/// </summary>
		private DbSql Provider;

		/// <summary>
		/// Used to add, delete, and update data using basic SQL functions with a single command line.
		/// </summary>
		/// <param name="provider">Provider.</param>
		public DbSqlCrud (DbSql provider)
		{
			Provider = provider;
		}
		#endregion

		#region Methods
		/// <summary>
		/// The method makes it easy to add data to a table.
		/// </summary>
		/// <param name="table">Table name.</param>
		/// <param name="data">The necessary data (field and value). For the value of the parameter, use like "@@examplename".</param>
		/// <param name="parameters">Parameter yang akan ditambahkan pada perintah SQL.</param>
		public int Insert(string table, Dictionary<string, object> data, Dictionary<string, object> parameters = null)
		{
			string col = string.Empty;
			string val = string.Empty;
			string qur = string.Empty;
			foreach (KeyValuePair<string, object> d in data)
			{
				col += string.Format(" {0},", d.Key);
				if (d.Value.ToString().Contains("@@") == true)
				{
					val += string.Format(" {0},", d.Value.ToString().Replace("@@", "@"));
				}
				else
				{
					val += string.Format(" '{0}',", d.Value);
				}
			}
			col = col.Substring(0, col.Length - 1);
			val = val.Substring(0, val.Length - 1);
			qur = string.Format("INSERT INTO {0} ({1}) VALUES ({2});", table, col, val);
			if (parameters != null)
			{
				return Provider.NonQuery(qur, parameters);
			}
			else
			{
				return Provider.NonQuery(qur);
			}
		}

		/// <summary>
		/// The method makes it easy to update the data in a table.
		/// </summary>
		/// <param name="table">Table name.</param>
		/// <param name="data">The necessary data (field and value). For the value of the parameter, use like "@@examplename".</param>
		/// <param name="where">Condition (terms) of data that will be affected.</param>
		/// <param name="parameters">Parameter yang akan ditambahkan pada perintah SQL.</param>
		public int Update(string table, Dictionary<string, object> data, string where, Dictionary<string, object> parameters = null)
		{
			string vals = string.Empty;
			string qur = string.Empty;
			foreach (KeyValuePair<string, object> d in data)
			{
				if (d.Value.ToString().Contains("@@") == true)
				{
					vals += string.Format(" {0}={1},", d.Key, d.Value.ToString().Replace("@@", "@"));
				}
				else
				{
					vals += string.Format(" {0}='{1}',", d.Key, d.Value);
				}
			}
			vals = vals.Substring(0, vals.Length - 1);
			qur = string.Format("UPDATE {0} SET {1} WHERE {2};", table, vals, where);
			if (parameters == null)
			{
				return Provider.NonQuery(qur);
			}
			else
			{
				return Provider.NonQuery(qur, parameters);
			}
		}

		/// <summary>
		/// Methods that allow you to delete rows in a table.
		/// </summary>
		/// <param name="table">Table name.</param>
		/// <param name="where">Condition (terms) of data that will be affected.</param>
		/// <param name="parameters">Parameter yang akan ditambahkan pada perintah SQL.</param>
		public int Delete(string table, string where, Dictionary<string, object> parameters = null)
		{
			string qur = string.Format("DELETE FROM {0} WHERE {1}", table, where);
			if (parameters == null)
			{
				return Provider.NonQuery(qur);
			}
			else
			{
				return Provider.NonQuery(qur, parameters);
			}
		}
		#endregion

	}
}