using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptiPlanBackend.Migrations
{
    /// <inheritdoc />
    public partial class m4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Tasks_WorkItemId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_WorkItemId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskHistories_Tasks_WorkItemId",
                table: "TaskHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskHistories_Users_ChangedById",
                table: "TaskHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Sprints_SprintId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasks_ParentId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_AssignedUserId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_ReporterId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskHistories",
                table: "TaskHistories");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "WorkItems");

            migrationBuilder.RenameTable(
                name: "TaskHistories",
                newName: "WorkItemHistories");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_SprintId",
                table: "WorkItems",
                newName: "IX_WorkItems_SprintId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ReporterId",
                table: "WorkItems",
                newName: "IX_WorkItems_ReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectId",
                table: "WorkItems",
                newName: "IX_WorkItems_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ParentId",
                table: "WorkItems",
                newName: "IX_WorkItems_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_AssignedUserId",
                table: "WorkItems",
                newName: "IX_WorkItems_AssignedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskHistories_WorkItemId",
                table: "WorkItemHistories",
                newName: "IX_WorkItemHistories_WorkItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskHistories_ChangedById",
                table: "WorkItemHistories",
                newName: "IX_WorkItemHistories_ChangedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkItems",
                table: "WorkItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkItemHistories",
                table: "WorkItemHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_WorkItems_WorkItemId",
                table: "Attachments",
                column: "WorkItemId",
                principalTable: "WorkItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_WorkItems_WorkItemId",
                table: "Comments",
                column: "WorkItemId",
                principalTable: "WorkItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItemHistories_Users_ChangedById",
                table: "WorkItemHistories",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItemHistories_WorkItems_WorkItemId",
                table: "WorkItemHistories",
                column: "WorkItemId",
                principalTable: "WorkItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Projects_ProjectId",
                table: "WorkItems",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Sprints_SprintId",
                table: "WorkItems",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Users_AssignedUserId",
                table: "WorkItems",
                column: "AssignedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Users_ReporterId",
                table: "WorkItems",
                column: "ReporterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_WorkItems_ParentId",
                table: "WorkItems",
                column: "ParentId",
                principalTable: "WorkItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_WorkItems_WorkItemId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_WorkItems_WorkItemId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItemHistories_Users_ChangedById",
                table: "WorkItemHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItemHistories_WorkItems_WorkItemId",
                table: "WorkItemHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Projects_ProjectId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Sprints_SprintId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Users_AssignedUserId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Users_ReporterId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_WorkItems_ParentId",
                table: "WorkItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkItems",
                table: "WorkItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkItemHistories",
                table: "WorkItemHistories");

            migrationBuilder.RenameTable(
                name: "WorkItems",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "WorkItemHistories",
                newName: "TaskHistories");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_SprintId",
                table: "Tasks",
                newName: "IX_Tasks_SprintId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_ReporterId",
                table: "Tasks",
                newName: "IX_Tasks_ReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_ProjectId",
                table: "Tasks",
                newName: "IX_Tasks_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_ParentId",
                table: "Tasks",
                newName: "IX_Tasks_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_AssignedUserId",
                table: "Tasks",
                newName: "IX_Tasks_AssignedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItemHistories_WorkItemId",
                table: "TaskHistories",
                newName: "IX_TaskHistories_WorkItemId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItemHistories_ChangedById",
                table: "TaskHistories",
                newName: "IX_TaskHistories_ChangedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskHistories",
                table: "TaskHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Tasks_WorkItemId",
                table: "Attachments",
                column: "WorkItemId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_WorkItemId",
                table: "Comments",
                column: "WorkItemId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHistories_Tasks_WorkItemId",
                table: "TaskHistories",
                column: "WorkItemId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHistories_Users_ChangedById",
                table: "TaskHistories",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Sprints_SprintId",
                table: "Tasks",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasks_ParentId",
                table: "Tasks",
                column: "ParentId",
                principalTable: "Tasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_AssignedUserId",
                table: "Tasks",
                column: "AssignedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_ReporterId",
                table: "Tasks",
                column: "ReporterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
