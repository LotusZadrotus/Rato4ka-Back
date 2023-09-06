DROP TABLE "Scores";
DROP TABLE "Contents_categories";
DROP TABLE "Score_categories";
DROP TABLE "Categories";
DROP TABLE "Creditionals";
DROP TABLE "Links_user";
DROP TABLE "Links_content";
DROP TABLE "Links_season";
DROP TABLE "Episodes";
DROP TABLE "Seasons";

DROP TABLE "Contents";



DROP TABLE "Users";




CREATE TABLE "Users" (
    "id" SERIAL PRIMARY KEY,
    "avatar" BYTEA NULL,
    "discord_id" VARCHAR(256) NULL,
    "password" VARCHAR(256) NOT NULL,
    "email" VARCHAR(128),
    "is_admin" BOOLEAN NOT NULL DEFAULT FALSE,
    "name" VARCHAR(64) NOT NULL,
    "login" VARCHAR(32) NOT NULL UNIQUE,
    "salt" VARCHAR(128) NOT NULL,
    "is_email_confirmed" BOOLEAN NOT NULL DEFAULT FALSE,
    "desc" TEXT
);
CREATE TABLE "Categories" (
    "id" SERIAL PRIMARY KEY,
    "desc" text NULL
);
CREATE TABLE "Creditionals" (
    "login" VARCHAR(32),
    "key" VARCHAR(128) UNIQUE,
    "email" BOOLEAN NOT NULL,
    "exp_date" DATE NOT NULL
);
CREATE TABLE "Links_user" (
    "id" SERIAL PRIMARY KEY,
    "link" VARCHAR(64) NOT NULL
);
CREATE TABLE "Links_content" (
    "id" SERIAL PRIMARY KEY,
    "link" VARCHAR(64) NOT NULL
);
CREATE TABLE "Links_season" (
    "id" SERIAL PRIMARY KEY,
    "link" VARCHAR(64) NOT NULL
);
CREATE TABLE "Contents" (
    "id" SERIAL PRIMARY KEY,
    "name" VARCHAR(256) NOT NULL,
    "creator_id" INT,
    "created_at" DATE,
    "release_date" DATE,
    "desc" TEXT NULL,
    "image" BYTEA NULL,
    FOREIGN KEY ("creator_id") REFERENCES "Users"("id")
);
CREATE TABLE "Contents_categories" (
	"contents_id" INTEGER REFERENCES "Contents"("id"),
	"categories_id" INTEGER REFERENCES "Categories"("id"),
	PRIMARY KEY ("contents_id", "categories_id")
);
CREATE TABLE "Seasons" (
    "id" SERIAL PRIMARY KEY,
    "name" VARCHAR(256) NOT NULL,
    "content_id" INT,
    "creator_id" INT,
    "release_date" DATE,
    "image" BYTEA NULL,
    "desc" TEXT NULL,
	"season_number" INT NULL,
    "is_movie" BOOLEAN DEFAULT FALSE,
    "duration" INT,
    FOREIGN KEY ("creator_id") REFERENCES "Users"("id"),
    FOREIGN KEY ("content_id") REFERENCES "Contents"("id")
);
CREATE TABLE "Episodes" (
    "id" SERIAL PRIMARY KEY,
    "name" VARCHAR(256) NOT NULL,
    "season_id" INT,
    "episode_number" INT,
    "image" BYTEA NULL,
	"release_date" DATE,
	"duration" INT,
    "desc" TEXT NULL,
    FOREIGN KEY ("season_id") REFERENCES "Seasons"("id")
);
CREATE TABLE "Score_categories" (
    "id" SERIAL PRIMARY KEY,
    "name" VARCHAR(256) NOT NULL,
	"creator_id" INT,
    "season_id" INT,
    "content_id" INT,
    "episode_id" INT,
    "image" BYTEA NULL,
    "desc" TEXT NULL,
    FOREIGN KEY ("season_id") REFERENCES "Seasons"("id"),
    FOREIGN KEY ("content_id") REFERENCES "Contents"("id"),
    FOREIGN KEY ("episode_id") REFERENCES "Episodes"("id"),
    FOREIGN KEY ("creator_id") REFERENCES "Users"("id")
);
CREATE TABLE "Scores" (
	"user_id" INT REFERENCES "Users"("id"),
	"score_cat_id" INT REFERENCES "Score_categories"("id"),
	"score" INT NOT NULL,
	PRIMARY KEY("user_id", "score_cat_id")
);