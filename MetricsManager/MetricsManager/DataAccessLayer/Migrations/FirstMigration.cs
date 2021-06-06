using FluentMigrator;

namespace MetricsManager.DataAccessLayer.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
            Create.Table("agents")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("url").AsString(300);
            
            CreateTable("cpumetrics");
            CreateTable("dotnetmetrics");
            CreateTable("hddmetrics");
            CreateTable("networkmetrics");
            CreateTable("rammetrics");
        }

        private void CreateTable(string tableName)
        {
            Create.Table(tableName)
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64()
                .WithColumn("Agent_id").AsInt64();//todo: foreign key
        }

        public override void Down()
        {
            Delete.Table("agents");
            Delete.Table("cpumetrics");
            Delete.Table("dotnetmetrics");
            Delete.Table("hddmetrics");
            Delete.Table("networkmetrics");
            Delete.Table("rammetrics");
        }
    }
}