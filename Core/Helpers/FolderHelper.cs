﻿// -----------------------------------------------------------------------
// <copyright file="FolderHelper.cs" company="Paragon Software Group">
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

namespace Core.Helpers
{
    using System.IO;

    internal static class FolderHelper
    {
        private static string defaultFolder = Path.Combine(
            Path.GetTempPath(),
            "UnitOfWork");
        
        internal static string JournalsFolder
        {
            get
            {
                bool useDefaultFolder = UserConfig.Default.UseDefaultJournalFolder;
                string userFolder = UserConfig.Default.CustomJournalFolderPath;
                return useDefaultFolder
                    ? defaultFolder
                    : userFolder;
            }
        }

        internal static void UseUserFolder(string path)
        {
            UserConfig.Default.UseDefaultJournalFolder = false;
            UserConfig.Default.CustomJournalFolderPath = path;
            CreateJournalsFolder();
        }

        internal static void UseDefaultFolder()
        {
            UserConfig.Default.UseDefaultJournalFolder = true;
            CreateJournalsFolder();
        }

        internal static void CreateJournalsFolder()
        {
            if (!Directory.Exists(JournalsFolder))
            {
                Directory.CreateDirectory(JournalsFolder);
            }
        }
    }
}
