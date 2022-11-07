## DocStorage
A simple document storage API under Postgres database with JWT Authentication and role access. We can manage users, groups, documents and give document access by user or by group
using Postgre, Automapper, Dapper, Npgsql, Swagger

- [x] Login through API
    - An user with 'Regular' role can download documents
    - An user with 'Manager' role can upload and download documents
    - An user with 'Admin' role can manage user/group/document/user-group/document/document-access
- [x] Manage users
    - A user can belong to one or more groups
- [x] Manage groups
- [x] Manage user-groups
- [x] Manage documents
- [x] Manage document access by group or user

Here is the database structure
- /DocStorageDapper/DocStorage.Repository/Resources/createstructure.sql

![image](https://user-images.githubusercontent.com/990917/200243757-d3fdc9ed-c8a8-4908-a09d-145acdf143ec.png)
