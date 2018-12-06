// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
namespace Coyote.Execution.Posting.Storage
{
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts;
    using Dapper;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class RuntimeSettings : IRuntimeSettings
    {
        #region " Private properties "
        private readonly ILog _log;
        private readonly string _connectionString;
        private Dictionary<string, string> _dicSettings = new Dictionary<string, string>();
        private int _intServiceUserId = 0;
        #endregion

        #region " Public properties "

        public int ServiceUserId
        {
            get { return _intServiceUserId; }
        }

        public string DATLoadPostingWebUrl
        {
            get { return _dicSettings[nameof(DATLoadPostingWebUrl)]; }
        }

        public bool IsPopulated { get; private set; }

        public string RealtimeUpdateServiceAddress
        {
            get { return _dicSettings[nameof(RealtimeUpdateServiceAddress)]; }
        }

        public string UpdateMaxPayWebUrl
        {
            get { return _dicSettings[nameof(UpdateMaxPayWebUrl)]; }
        }

        #endregion

        #region " Constructor "
        public RuntimeSettings(ILog log, string connectionString)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _connectionString = connectionString.ThrowIfArgumentNullOrEmpty(nameof(connectionString));
            Populate();
        }
        #endregion

        #region " Private Methods "
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        private void Populate()
        {
            try
            {
                if (!IsPopulated)
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        string query = @"SELECT  SettingName,
				                             SettingValue,
				                             VBDataType,
			                                 Category
		                             FROM	 dbo.SystemSettings (NOLOCK)
		                             WHERE	Category = 'Bazooka';";

                        var reader = connection.Query(query).ToList();
                        if (reader != null && reader.Count > 0)
                        {
                            foreach (dynamic obj in reader)
                            {
                                _dicSettings.Add(obj.SettingName, obj.SettingValue);
                            }
                        }

                        query = @"SELECT  UserId
                            FROM    [dbo].[SystemUser] (NOLOCK)
                            WHERE   Code = 'Coyote.Execution.Posting';";

                        reader = connection.Query(query).ToList();

                        if (reader != null && reader.Count == 1) _intServiceUserId = reader.First().UserId;

                        IsPopulated = true;
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error($"Unable to populate System Settings: {e}");
                IsPopulated = false;
                throw new Exception("Unable to populate System Settings.", e);
            }
        }
        #endregion
    }
}