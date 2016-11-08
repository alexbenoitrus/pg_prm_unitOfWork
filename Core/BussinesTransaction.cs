﻿// -----------------------------------------------------------------------
// <copyright file="BussinesTransaction.cs" company="Paragon Software Group">
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

namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Core.Interfaces;
    using Core.Journals;
    using Newtonsoft.Json;
    using System.IO;
    public sealed class BussinesTransaction : IDisposable
    {
        internal IJournal Journal { get; private set; }
        public List<ITransactionUnit> Operations { get; private set; }
        public List<ITransactionUnit> CommitedOperations { get; private set; }

        internal BussinesTransaction(IJournal journal)               
        {
            Journal = journal;
            Operations = new List<ITransactionUnit>();
            CommitedOperations = new List<ITransactionUnit>();
        }

        public void RegisterOperation(ITransactionUnit operation)
        {
            Operations.Add(operation);
        }

        public void Commit()
        {
            try
            {
                CommitEachOperation();
            }
            catch (SerializationException e)
            {
                Rollback();
                throw e;
            }
            catch (Exception e)
            {
                Rollback();
            }
        }

        public void Rollback()
        {
            var json = File.ReadAllText(@"C:\TestWoU\test.txt");
            Operations = Journal.GetOperationsFromJournal();

            foreach (var operation in Operations)
            {
                operation.Rollback();
                Journal.Remove(operation);
            }
        }

        private void CommitEachOperation()
        {
            foreach (var operation in Operations)
            {
                Journal.Write(operation);
                try
                {
                    operation.Commit();
                    CommitedOperations.Add(operation);
                    string json = JsonConvert.SerializeObject(CommitedOperations, Formatting.Indented);
                    File.Create(@"C:\TestWoU\test.txt").Close();
                    File.WriteAllText(@"C:\TestWoU\test.txt", json);
                }
                catch (Exception e)
                {
                    string json = JsonConvert.SerializeObject(CommitedOperations, Formatting.Indented);
                    File.Create(@"C:\TestWoU\test.txt").Close();
                    File.WriteAllText(@"C:\TestWoU\test.txt", json);
                    CommitedOperations.Remove(operation);
                    throw e;
                }
            }
        }

        public void Dispose()
        {
            Operations.Clear();
            Operations = null;
            Journal.Dispose();
        }
    }
}