# Commands

```
dotnet aspnet-codegenerator controller -name StudentsController -m Student -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries

dotnet ef migrations add InitialCreate

dotnet aspnet-codegenerator controller -name CoursesController -m Course -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
dotnet aspnet-codegenerator controller -name DepartmentsController -m Department -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
```