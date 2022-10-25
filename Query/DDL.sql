CREATE TABLE "Users"{
    "id" SERIAL PRIMARY KEY,
    "avatar" BYTEA NULL,
    "is_admin" BOOLEAN NOT NULL DEFAULT FALSE,
    "is_email_confirmed" BOOLEAN NOT NULL DEFAULT FALSE,
    "name" VARCHAR(64) NOT NULL,
    "login" VARCHAR(32) NOT NULL UNIQUE,
    "password" VARCHAR(256) NOT NULL,
    "salt" VARCHAR(128) NOT NULL,
    "discord_id" VARCHAR(256) NULL,
    "desc" TEXT,
    "email" VARCHAR(128)
};
CREATE TABLE "Categories"{
    "id" SERIAL PRIMARY KEY,
    "desc" text NULL
};
CREATE TABLE "Creditionals"{
    "login" VARCHAR(32),
    "key" VARCHAR(128) UNIQUE,
    "email" VARCHAR(128) NOT NULL,
    "exp_date" DATE NOT NULL
};
CREATE TABLE "Links"{
    "id" SERIAL PRIMARY KEY,
    "link" VARCHAR(64) NOT NULL
};
CREATE TABLE "Contents"{
    "id" SERIAL PRIMARY KEY,
    "name" VARCHAR(256) NOT NULL,
    "categories" INT[],
    "creator_id" INT,
    "created_at" DATE,
    "image" BYTEA NULL,
    "desc" TEXT NULL,
    FOREIGN KEY "creator_id" REFERENCES "Users"("id"),
    FOREIGN KEY (EACH ELEMENT OF "categories") REFERENCES "Categories"("id")
};
CREATE TABLE "Seasons"{
    "id" SERIAL PRIMARY KEY,
    "name" VARCHAR(256) NOT NULL,
    "content_id" INT,
    "release_date" DATE,
    "image" BYTEA NULL,
    "desc" TEXT NULL,
    "is_movie" BOOLEAN DEFAULT FALSE,
    "duration" VARCHAR(8),
    FOREIGN KEY "creator_id" REFERENCES "Users"("id")
};
CREATE TABLE "Episodes"{
    "id" SERIAL PRIMARY KEY,
    "name" VARCHAR(256) NOT NULL,
    "season_id" INT,
    "episode_number" INT,
    "image" BYTEA NULL,
    "desc" TEXT NULL,
    FOREIGN KEY "season_id" REFERENCES "Seasons"("id")
};
CREATE TABLE "Score_categories"{
    "id" SERIAL PRIMARY KEY,
    "name" VARCHAR(256) NOT NULL,
    "season_id" INT,
    "content_id" INT,
    "episode_id" INT,
    "image" BYTEA NULL,
    "desc" TEXT NULL,
    FOREIGN KEY "season_id" REFERENCES "Seasons"("id"),
    FOREIGN KEY "content_id" REFERENCES "Contents"("id"),
    FOREIGN KEY "episode_id" REFERENCES "Episodes"("id")
};