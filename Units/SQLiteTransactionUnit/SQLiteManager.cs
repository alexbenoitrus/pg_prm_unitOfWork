﻿// -----------------------------------------------------------------------
// <copyright file="SQLiteManager.cs" company="Paragon Software Group">
// EXCEPT WHERE OTHERWISE STATED, THE INFORMATION AND SOURCE CODE CONTAINED 
// HEREIN AND IN RELATED FILES IS THE EXCLUSIVE PROPERTY OF PARAGON SOFTWARE
// GROUP COMPANY AND MAY NOT BE EXAMINED, DISTRIBUTED, DISCLOSED, OR REPRODUCED
// IN WHOLE OR IN PART WITHOUT EXPLICIT WRITTEN AUTHORIZATION FROM THE COMPANY.
// 
// Copyright (c) 1994-2016 Paragon Software Group, All rights reserved.
// 
// UNLESS OTHERWISE AGREED IN A WRITING SIGNED BY THE PARTIES, THIS SOFTWARE IS
// PROVIDED "AS-IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE, ALL OF WHICH ARE HEREBY DISCLAIMED. IN NO EVENT SHALL THE
// AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF NOT ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace Units.SQLiteTransactionUnit
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using Resources = Properties.SQLiteUnit;
    
    public static class SQLiteManager
    {
        public static string GetConnectionString(string dataBasePath)
        {
            return string.Format(
                    Resources.ConnectionString,
                    dataBasePath);
        }

        public static void ExecuteCommands(string dataBasePath, IEnumerable<string> commands)
        {
            var сonnectionString = GetConnectionString(dataBasePath);
            var connection = new SQLiteConnection(сonnectionString);
            connection.Open();
            var transaction = connection.BeginTransaction();
            var sqlCommand = new SQLiteCommand(connection)
            {
                Transaction = transaction
            };
            try
            {
                foreach (string command in commands)
                {
                    sqlCommand.CommandText = command;
                    sqlCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                transaction.Dispose();
                sqlCommand.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
    }
}