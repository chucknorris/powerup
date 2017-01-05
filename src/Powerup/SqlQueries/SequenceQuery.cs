namespace Powerup.SqlQueries
{
    using Powerup.SqlObjects;
    using Powerup.Templates;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class SequenceQuery : QueryBase
    {
        public override string Folder
        {
            get
            {
                return "RunBeforeUp";
            }
        }

        public string scriptValidation => @"IF NOT EXISTS(SELECT 1 FROM sys.sequences WHERE name = '{0}')
BEGIN
	{1}
END";

        public override void AddCode(SqlConnection connection, SqlObject obj)
        {
            
            SysSequence sequence = null;
            using (var cmd = new SqlCommand(@"SELECT  seq.name ,
		t.name ,
        is_cached ,
		is_cycling ,
		increment ,
		maximum_value ,
		minimum_value ,
		start_value  
		FROM sys.sequences AS seq
		JOIN Sys.types t ON seq.user_type_id = t.user_type_id
WHERE seq.name = @NAME AND object_id = @ID", connection))
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
                        if(sequence == null)
                        {
                            sequence = new SysSequence
                            {
                                Name = $"{reader[0]}",
                                UserTypeId = $"{reader[1]}",
                                Cache = (bool)reader[2],
                                Cycle = (bool)reader[3],
                                Increment = Convert.ToInt32(reader[4]),
                                MaxValue = reader[5].ToString(),
                                MinValue = reader[6].ToString(),
                                StartWith = Convert.ToInt32(reader[7])
                            };
                        }
                    }
                    string codeSequence = $@"CREATE SEQUENCE [dbo].[{sequence.Name}] 
        AS {sequence.UserTypeId} 
        START WITH {sequence.StartWith}
        INCREMENT BY {sequence.Increment}
        MINVALUE {sequence.MinValue}
        MAXVALUE {sequence.MaxValue}
        {sequence.StrCache}
        {sequence.StrCycle}";
                    obj.Code = string.Format(scriptValidation, obj.Name, codeSequence);
                    obj.AddCodeTemplate();
                }
            }
        }

        public override string NameSql
        {
            get
            {
                return @"SELECT
seq.name AS [Name],
s.name [Schema],
seq.object_id AS [Object ID]
FROM
sys.sequences AS seq
JOIN sys.schemas s ON seq.schema_id = s.schema_id";
            }
        }

        public override SqlType SqlType
        {
            get
            {
                return SqlType.Sequence;
            }
        }

        public override ITemplate TemplateToUse(SqlObject sqlObject)
        {
            return new VoidTemplate(sqlObject);
        }
    }
}