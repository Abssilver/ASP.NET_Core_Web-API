using FluentMigrator;

namespace MetricsAgent.DataAccessLayer.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
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
                .WithColumn("Time").AsInt64();
        }

        public override void Down()
        {
            Delete.Table("cpumetrics");
            Delete.Table("dotnetmetrics");
            Delete.Table("hddmetrics");
            Delete.Table("networkmetrics");
            Delete.Table("rammetrics");
        }
    }
}