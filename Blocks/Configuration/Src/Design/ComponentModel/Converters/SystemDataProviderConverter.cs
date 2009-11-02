using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Console.Wpf.ComponentModel.Converters
{
    public class SystemDataProviderConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new[]{
                "System.Data.SqlClient",
                "Oracle.DataAccess.Client",
                "System.Data.Odbc",
                "System.Data.OleDb",
                "System.Data.OracleClient",
                "System.Data.SqlServerCe",
                "System.Data.SqlServerCe.3.5" });
        }
    }
}
