namespace Powerup.SqlQueries
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    using Powerup.SqlObjects;
    using Powerup.Templates;

    public class IndexQuery : QueryBase
    {
        public override string Folder
        {
            get
            {
                return "indexes";
            }
        }

        public override void AddCode(SqlConnection connection, SqlObject obj)
        {
            SysIndex index = null;
            using (var cmd = new SqlCommand(@"SELECT ind.index_id AS IndexId
,ind.name AS IndexName
,col.name AS ColumnName
,t.name AS TableName
,s.name AS SchemaName
,ic.index_column_id AS ColumnId
,ind.is_unique AS IsUnique
,ind.type_desc as Type
,ind.has_filter AS HasFilter
,ind.filter_definition AS FilterDefinition
,ic.is_descending_key AS IsDescending
,ic.is_included_column AS IsIncluded
FROM sys.indexes ind 
JOIN sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 
JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id 
JOIN sys.tables t ON ind.object_id = t.object_id 
JOIN sys.schemas s ON t.schema_id = s.schema_id
WHERE ind.name = @NAME
  AND ind.object_id = @ID
ORDER BY TableName
,IndexId
,ColumnId", connection))
            {
                cmd.Parameters.Add("@NAME", SqlDbType.NVarChar, 128);
                cmd.Parameters.Add("@ID", SqlDbType.Int);
                cmd.Parameters["@NAME"].Value = obj.Name;
                cmd.Parameters["@ID"].Value = obj.ObjectId;
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows) return;
                    while (reader.Read())
                    {
                        if (index == null)
                        {
                            var tbl = new SysTable
                            {
                                Name = reader[3].ToString(),
                                Schema = reader[4].ToString()
                            };

                            index = new SysIndex
                            {
                                Id = Convert.ToInt32(reader[0]),
                                Name = reader[1].ToString(),
                                Table = tbl,
                                Type = reader[7].ToString(),
                                IsUnique = Convert.ToBoolean(reader[6]),
                                HasFilter = Convert.ToBoolean(reader[8]),
                                FilterDefinition = Convert.ToString(reader[9])
                            };
                        }

                        index.Columns.Add(new SysIndexColumn
                        {
                            Id = Convert.ToInt32(reader[5]),
                            Name = Convert.ToString(reader[2]),
                            IsDescending = Convert.ToBoolean(reader[10]),
                            IsIncluded = Convert.ToBoolean(reader[11]),
                        });
                    }
                    var buffer = new StringBuilder();
                    buffer.AppendLine(@"DECLARE @Name nvarchar(128), @TableName nvarchar(128), @TableSchema nvarchar(128)");
                    buffer.AppendFormat(
                        @"SELECT @Name = N'{0}', @TableName=N'{1}', @TableSchema = N'{2}'",
                        index.Name,
                        index.Table.Name,
                        index.Table.Schema);
                    buffer.AppendLine();
                    buffer.AppendLine(
                        "IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('[' + @TableSchema + '].[' + @TableName + ']') AND UPPER(name) = UPPER(@Name))");
                    buffer.AppendLine(
                        "    EXECUTE('DROP INDEX [' + @Name + '] ON [' + @TableSchema + '].[' + @TableName + ']')");
                    buffer.AppendLine();
                    buffer.AppendLine(@"PRINT 'Creating index [' + @Name + '] on table [' + @TableSchema + '].[' + @TableName + ']'");
                    buffer.AppendLine("GO");
                    buffer.AppendLine();
                    buffer.Append(@"CREATE");
                    if (index.IsUnique)
                    {
                        buffer.Append(" UNIQUE");
                    }

                    buffer.AppendFormat(" {0} INDEX [{1}]", index.Type, index.Name);
                    buffer.AppendLine();
                    buffer.AppendFormat("ON [{0}].[{1}]", index.Table.Schema, index.Table.Name);
                    buffer.AppendFormat("({0})", string.Join(", ", index.Columns.Where(c => !c.IsIncluded).Select(
                        c =>
                        {
                            if (c.IsDescending)
                            {
                                return c.Name + " DESC";
                            }

                            return c.Name;
                        })));
                    if (index.Columns.Any(c => c.IsIncluded))
                    {
                        buffer.AppendLine();
                        buffer.AppendFormat("INCLUDE ({0})", string.Join(", ", index.Columns.Where(c => c.IsIncluded).Select(c => c.Name)));
                    }

                    if (index.HasFilter)
                    {
                        buffer.AppendLine();
                        buffer.AppendFormat("WHERE {0}", index.FilterDefinition);
                    }

                    obj.Code += buffer.ToString();
                    obj.AddCodeTemplate();
                }
            }
        }

        public override string NameSql
        {
            get
            {
                return @"SELECT DISTINCT ind.name AS IndexName
,s.name AS SchemaName
,ind.object_id AS ObjectId
FROM sys.indexes ind
JOIN sys.tables t ON ind.object_id = t.object_id 
JOIN sys.schemas s ON t.schema_id = s.schema_id
WHERE ind.index_id <> 0
  AND ind.is_primary_key = 0 
  AND ind.is_unique_constraint = 0 
  AND t.is_ms_shipped = 0";
            }
        }

        public override SqlType SqlType
        {
            get
            {
                return SqlType.Index;
            }
        }

        public override ITemplate TemplateToUse(SqlObject sqlObject)
        {
            return new VoidTemplate(sqlObject);
        }
    }
}