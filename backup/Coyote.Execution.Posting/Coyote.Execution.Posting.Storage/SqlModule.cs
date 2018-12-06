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
    using Autofac;
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts;
    using System;
    using System.Data;
    using System.Linq;

    public sealed class SqlModule : Module
    {
        private string _connectionString { get; set; }

        public SqlModule(string connectionString)
        {
            _connectionString = connectionString.ThrowIfArgumentNullOrEmpty(nameof(connectionString));
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.RegisterType<Database>()
                .SingleInstance()
                .WithParameter("connectionString",_connectionString)
                .AsSelf();

            builder.RegisterType<RuntimeSettings>()
                .As<IRuntimeSettings>()
                .SingleInstance()
                .AsSelf();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains(".Storage"))
                .AsImplementedInterfaces()
                .SingleInstance()
                .WithParameters(new NamedParameter[]
                {
                    new NamedParameter("connectionString", _connectionString)
                });
        }
    }
}