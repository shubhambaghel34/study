// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2017
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
namespace Coyote.Execution.CheckCall.Storage
{
    using Autofac;
    using System;
    using System.Data;
    using System.Linq;
    public sealed class SqlModule : Module
    {
        #region Private properties 
        private readonly string _connectionString;
        #endregion

        #region Constructor
        public SqlModule(string connectionString)
        {
            _connectionString = connectionString.ThrowIfArgumentNullOrEmpty(nameof(connectionString));
        }

        #endregion

        #region Methods
        protected override void Load(ContainerBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains(".Storage"))
                .AsImplementedInterfaces()
                .WithParameters(new NamedParameter[]
                {
                    new NamedParameter("connectionString", _connectionString)
                });
        }
        #endregion
    }
}
