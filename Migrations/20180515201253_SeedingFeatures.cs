﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarSale.Migrations {
    public partial class SeedingFeatures : Migration {
        protected override void Up (MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql ("INSERT INTO Features (Name) VALUES ('Feature1')");
            migrationBuilder.Sql ("INSERT INTO Features (Name) VALUES ('Feature3')");
            migrationBuilder.Sql ("INSERT INTO Features (Name) VALUES ('Feature3')");
        }

        protected override void Down (MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql ("DELETE FROM Features WHERE Name IN ('Feature1','Feature2''Feature3')");
        }
    }
}