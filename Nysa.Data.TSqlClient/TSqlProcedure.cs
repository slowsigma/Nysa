using System;
using System.Collections.Generic;

namespace Nysa.Data.TSqlClient
{

    public class TSqlProcedure
    {
        public String Name { get; init; }
        public IReadOnlyList<TSqlParameter> Paremeters { get; init; }

        public TSqlProcedure(String name)
        {
            this.Name       = name;
            this.Paremeters = new TSqlParameter[] {};
        }

        public TSqlProcedure(String name, params TSqlParameter[] parameters)
        {
            this.Name       = name;
            this.Paremeters = parameters;
        }

        public TSqlProcedure WithParameters(params TSqlParameter[] parameters)
            => new TSqlProcedure(this.Name, parameters);
    }

}