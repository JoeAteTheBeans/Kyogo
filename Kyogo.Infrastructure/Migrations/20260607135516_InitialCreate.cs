using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kyogo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "deck_subscriptions",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    deck_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deck_subscriptions", x => new { x.deck_id, x.user_id });
                });

            migrationBuilder.CreateTable(
                name: "decks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    artwork_path = table.Column<string>(type: "text", nullable: true),
                    terms = table.Column<Guid[]>(type: "uuid[]", nullable: false),
                    kanji = table.Column<string[]>(type: "text[]", nullable: false),
                    deck_type = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_decks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kanji",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    onyoumi_readings = table.Column<string[]>(type: "text[]", nullable: false),
                    kunyoumi_readings = table.Column<string[]>(type: "text[]", nullable: false),
                    meanings = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kanji", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "progress_records",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    term_id = table.Column<Guid>(type: "uuid", nullable: false),
                    card_type = table.Column<int>(type: "integer", nullable: false),
                    fluent = table.Column<bool>(type: "boolean", nullable: false),
                    stage = table.Column<string>(type: "text", nullable: false),
                    due_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    interval = table.Column<TimeSpan>(type: "interval", nullable: false),
                    step = table.Column<int>(type: "integer", nullable: true),
                    retained_interval = table.Column<TimeSpan>(type: "interval", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_progress_records", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    raw_token = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revoked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "term_modifications",
                columns: table => new
                {
                    term_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_term_modifications", x => new { x.term_id, x.owner_id });
                });

            migrationBuilder.CreateTable(
                name: "terms",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    components = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_terms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sense_addition",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    part_of_speech = table.Column<string>(type: "text", nullable: false),
                    common = table.Column<bool>(type: "boolean", nullable: false),
                    tags = table.Column<string[]>(type: "text[]", nullable: false),
                    insertion_index = table.Column<int>(type: "integer", nullable: false),
                    term_modification_owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    term_modification_term_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sense_addition", x => x.id);
                    table.ForeignKey(
                        name: "fk_sense_addition_term_modifications_term_modification_term_id",
                        columns: x => new { x.term_modification_term_id, x.term_modification_owner_id },
                        principalTable: "term_modifications",
                        principalColumns: new[] { "term_id", "owner_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sense_modification",
                columns: table => new
                {
                    sense_id = table.Column<Guid>(type: "uuid", nullable: false),
                    part_of_speech_override = table.Column<string>(type: "text", nullable: true),
                    common_override = table.Column<bool>(type: "boolean", nullable: true),
                    tags_override = table.Column<string[]>(type: "text[]", nullable: true),
                    term_modification_owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    term_modification_term_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sense_modification", x => x.sense_id);
                    table.ForeignKey(
                        name: "fk_sense_modification_term_modifications_term_modification_ter",
                        columns: x => new { x.term_modification_term_id, x.term_modification_owner_id },
                        principalTable: "term_modifications",
                        principalColumns: new[] { "term_id", "owner_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sense_removal",
                columns: table => new
                {
                    remove_sense_id = table.Column<Guid>(type: "uuid", nullable: false),
                    term_modification_owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    term_modification_term_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sense_removal", x => x.remove_sense_id);
                    table.ForeignKey(
                        name: "fk_sense_removal_term_modifications_term_modification_term_id_",
                        columns: x => new { x.term_modification_term_id, x.term_modification_owner_id },
                        principalTable: "term_modifications",
                        principalColumns: new[] { "term_id", "owner_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sense",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    part_of_speech = table.Column<string>(type: "text", nullable: false),
                    common = table.Column<bool>(type: "boolean", nullable: false),
                    tags = table.Column<string[]>(type: "text[]", nullable: false),
                    term_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sense", x => x.id);
                    table.ForeignKey(
                        name: "fk_sense_terms_term_id",
                        column: x => x.term_id,
                        principalTable: "terms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "custom_gloss",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    primary = table.Column<bool>(type: "boolean", nullable: false),
                    sense_addition_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_custom_gloss", x => x.id);
                    table.ForeignKey(
                        name: "fk_custom_gloss_sense_addition_sense_addition_id",
                        column: x => x.sense_addition_id,
                        principalTable: "sense_addition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gloss_addition",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    primary = table.Column<bool>(type: "boolean", nullable: false),
                    insertion_index = table.Column<int>(type: "integer", nullable: false),
                    sense_modification_sense_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gloss_addition", x => x.id);
                    table.ForeignKey(
                        name: "fk_gloss_addition_sense_modification_sense_modification_sense_",
                        column: x => x.sense_modification_sense_id,
                        principalTable: "sense_modification",
                        principalColumn: "sense_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gloss_modification",
                columns: table => new
                {
                    modify_gloss_id = table.Column<Guid>(type: "uuid", nullable: false),
                    text_override = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    primary_override = table.Column<bool>(type: "boolean", nullable: true),
                    sense_modification_sense_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gloss_modification", x => x.modify_gloss_id);
                    table.ForeignKey(
                        name: "fk_gloss_modification_sense_modification_sense_modification_se",
                        column: x => x.sense_modification_sense_id,
                        principalTable: "sense_modification",
                        principalColumn: "sense_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gloss_removal",
                columns: table => new
                {
                    remove_gloss_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sense_modification_sense_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gloss_removal", x => x.remove_gloss_id);
                    table.ForeignKey(
                        name: "fk_gloss_removal_sense_modification_sense_modification_sense_id",
                        column: x => x.sense_modification_sense_id,
                        principalTable: "sense_modification",
                        principalColumn: "sense_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gloss",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    primary = table.Column<bool>(type: "boolean", nullable: false),
                    sense_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gloss", x => x.id);
                    table.ForeignKey(
                        name: "fk_gloss_sense_sense_id",
                        column: x => x.sense_id,
                        principalTable: "sense",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_custom_gloss_sense_addition_id",
                table: "custom_gloss",
                column: "sense_addition_id");

            migrationBuilder.CreateIndex(
                name: "ix_gloss_sense_id",
                table: "gloss",
                column: "sense_id");

            migrationBuilder.CreateIndex(
                name: "ix_gloss_addition_sense_modification_sense_id",
                table: "gloss_addition",
                column: "sense_modification_sense_id");

            migrationBuilder.CreateIndex(
                name: "ix_gloss_modification_sense_modification_sense_id",
                table: "gloss_modification",
                column: "sense_modification_sense_id");

            migrationBuilder.CreateIndex(
                name: "ix_gloss_removal_sense_modification_sense_id",
                table: "gloss_removal",
                column: "sense_modification_sense_id");

            migrationBuilder.CreateIndex(
                name: "ix_sense_term_id",
                table: "sense",
                column: "term_id");

            migrationBuilder.CreateIndex(
                name: "ix_sense_addition_term_modification_term_id_term_modification_",
                table: "sense_addition",
                columns: new[] { "term_modification_term_id", "term_modification_owner_id" });

            migrationBuilder.CreateIndex(
                name: "ix_sense_modification_term_modification_term_id_term_modificat",
                table: "sense_modification",
                columns: new[] { "term_modification_term_id", "term_modification_owner_id" });

            migrationBuilder.CreateIndex(
                name: "ix_sense_removal_term_modification_term_id_term_modification_o",
                table: "sense_removal",
                columns: new[] { "term_modification_term_id", "term_modification_owner_id" });

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "custom_gloss");

            migrationBuilder.DropTable(
                name: "deck_subscriptions");

            migrationBuilder.DropTable(
                name: "decks");

            migrationBuilder.DropTable(
                name: "gloss");

            migrationBuilder.DropTable(
                name: "gloss_addition");

            migrationBuilder.DropTable(
                name: "gloss_modification");

            migrationBuilder.DropTable(
                name: "gloss_removal");

            migrationBuilder.DropTable(
                name: "kanji");

            migrationBuilder.DropTable(
                name: "progress_records");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "sense_removal");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "sense_addition");

            migrationBuilder.DropTable(
                name: "sense");

            migrationBuilder.DropTable(
                name: "sense_modification");

            migrationBuilder.DropTable(
                name: "terms");

            migrationBuilder.DropTable(
                name: "term_modifications");
        }
    }
}
