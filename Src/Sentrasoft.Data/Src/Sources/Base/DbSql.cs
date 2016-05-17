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
	/// Abstract class that is used to run the SQL function. This class is the basis of a wide range of databases.
	/// </summary>
	public abstract class DbSql
	{

		#region Connections
		/// <summary>
		/// Gets or sets the default connection.
		/// </summary>
		/// <value>The default connection is used for global functions.</value>
		public virtual DbConnection DefaultConnection {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the render connection.
		/// </summary>
		/// <value>Connection used to read data separately from the default connection.</value>
		public virtual DbConnection RenderConnection {
			get;
			set;
		}
		#endregion


		#region Parameters
		/// <summary>
		/// Gets or sets the data source.
		/// </summary>
		/// <value>The host name, provider or server name.</value>
		public virtual string DataSource {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the server port.
		/// </summary>
		/// <value>Server database port (service).</value>
		public virtual int Port {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the database user.
		/// </summary>
		/// <value>The database user.</value>
		public virtual string User {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the database password.
		/// </summary>
		/// <value>The database password.</value>
		public virtual string Password {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the database/schema name.
		/// </summary>
		/// <value>The database/schema name.</value>
		public virtual string Database {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the identifier or like application name as accessor.
		/// </summary>
		/// <value>Connection identity (connection description) is displayed on the session host/server. Apply if the host / server supports the client's identity/accessor.</value>
		public virtual string Identifier {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the default connection timeout.
		/// </summary>
		/// <value>The default connection timeout.</value>
		public virtual int DefaultConnectionTimeout {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the default command timeout.
		/// </summary>
		/// <value>The default command timeout.</value>
		public virtual int DefaultCommandTimeout {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value>The connection string in accordance with the arrangements provided.</value>
		public abstract string ConnectionString {
			get;
			set;
		}
		#endregion

		#region Helpers
		/// <summary>
		/// Gets or sets the SQL-Crud Helper.
		/// </summary>
		/// <value>Used to add, delete, and update data using single line functions.</value>
		public virtual object Crud {
			get;
			set;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Open active connection.
		/// </summary>
		public abstract void Open();

		/// <summary>
		/// Close active connection.
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// Close the specified connection.
		/// </summary>
		/// <param name="all">If set to <c>true</c> all connection will be closed.</param>
		public abstract void Close(bool all);

		/// <summary>
		/// Execute the sql using NonQuery method.
		/// </summary>
		/// <returns>Showing the amount of data affected.</returns>
		/// <param name="sql">Sql.</param>
		/// <param name="parameters">The parameters to be added to the SQL command.</param>
		/// <param name="timeout">If the estimation process more than this time, the query will not be processed.</param>
		public abstract int NonQuery(string sql, Dictionary<string, object> parameters = null, int timeout = 0);

		/// <summary>
		/// Execute the sql queries using Scalar method.
		/// </summary>
		/// <param name="sql">Sql.</param>
		/// <param name="parameters">The parameters to be added to the SQL command.</param>
		/// <param name="timeout">If the estimation process more than this time, the query will not be processed.</param>
		public abstract object Scalar(string sql, Dictionary<string, object> parameters = null, int timeout = 0);

		/// <summary>
		/// Obtain and make data based on SQL commands into <see cref="System.Data.DataTable"/>.
		/// </summary>
		/// <param name="sql">Sql.</param>
		public abstract DataTable Fill(string sql);

		/// <summary>
		/// Rendering to obtain data based on SQL commands.
		/// </summary>
		/// <param name="sql">Sql.</param>
		/// <param name="parameters">The parameters to be added to the SQL command.</param>
		/// <param name="timeout">If the estimation process more than this time, the query will not be processed.</param>
		/// <param name="multirender">Options for creating a multi-render data. (Render data in the rendering process data).</param>
		public abstract DbDataReader Render(string sql, Dictionary<string, object> parameters = null, int timeout = 0, bool multirender = false);

		/// <summary>
		/// Creating a new SQL command with certain parameters.
		/// </summary>
		/// <returns>The sql command.</returns>
		/// <param name="sql">Sql.</param>
		/// <param name="parameters">The parameters to be added to the SQL command.</param>
		/// <param name="timeout">If the estimation process more than this time, the query will not be processed.</param>
		public abstract DbCommand CreateCommand(string sql, Dictionary<string, object> parameters = null, int timeout = 0);

		/// <summary>
		/// Prebuild or make temporary to manage data on the data control.
		/// </summary>
		/// <param name="sql">Sql.</param>
		/// <param name="editor">Data Control.</param>
		public abstract void Prebuild(string sql, object editor);

		/// <summary>
		/// Posting the data is already made changes to (build).
		/// </summary>
		/// <param name="adapter">Use the property "Tag" on the control editor.</param>
		/// <param name="data">Data or object as datasource.</param>
		public abstract void Postbuild(object adapter, DataTable data);
		#endregion

	}
}