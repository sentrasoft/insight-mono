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
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Sentrasoft.Data
{
	/// <summary>
	/// Represents a connection on database providers that are designed for Microsoft SQL Server.
	/// </summary>
	public sealed class SqlServer : DbSql
	{

		#region Construct
		/// <summary>
		/// Initializes a new instance of the <see cref="Sentrasoft.Data.SqlServer"/> class.
		/// </summary>
		public SqlServer ()
		{
			Crud = new DbSqlCrud (this);
		}
		#endregion

		#region Parameters
		/// <summary>
		/// Gets or sets the data source.
		/// </summary>
		/// <value>The host name, provider or server name.</value>
		public override string DataSource {
			get {
				return base.DataSource;
			}
			set {
				base.DataSource = value;
			}
		}

		/// <summary>
		/// Gets or sets the server port.
		/// </summary>
		/// <value>Server database port (service).</value>
		public override int Port {
			get {
				return base.Port;
			}
			set {
				base.Port = value;
			}
		}

		/// <summary>
		/// Gets or sets the database user.
		/// </summary>
		/// <value>The database user.</value>
		public override string User {
			get {
				return base.User;
			}
			set {
				base.User = value;
			}
		}

		/// <summary>
		/// Gets or sets the database password.
		/// </summary>
		/// <value>The database password.</value>
		public override string Password {
			get {
				return base.Password;
			}
			set {
				base.Password = value;
			}
		}

		/// <summary>
		/// Gets or sets the database/schema name.
		/// </summary>
		/// <value>The database/schema name.</value>
		public override string Database {
			get {
				return base.Database;
			}
			set {
				base.Database = value;
			}
		}

		/// <summary>
		/// Gets or sets the identifier or like application name as accessor.
		/// </summary>
		/// <value>Connection identity (connection description) is displayed on the session host/server. Apply if the host / server
		/// supports the client's identity/accessor.</value>
		public override string Identifier {
			get {
				return base.Identifier;
			}
			set {
				base.Identifier = value;
			}
		}

		/// <summary>
		/// Gets or sets the default connection timeout.
		/// </summary>
		/// <value>The default connection timeout.</value>
		public override int DefaultConnectionTimeout {
			get {
				return base.DefaultConnectionTimeout;
			}
			set {
				base.DefaultConnectionTimeout = value;
			}
		}

		/// <summary>
		/// Gets or sets the default command timeout.
		/// </summary>
		/// <value>The default command timeout.</value>
		public override int DefaultCommandTimeout {
			get {
				return base.DefaultCommandTimeout;
			}
			set {
				base.DefaultCommandTimeout = value;
			}
		}

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value>The connection string in accordance with the arrangements provided.</value>
		public override string ConnectionString {
			get
			{
				if (Password == string.Empty)
				{
					return string.Format("Server={0}; Database={1}; User Id={2}; Application Name={3}; Connection Timeout={4};", base.DataSource, base.Database, base.User, base.Identifier, base.DefaultConnectionTimeout);
				}
				else
				{
					return string.Format("Server={0}; Database={1}; User Id={2}; Password={3}; Application Name={4}; Connection Timeout={5};", base.DataSource, base.Database, base.User, base.Password, base.Identifier, base.DefaultConnectionTimeout);
				}
			}
			set
			{
				base.DefaultConnection.ConnectionString = value;
				base.RenderConnection.ConnectionString = value;
			}
		}
		#endregion

		#region Helpers
		/// <summary>
		/// Gets or sets the SQL-Crud Helper.
		/// </summary>
		/// <value>Used to add, delete, and update data using single line functions.</value>
		public override object Crud {
			get {
				return base.Crud;
			}
			set {
				base.Crud = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Open active connection.
		/// </summary>
		public override void Open ()
		{
			if (base.DefaultConnection != null)
				base.DefaultConnection.Close();

			base.DefaultConnection = new SqlConnection(ConnectionString);
			base.DefaultConnection.Open();
		}

		/// <summary>
		/// Close active connection.
		/// </summary>
		public override void Close ()
		{
			if (base.DefaultConnection != null)
				base.DefaultConnection.Close();

			base.DefaultConnection.Close();
		}

		/// <summary>
		/// Close the specified connection.
		/// </summary>
		/// <param name="all">If set to <c>true</c> all.</param>
		public override void Close (bool all)
		{
			if (base.DefaultConnection != null)
				base.DefaultConnection.Close();

			if (all == true)
			if (base.RenderConnection != null) { base.RenderConnection.Close(); }

			base.DefaultConnection.Close();
		}

		/// <summary>
		/// Execute the sql using NonQuery method.
		/// </summary>
		/// <returns>Showing the amount of data affected.</returns>
		/// <param name="sql">Sql.</param>
		/// <param name="parameters">The parameters to be added to the SQL command.</param>
		/// <param name="timeout">If the estimation process more than this time, the query will not be processed.</param>
		public override int NonQuery (string sql, Dictionary<string, object> parameters, int timeout = 0)
		{
			Open();

			SqlCommand cmd = new SqlCommand(sql, (SqlConnection)base.DefaultConnection);
			cmd.CommandTimeout = base.DefaultCommandTimeout;
			if (timeout > 0)
				cmd.CommandTimeout = timeout;

			if (parameters != null)
			{
				foreach (KeyValuePair<string, object> p in parameters)
				{ cmd.Parameters.AddWithValue(p.Key, p.Value); }
			}

			int affected = cmd.ExecuteNonQuery();

			Close();
			return affected;
		}

		/// <summary>
		/// Execute the sql queries using Scalar method.
		/// </summary>
		/// <param name="sql">Sql.</param>
		/// <param name="parameters">The parameters to be added to the SQL command.</param>
		/// <param name="timeout">If the estimation process more than this time, the query will not be processed.</param>
		public override object Scalar (string sql, Dictionary<string, object> parameters, int timeout = 0)
		{
			Open();

			SqlCommand cmd = new SqlCommand(sql, (SqlConnection)base.DefaultConnection);
			cmd.CommandTimeout = base.DefaultCommandTimeout;
			if (timeout > 0)
				cmd.CommandTimeout = timeout;

			if (parameters != null)
			{
				foreach (KeyValuePair<string, object> p in parameters)
				{ cmd.Parameters.AddWithValue(p.Key, p.Value); }
			}
			object retval = cmd.ExecuteScalar();
			Close();
			return retval;
		}

		/// <summary>
		/// Fill the specified sql.
		/// </summary>
		/// <param name="sql">Sql.</param>
		public override DataTable Fill (string sql)
		{
			Open();

			DataTable _d = new DataTable();
			SqlDataAdapter adapter = new SqlDataAdapter(sql, (SqlConnection)base.DefaultConnection);
			adapter.Fill(_d);

			Close();
			return _d;
		}

		/// <summary>
		/// Rendering to obtain data based on SQL commands.
		/// </summary>
		/// <param name="sql">Sql.</param>
		/// <param name="parameters">The parameters to be added to the SQL command.</param>
		/// <param name="timeout">If the estimation process more than this time, the query will not be processed.</param>
		/// <param name="multirender">Options for creating a multi-render data. (Render data in the rendering process data).</param>
		public override DbDataReader Render (string sql, Dictionary<string, object> parameters, int timeout = 0, bool multirender = false)
		{
			DbConnection activeConnection;
			DbConnection lockedConnection;
			if (multirender)
			{
				lockedConnection = new SqlConnection(ConnectionString);
				lockedConnection.Open();
				activeConnection = lockedConnection;
			}
			else
			{
				if (base.RenderConnection != null) { base.RenderConnection.Close(); }
				base.RenderConnection = new SqlConnection(ConnectionString);
				base.RenderConnection.Open();
				activeConnection = base.RenderConnection;
			}

			SqlDataReader result;
			SqlCommand cmd = new SqlCommand(sql, (SqlConnection)activeConnection);

			cmd.CommandTimeout = base.DefaultCommandTimeout;
			if (timeout > 0)
				cmd.CommandTimeout = timeout;

			if (parameters != null)
			{
				foreach (KeyValuePair<string, object> par in parameters)
				{
					cmd.Parameters.AddWithValue(par.Key, par.Value);
				}
			}
			result = cmd.ExecuteReader();
			return result;
		}

		/// <summary>
		/// Creating a new SQL command with certain parameters.
		/// </summary>
		/// <returns>The sql command.</returns>
		/// <param name="sql">Sql.</param>
		/// <param name="parameters">The parameters to be added to the SQL command.</param>
		/// <param name="timeout">If the estimation process more than this time, the query will not be processed.</param>
		public override DbCommand CreateCommand (string sql, Dictionary<string, object> parameters, int timeout = 0)
		{
			SqlCommand cmd = new SqlCommand(sql, (SqlConnection)base.DefaultConnection);

			cmd.CommandTimeout = base.DefaultCommandTimeout;
			if (timeout > 0)
				cmd.CommandTimeout = timeout;

			if (parameters != null)
			{
				foreach (KeyValuePair<string, object> par in parameters)
				{
					cmd.Parameters.AddWithValue(par.Key, par.Value);
				}
			}
			return cmd;
		}

		/// <summary>
		/// Prebuild or make temporary to manage data on the data control.
		/// </summary>
		/// <param name="sql">Sql.</param>
		/// <param name="editor">Data Control.</param>
		public override void Prebuild (string sql, object editor)
		{
			Open();
			dynamic _editor = editor;
			DataTable _data = new DataTable();
			SqlCommandBuilder _build = new SqlCommandBuilder(new SqlDataAdapter(sql, (SqlConnection)base.DefaultConnection));
			_build.DataAdapter.Fill(_data);

			_editor.Tag = _build.DataAdapter;
			_editor.DataSource = _data;
			Close();
		}

		/// <summary>
		/// Posting the data is already made changes to (build).
		/// </summary>
		/// <param name="adapter">Use the property "Tag" on the control editor.</param>
		/// <param name="data">Data or object as datasource.</param>
		public override void Postbuild (object adapter, DataTable data)
		{
			dynamic Adapter = adapter;
			Adapter.Update(data);
			data.AcceptChanges();
		}
		#endregion


	}
}