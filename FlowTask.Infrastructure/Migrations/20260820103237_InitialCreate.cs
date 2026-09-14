using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowTask.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "system_roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    avatar_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    timezone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "system_role_claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_role_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_system_role_claims_system_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "system_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "access_tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_access_tokens_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "apps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeveloperId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DocumentationUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_apps_users_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_notifications_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "permission_grants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResourceId = table.Column<int>(type: "int", nullable: true),
                    Permission = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission_grants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_permission_grants_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "system_user_roles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_user_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_system_user_roles_system_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "system_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_system_user_roles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_claims_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_logins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_logins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_user_logins_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_preferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NotificationsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_preferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_preferences_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SessionToken = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastActivity = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_sessions_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_tokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_tokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_user_tokens_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "workspaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workspaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workspaces_users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "app_permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<int>(type: "int", nullable: false),
                    PermissionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Required = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_app_permissions_apps_AppId",
                        column: x => x.AppId,
                        principalTable: "apps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "activity_logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_activity_logs_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_activity_logs_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "activity_snapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    SnapshotDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivitySummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metrics = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_snapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_activity_snapshots_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ai_agents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_agents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_agents_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ai_agents_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ai_knowledge_base",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RelevanceScore = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    LastUpdatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_knowledge_base", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_knowledge_base_users_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ai_knowledge_base_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ai_model_configs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModelProvider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApiKeyHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Temperature = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    MaxTokens = table.Column<int>(type: "int", nullable: false),
                    TopP = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_model_configs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_model_configs_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ai_model_usage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UsageDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestCount = table.Column<int>(type: "int", nullable: false),
                    TokensInput = table.Column<int>(type: "int", nullable: false),
                    TokensOutput = table.Column<int>(type: "int", nullable: false),
                    TotalTokens = table.Column<int>(type: "int", nullable: false),
                    CostUsd = table.Column<decimal>(type: "decimal(12,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_model_usage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_model_usage_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ai_prompt_responses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PromptInput = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelUsed = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserFeedback = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FeedbackComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContextType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContextId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_prompt_responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_prompt_responses_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ai_prompt_responses_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ai_skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromptTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UseCases = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_skills_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ai_skills_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "app_installations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<int>(type: "int", nullable: false),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    InstalledBy = table.Column<int>(type: "int", nullable: false),
                    Configuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InstalledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_installations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_app_installations_apps_AppId",
                        column: x => x.AppId,
                        principalTable: "apps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_app_installations_users_InstalledBy",
                        column: x => x.InstalledBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_app_installations_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "audit_events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResourceId = table.Column<int>(type: "int", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_audit_events_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_audit_events_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "custom_fields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Configuration = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_fields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_custom_fields_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dashboards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsShared = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboards_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboards_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_documents_users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_documents_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "goals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_goals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_goals_users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_goals_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "integrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Credentials = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_integrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_integrations_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "invitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    InvitedBy = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_invitations_users_InvitedBy",
                        column: x => x.InvitedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_invitations_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "priority_levels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_priority_levels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_priority_levels_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Filters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reports_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reports_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "scheduled_jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorkspaceId = table.Column<int>(type: "int", nullable: true),
                    Schedule = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastRun = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextRun = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Config = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scheduled_jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_scheduled_jobs_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "space_templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Configuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_space_templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_space_templates_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "spaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_spaces_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tags_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Handle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_teams_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TemplateType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_templates_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_templates_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vector_embeddings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SourceId = table.Column<int>(type: "int", nullable: true),
                    SourceTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TextChunk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmbeddingVector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmbeddingModel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vector_embeddings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vector_embeddings_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "webhooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Events = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Secret = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_webhooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_webhooks_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "workspace_members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workspace_members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workspace_members_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_workspace_members_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "workspace_roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workspace_roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workspace_roles_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "agent_actions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AiAgentId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true),
                    RetryPolicy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FallbackActionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_actions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_agent_actions_agent_actions_FallbackActionId",
                        column: x => x.FallbackActionId,
                        principalTable: "agent_actions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_agent_actions_ai_agents_AiAgentId",
                        column: x => x.AiAgentId,
                        principalTable: "ai_agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "agent_prompts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AiAgentId = table.Column<int>(type: "int", nullable: false),
                    PromptType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PromptText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_prompts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_agent_prompts_ai_agents_AiAgentId",
                        column: x => x.AiAgentId,
                        principalTable: "ai_agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ai_agent_executions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AiAgentId = table.Column<int>(type: "int", nullable: false),
                    TriggeredBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TriggeredByUserId = table.Column<int>(type: "int", nullable: true),
                    ModelUsed = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InputPrompt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionsExecuted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationMs = table.Column<int>(type: "int", nullable: true),
                    TokensInput = table.Column<int>(type: "int", nullable: false),
                    TokensOutput = table.Column<int>(type: "int", nullable: false),
                    TokensTotal = table.Column<int>(type: "int", nullable: false),
                    CostUsd = table.Column<decimal>(type: "decimal(10,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_agent_executions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_agent_executions_ai_agents_AiAgentId",
                        column: x => x.AiAgentId,
                        principalTable: "ai_agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ai_agent_executions_users_TriggeredByUserId",
                        column: x => x.TriggeredByUserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "app_webhooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppInstallationId = table.Column<int>(type: "int", nullable: false),
                    WebhookUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Events = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Secret = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_webhooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_app_webhooks_app_installations_AppInstallationId",
                        column: x => x.AppInstallationId,
                        principalTable: "app_installations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_widgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DashboardId = table.Column<int>(type: "int", nullable: false),
                    WidgetType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DataSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_widgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_widgets_dashboards_DashboardId",
                        column: x => x.DashboardId,
                        principalTable: "dashboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "document_pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ParentPageId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_document_pages_document_pages_ParentPageId",
                        column: x => x.ParentPageId,
                        principalTable: "document_pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_document_pages_documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "key_results",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoalId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ResultType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TargetValue = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    CurrentValue = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_key_results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_key_results_goals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "goals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_key_results_users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "folders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_folders_spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "list_templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultFields = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultStatuses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_list_templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_list_templates_spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "space_members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpaceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccessLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_space_members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_space_members_spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_space_members_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "team_members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team_members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_team_members_teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_team_members_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceRoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Allowed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_role_permissions_workspace_roles_WorkspaceRoleId",
                        column: x => x.WorkspaceRoleId,
                        principalTable: "workspace_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "workspace_user_roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkspaceRoleId = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workspace_user_roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workspace_user_roles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_workspace_user_roles_workspace_roles_WorkspaceRoleId",
                        column: x => x.WorkspaceRoleId,
                        principalTable: "workspace_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_workspace_user_roles_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ai_feedback_ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AiAgentExecutionId = table.Column<int>(type: "int", nullable: true),
                    AiPromptResponseId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    AccuracyRating = table.Column<int>(type: "int", nullable: true),
                    RelevanceRating = table.Column<int>(type: "int", nullable: true),
                    HelpfulnessRating = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImprovementSuggestions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_feedback_ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_feedback_ratings_ai_agent_executions_AiAgentExecutionId",
                        column: x => x.AiAgentExecutionId,
                        principalTable: "ai_agent_executions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ai_feedback_ratings_ai_prompt_responses_AiPromptResponseId",
                        column: x => x.AiPromptResponseId,
                        principalTable: "ai_prompt_responses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ai_feedback_ratings_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FolderId = table.Column<int>(type: "int", nullable: true),
                    SpaceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lists_folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "folders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_lists_spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "automations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    SpaceId = table.Column<int>(type: "int", nullable: true),
                    ListId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TriggerType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConditionLogic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_automations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_automations_lists_ListId",
                        column: x => x.ListId,
                        principalTable: "lists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_automations_spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "spaces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_automations_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_automations_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recurring_tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecurrencePattern = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RecurrenceConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recurring_tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_recurring_tasks_lists_ListId",
                        column: x => x.ListId,
                        principalTable: "lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sprints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Goal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sprints_lists_ListId",
                        column: x => x.ListId,
                        principalTable: "lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpaceId = table.Column<int>(type: "int", nullable: true),
                    ListId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    StatusType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_statuses_lists_ListId",
                        column: x => x.ListId,
                        principalTable: "lists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_statuses_spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "spaces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "views",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: true),
                    SpaceId = table.Column<int>(type: "int", nullable: true),
                    FolderId = table.Column<int>(type: "int", nullable: true),
                    ListId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ViewType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuerySettings = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_views", x => x.Id);
                    table.ForeignKey(
                        name: "FK_views_folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "folders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_views_lists_ListId",
                        column: x => x.ListId,
                        principalTable: "lists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_views_spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "spaces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_views_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "automation_actions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutomationId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_automation_actions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_automation_actions_automations_AutomationId",
                        column: x => x.AutomationId,
                        principalTable: "automations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "automation_executions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutomationId = table.Column<int>(type: "int", nullable: false),
                    TriggeredBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationMs = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_automation_executions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_automation_executions_automations_AutomationId",
                        column: x => x.AutomationId,
                        principalTable: "automations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "automation_triggers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutomationId = table.Column<int>(type: "int", nullable: false),
                    TriggerEvent = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_automation_triggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_automation_triggers_automations_AutomationId",
                        column: x => x.AutomationId,
                        principalTable: "automations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sprint_reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SprintId = table.Column<int>(type: "int", nullable: false),
                    TotalTasks = table.Column<int>(type: "int", nullable: true),
                    CompletedTasks = table.Column<int>(type: "int", nullable: true),
                    TotalPoints = table.Column<int>(type: "int", nullable: true),
                    CompletedPoints = table.Column<int>(type: "int", nullable: true),
                    Velocity = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    BurndownData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprint_reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sprint_reports_sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeEstimate = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tasks_lists_ListId",
                        column: x => x.ListId,
                        principalTable: "lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tasks_statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "statuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tasks_tasks_ParentId",
                        column: x => x.ParentId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tasks_users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "automation_logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutomationId = table.Column<int>(type: "int", nullable: false),
                    ExecutionId = table.Column<int>(type: "int", nullable: true),
                    LogLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_automation_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_automation_logs_automation_executions_ExecutionId",
                        column: x => x.ExecutionId,
                        principalTable: "automation_executions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_automation_logs_automations_AutomationId",
                        column: x => x.AutomationId,
                        principalTable: "automations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    UploaderId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileSize = table.Column<int>(type: "int", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_attachments_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_attachments_users_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "checklists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_checklists_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Resolved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comments_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comments_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "custom_field_values",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomFieldId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_field_values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_custom_field_values_custom_fields_CustomFieldId",
                        column: x => x.CustomFieldId,
                        principalTable: "custom_fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_custom_field_values_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dependencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    DependsOnTaskId = table.Column<int>(type: "int", nullable: false),
                    DependencyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dependencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dependencies_tasks_DependsOnTaskId",
                        column: x => x.DependsOnTaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dependencies_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "effort_estimates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    EstimatedHours = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ActualHours = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    EffortPoints = table.Column<int>(type: "int", nullable: true),
                    ConfidenceLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EstimatedBy = table.Column<int>(type: "int", nullable: false),
                    EstimatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_effort_estimates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_effort_estimates_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_effort_estimates_users_EstimatedBy",
                        column: x => x.EstimatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "field_history",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedBy = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_field_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_field_history_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_field_history_users_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recurring_task_instances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecurringTaskId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recurring_task_instances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_recurring_task_instances_recurring_tasks_RecurringTaskId",
                        column: x => x.RecurringTaskId,
                        principalTable: "recurring_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_recurring_task_instances_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sprint_tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SprintId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    StoryPoints = table.Column<int>(type: "int", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprint_tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sprint_tasks_sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sprint_tasks_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "task_assignees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_assignees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_assignees_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_task_assignees_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "task_relationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceTaskId = table.Column<int>(type: "int", nullable: false),
                    TargetTaskId = table.Column<int>(type: "int", nullable: false),
                    RelationshipType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_relationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_relationships_tasks_SourceTaskId",
                        column: x => x.SourceTaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_task_relationships_tasks_TargetTaskId",
                        column: x => x.TargetTaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "task_tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_tags_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_task_tags_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "task_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    Snapshot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_versions_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_task_versions_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "task_watchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_watchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_watchers_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_task_watchers_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "time_entries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_time_entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_time_entries_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_time_entries_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "checklist_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    AssigneeId = table.Column<int>(type: "int", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checklist_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_checklist_items_checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "checklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checklist_items_users_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "comment_reactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment_reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comment_reactions_comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comment_reactions_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comment_threads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    ReplyToCommentId = table.Column<int>(type: "int", nullable: true),
                    ThreadLevel = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment_threads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comment_threads_comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comment_threads_comments_ReplyToCommentId",
                        column: x => x.ReplyToCommentId,
                        principalTable: "comments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_access_tokens_UserId",
                table: "access_tokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_activity_logs_UserId",
                table: "activity_logs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_activity_logs_WorkspaceId",
                table: "activity_logs",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_activity_snapshots_WorkspaceId_SnapshotDate",
                table: "activity_snapshots",
                columns: new[] { "WorkspaceId", "SnapshotDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_agent_actions_AiAgentId",
                table: "agent_actions",
                column: "AiAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_agent_actions_FallbackActionId",
                table: "agent_actions",
                column: "FallbackActionId");

            migrationBuilder.CreateIndex(
                name: "IX_agent_prompts_AiAgentId",
                table: "agent_prompts",
                column: "AiAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_agent_executions_AiAgentId",
                table: "ai_agent_executions",
                column: "AiAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_agent_executions_TriggeredByUserId",
                table: "ai_agent_executions",
                column: "TriggeredByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_agents_CreatedBy",
                table: "ai_agents",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ai_agents_WorkspaceId",
                table: "ai_agents",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_feedback_ratings_AiAgentExecutionId",
                table: "ai_feedback_ratings",
                column: "AiAgentExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_feedback_ratings_AiPromptResponseId",
                table: "ai_feedback_ratings",
                column: "AiPromptResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_feedback_ratings_UserId",
                table: "ai_feedback_ratings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_knowledge_base_LastUpdatedById",
                table: "ai_knowledge_base",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ai_knowledge_base_WorkspaceId",
                table: "ai_knowledge_base",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_model_configs_WorkspaceId_ModelName",
                table: "ai_model_configs",
                columns: new[] { "WorkspaceId", "ModelName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ai_model_usage_WorkspaceId_ModelName_UsageDate",
                table: "ai_model_usage",
                columns: new[] { "WorkspaceId", "ModelName", "UsageDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ai_prompt_responses_UserId",
                table: "ai_prompt_responses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_prompt_responses_WorkspaceId",
                table: "ai_prompt_responses",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_skills_CreatedBy",
                table: "ai_skills",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ai_skills_WorkspaceId_Name",
                table: "ai_skills",
                columns: new[] { "WorkspaceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_installations_AppId_WorkspaceId",
                table: "app_installations",
                columns: new[] { "AppId", "WorkspaceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_installations_InstalledBy",
                table: "app_installations",
                column: "InstalledBy");

            migrationBuilder.CreateIndex(
                name: "IX_app_installations_WorkspaceId",
                table: "app_installations",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_app_permissions_AppId",
                table: "app_permissions",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_app_webhooks_AppInstallationId",
                table: "app_webhooks",
                column: "AppInstallationId");

            migrationBuilder.CreateIndex(
                name: "IX_apps_DeveloperId",
                table: "apps",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_attachments_TaskId",
                table: "attachments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_attachments_UploaderId",
                table: "attachments",
                column: "UploaderId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_UserId",
                table: "audit_events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_WorkspaceId",
                table: "audit_events",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_automation_actions_AutomationId",
                table: "automation_actions",
                column: "AutomationId");

            migrationBuilder.CreateIndex(
                name: "IX_automation_executions_AutomationId",
                table: "automation_executions",
                column: "AutomationId");

            migrationBuilder.CreateIndex(
                name: "IX_automation_logs_AutomationId",
                table: "automation_logs",
                column: "AutomationId");

            migrationBuilder.CreateIndex(
                name: "IX_automation_logs_ExecutionId",
                table: "automation_logs",
                column: "ExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_automation_triggers_AutomationId",
                table: "automation_triggers",
                column: "AutomationId");

            migrationBuilder.CreateIndex(
                name: "IX_automations_CreatedBy",
                table: "automations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_automations_ListId",
                table: "automations",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_automations_SpaceId",
                table: "automations",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_automations_WorkspaceId",
                table: "automations",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_checklist_items_AssigneeId",
                table: "checklist_items",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_checklist_items_ChecklistId",
                table: "checklist_items",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_checklists_TaskId",
                table: "checklists",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_comment_reactions_CommentId_UserId_Emoji",
                table: "comment_reactions",
                columns: new[] { "CommentId", "UserId", "Emoji" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_comment_reactions_UserId",
                table: "comment_reactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_comment_threads_CommentId",
                table: "comment_threads",
                column: "CommentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_comment_threads_ReplyToCommentId",
                table: "comment_threads",
                column: "ReplyToCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_TaskId",
                table: "comments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_UserId",
                table: "comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_custom_field_values_CustomFieldId_TaskId",
                table: "custom_field_values",
                columns: new[] { "CustomFieldId", "TaskId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_custom_field_values_TaskId",
                table: "custom_field_values",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_custom_fields_WorkspaceId_Name",
                table: "custom_fields",
                columns: new[] { "WorkspaceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_widgets_DashboardId",
                table: "dashboard_widgets",
                column: "DashboardId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboards_CreatedBy",
                table: "dashboards",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_dashboards_WorkspaceId",
                table: "dashboards",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_dependencies_DependsOnTaskId",
                table: "dependencies",
                column: "DependsOnTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_dependencies_TaskId_DependsOnTaskId",
                table: "dependencies",
                columns: new[] { "TaskId", "DependsOnTaskId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_document_pages_DocumentId",
                table: "document_pages",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_document_pages_ParentPageId",
                table: "document_pages",
                column: "ParentPageId");

            migrationBuilder.CreateIndex(
                name: "IX_documents_CreatorId",
                table: "documents",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_documents_WorkspaceId",
                table: "documents",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_effort_estimates_EstimatedBy",
                table: "effort_estimates",
                column: "EstimatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_effort_estimates_TaskId",
                table: "effort_estimates",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_field_history_ChangedBy",
                table: "field_history",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_field_history_TaskId",
                table: "field_history",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_folders_SpaceId",
                table: "folders",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_goals_OwnerId",
                table: "goals",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_goals_WorkspaceId",
                table: "goals",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_integrations_WorkspaceId_ServiceName",
                table: "integrations",
                columns: new[] { "WorkspaceId", "ServiceName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_invitations_InvitedBy",
                table: "invitations",
                column: "InvitedBy");

            migrationBuilder.CreateIndex(
                name: "IX_invitations_WorkspaceId",
                table: "invitations",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_key_results_GoalId",
                table: "key_results",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_key_results_OwnerId",
                table: "key_results",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_list_templates_SpaceId",
                table: "list_templates",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_lists_FolderId",
                table: "lists",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_lists_SpaceId",
                table: "lists",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_UserId",
                table: "notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_permission_grants_UserId",
                table: "permission_grants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_priority_levels_WorkspaceId_Name",
                table: "priority_levels",
                columns: new[] { "WorkspaceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recurring_task_instances_RecurringTaskId_SequenceNumber",
                table: "recurring_task_instances",
                columns: new[] { "RecurringTaskId", "SequenceNumber" },
                unique: true,
                filter: "[SequenceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_recurring_task_instances_TaskId",
                table: "recurring_task_instances",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_recurring_tasks_ListId",
                table: "recurring_tasks",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_reports_CreatedBy",
                table: "reports",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_reports_WorkspaceId",
                table: "reports",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_WorkspaceRoleId_PermissionName",
                table: "role_permissions",
                columns: new[] { "WorkspaceRoleId", "PermissionName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_jobs_WorkspaceId",
                table: "scheduled_jobs",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_space_members_SpaceId_UserId",
                table: "space_members",
                columns: new[] { "SpaceId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_space_members_UserId",
                table: "space_members",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_space_templates_WorkspaceId",
                table: "space_templates",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_spaces_WorkspaceId",
                table: "spaces",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_reports_SprintId",
                table: "sprint_reports",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_tasks_SprintId_TaskId",
                table: "sprint_tasks",
                columns: new[] { "SprintId", "TaskId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sprint_tasks_TaskId",
                table: "sprint_tasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_sprints_ListId",
                table: "sprints",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_statuses_ListId",
                table: "statuses",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_statuses_SpaceId",
                table: "statuses",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_system_role_claims_RoleId",
                table: "system_role_claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "system_roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_system_user_roles_RoleId",
                table: "system_user_roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tags_WorkspaceId_Name",
                table: "tags",
                columns: new[] { "WorkspaceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_assignees_TaskId_UserId",
                table: "task_assignees",
                columns: new[] { "TaskId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_assignees_UserId",
                table: "task_assignees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_task_relationships_SourceTaskId_TargetTaskId",
                table: "task_relationships",
                columns: new[] { "SourceTaskId", "TargetTaskId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_relationships_TargetTaskId",
                table: "task_relationships",
                column: "TargetTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_task_tags_TagId",
                table: "task_tags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_task_tags_TaskId_TagId",
                table: "task_tags",
                columns: new[] { "TaskId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_versions_CreatedBy",
                table: "task_versions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_task_versions_TaskId_VersionNumber",
                table: "task_versions",
                columns: new[] { "TaskId", "VersionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_watchers_TaskId_UserId",
                table: "task_watchers",
                columns: new[] { "TaskId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_watchers_UserId",
                table: "task_watchers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_CreatorId",
                table: "tasks",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_ListId",
                table: "tasks",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_ParentId",
                table: "tasks",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_StatusId",
                table: "tasks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_team_members_TeamId_UserId",
                table: "team_members",
                columns: new[] { "TeamId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_team_members_UserId",
                table: "team_members",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_teams_WorkspaceId",
                table: "teams",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_templates_CreatedBy",
                table: "templates",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_templates_WorkspaceId",
                table: "templates",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_time_entries_TaskId",
                table: "time_entries",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_time_entries_UserId",
                table: "time_entries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_claims_UserId",
                table: "user_claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_logins_UserId",
                table: "user_logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_preferences_UserId",
                table: "user_preferences",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_sessions_UserId",
                table: "user_sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_vector_embeddings_WorkspaceId",
                table: "vector_embeddings",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_views_FolderId",
                table: "views",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_views_ListId",
                table: "views",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_views_SpaceId",
                table: "views",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_views_WorkspaceId",
                table: "views",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_webhooks_WorkspaceId",
                table: "webhooks",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_workspace_members_UserId",
                table: "workspace_members",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_workspace_members_WorkspaceId_UserId",
                table: "workspace_members",
                columns: new[] { "WorkspaceId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workspace_roles_WorkspaceId_Name",
                table: "workspace_roles",
                columns: new[] { "WorkspaceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workspace_user_roles_UserId",
                table: "workspace_user_roles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_workspace_user_roles_WorkspaceId_UserId_WorkspaceRoleId",
                table: "workspace_user_roles",
                columns: new[] { "WorkspaceId", "UserId", "WorkspaceRoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workspace_user_roles_WorkspaceRoleId",
                table: "workspace_user_roles",
                column: "WorkspaceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_workspaces_OwnerId",
                table: "workspaces",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_tokens");

            migrationBuilder.DropTable(
                name: "activity_logs");

            migrationBuilder.DropTable(
                name: "activity_snapshots");

            migrationBuilder.DropTable(
                name: "agent_actions");

            migrationBuilder.DropTable(
                name: "agent_prompts");

            migrationBuilder.DropTable(
                name: "ai_feedback_ratings");

            migrationBuilder.DropTable(
                name: "ai_knowledge_base");

            migrationBuilder.DropTable(
                name: "ai_model_configs");

            migrationBuilder.DropTable(
                name: "ai_model_usage");

            migrationBuilder.DropTable(
                name: "ai_skills");

            migrationBuilder.DropTable(
                name: "app_permissions");

            migrationBuilder.DropTable(
                name: "app_webhooks");

            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropTable(
                name: "audit_events");

            migrationBuilder.DropTable(
                name: "automation_actions");

            migrationBuilder.DropTable(
                name: "automation_logs");

            migrationBuilder.DropTable(
                name: "automation_triggers");

            migrationBuilder.DropTable(
                name: "checklist_items");

            migrationBuilder.DropTable(
                name: "comment_reactions");

            migrationBuilder.DropTable(
                name: "comment_threads");

            migrationBuilder.DropTable(
                name: "custom_field_values");

            migrationBuilder.DropTable(
                name: "dashboard_widgets");

            migrationBuilder.DropTable(
                name: "dependencies");

            migrationBuilder.DropTable(
                name: "document_pages");

            migrationBuilder.DropTable(
                name: "effort_estimates");

            migrationBuilder.DropTable(
                name: "field_history");

            migrationBuilder.DropTable(
                name: "integrations");

            migrationBuilder.DropTable(
                name: "invitations");

            migrationBuilder.DropTable(
                name: "key_results");

            migrationBuilder.DropTable(
                name: "list_templates");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "permission_grants");

            migrationBuilder.DropTable(
                name: "priority_levels");

            migrationBuilder.DropTable(
                name: "recurring_task_instances");

            migrationBuilder.DropTable(
                name: "reports");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "scheduled_jobs");

            migrationBuilder.DropTable(
                name: "space_members");

            migrationBuilder.DropTable(
                name: "space_templates");

            migrationBuilder.DropTable(
                name: "sprint_reports");

            migrationBuilder.DropTable(
                name: "sprint_tasks");

            migrationBuilder.DropTable(
                name: "system_role_claims");

            migrationBuilder.DropTable(
                name: "system_user_roles");

            migrationBuilder.DropTable(
                name: "task_assignees");

            migrationBuilder.DropTable(
                name: "task_relationships");

            migrationBuilder.DropTable(
                name: "task_tags");

            migrationBuilder.DropTable(
                name: "task_versions");

            migrationBuilder.DropTable(
                name: "task_watchers");

            migrationBuilder.DropTable(
                name: "team_members");

            migrationBuilder.DropTable(
                name: "templates");

            migrationBuilder.DropTable(
                name: "time_entries");

            migrationBuilder.DropTable(
                name: "user_claims");

            migrationBuilder.DropTable(
                name: "user_logins");

            migrationBuilder.DropTable(
                name: "user_preferences");

            migrationBuilder.DropTable(
                name: "user_sessions");

            migrationBuilder.DropTable(
                name: "user_tokens");

            migrationBuilder.DropTable(
                name: "vector_embeddings");

            migrationBuilder.DropTable(
                name: "views");

            migrationBuilder.DropTable(
                name: "webhooks");

            migrationBuilder.DropTable(
                name: "workspace_members");

            migrationBuilder.DropTable(
                name: "workspace_user_roles");

            migrationBuilder.DropTable(
                name: "ai_agent_executions");

            migrationBuilder.DropTable(
                name: "ai_prompt_responses");

            migrationBuilder.DropTable(
                name: "app_installations");

            migrationBuilder.DropTable(
                name: "automation_executions");

            migrationBuilder.DropTable(
                name: "checklists");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "custom_fields");

            migrationBuilder.DropTable(
                name: "dashboards");

            migrationBuilder.DropTable(
                name: "documents");

            migrationBuilder.DropTable(
                name: "goals");

            migrationBuilder.DropTable(
                name: "recurring_tasks");

            migrationBuilder.DropTable(
                name: "sprints");

            migrationBuilder.DropTable(
                name: "system_roles");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "workspace_roles");

            migrationBuilder.DropTable(
                name: "ai_agents");

            migrationBuilder.DropTable(
                name: "apps");

            migrationBuilder.DropTable(
                name: "automations");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "statuses");

            migrationBuilder.DropTable(
                name: "lists");

            migrationBuilder.DropTable(
                name: "folders");

            migrationBuilder.DropTable(
                name: "spaces");

            migrationBuilder.DropTable(
                name: "workspaces");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
