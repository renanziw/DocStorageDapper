DROP TABLE "usergroup";
DROP TABLE "documentaccess";
DROP TABLE "group";
DROP TABLE "document";
DROP TABLE "user";


CREATE TABLE document (
    "document_id" bigserial NOT NULL,
    "name" text NOT NULL,
    "description" text NOT NULL,
    "category" text NOT NULL,
    "posted_date" timestamp with time zone NOT NULL,
    "extension" text NOT NULL,
    CONSTRAINT "PK_document" PRIMARY KEY ("document_id")
);


CREATE TABLE "group" (
    "group_id" bigserial NOT NULL,
    "name" text NOT NULL,
    CONSTRAINT "PK_group" PRIMARY KEY ("group_id")
);


CREATE TABLE "user" (
    "user_id" bigserial NOT NULL,
    "user_name" text NOT NULL,
    "password_hash" text NOT NULL,
    "role" integer NOT NULL,
    CONSTRAINT "PK_user" PRIMARY KEY ("user_id")
);


CREATE TABLE documentaccess (
    "document_access_id" bigserial NOT NULL,
    "document_id" integer NOT NULL,
    "group_id" integer NULL,
    "user_id" integer NULL,
    CONSTRAINT "PK_documentaccess" PRIMARY KEY ("document_access_id"),
    CONSTRAINT "FK_documentaccess_document_DocumentId" FOREIGN KEY ("document_id") REFERENCES document ("document_id") ON DELETE CASCADE,
    CONSTRAINT "FK_documentaccess_group_GroupId" FOREIGN KEY ("group_id") REFERENCES "group" ("group_id"),
    CONSTRAINT "FK_documentaccess_user_UserId" FOREIGN KEY ("user_id") REFERENCES "user" ("user_id")
);


CREATE TABLE usergroup (
    "user_group_id" bigserial NOT NULL,
    "user_id" integer NOT NULL,
    "group_id" integer NOT NULL,
    CONSTRAINT "PK_usergroup" PRIMARY KEY ("user_group_id"),
    CONSTRAINT "FK_usergroup_group_GroupId" FOREIGN KEY ("group_id") REFERENCES "group" ("group_id") ON DELETE CASCADE,
    CONSTRAINT "FK_usergroup_user_UserId" FOREIGN KEY ("user_id") REFERENCES "user" ("user_id") ON DELETE CASCADE
);


CREATE INDEX "IX_documentaccess_DocumentId" ON documentaccess ("document_id");


CREATE INDEX "IX_documentaccess_GroupId" ON documentaccess ("group_id");


CREATE INDEX "IX_documentaccess_UserId" ON documentaccess ("user_id");


CREATE INDEX "IX_usergroup_GroupId" ON usergroup ("group_id");


CREATE INDEX "IX_usergroup_UserId" ON usergroup ("user_id");

INSERT INTO 
public.user (user_name, password_hash, role) 
VALUES ('Admin', '$2a$11$ajg.SGPuNwmes/h4G6qMyOlA8XO0Fftfix.LOR5Wl.iZWYtieRSde', 0);

INSERT INTO 
public.user (user_name, password_hash, role) 
VALUES ('Manager', '$2a$11$Lr9n4VlRE.vD./1bIawExeUTvH0w/jV/dIIKX3IVzdN3NHueyynZi', 1);

INSERT INTO 
public.user (user_name, password_hash, role) 
VALUES ('Regular', '$2a$11$osTWv0smbRX2quzWMuR9Z.3CbmeFzUpNf/SFRy1Zpv/UdNosYubSm', 2);

INSERT INTO 
public.group (name) 
VALUES ('Admin Group');

INSERT INTO 
public.group (name) 
VALUES ('Manager Group');

INSERT INTO 
public.group (name) 
VALUES ('Regular Group');

INSERT INTO 
public.usergroup (user_id, group_id) 
VALUES (1, 1);

INSERT INTO 
public.usergroup (user_id, group_id) 
VALUES (2, 2);

INSERT INTO 
public.usergroup (user_id, group_id) 
VALUES (3, 3);

INSERT INTO public.document(name, description, category, posted_date, extension)
VALUES ('Document 1', 'Description 1', 'Category 1', NOW(), 'txt');

INSERT INTO public.document(name, description, category, posted_date, extension)
VALUES ('Document 2', 'Description 2', 'Category 2', NOW(), 'txt');

INSERT INTO public.document(name, description, category, posted_date, extension)
VALUES ('Document 3', 'Description 3', 'Category 3', NOW(), 'txt');

INSERT INTO public.documentaccess(document_id, group_id, user_id)
VALUES (1, 1, 1);

INSERT INTO public.documentaccess(document_id, group_id, user_id)
VALUES (2, 2, 2);

INSERT INTO public.documentaccess(document_id, group_id, user_id)
VALUES (3, 3, 3);
