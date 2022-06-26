# ProjectTracker

 EF Command Lines
---
dotnet-ef migrations add <MigrationName> --project ProjectTracker.Data --startup-project ProjectTracker.Ui.Api
dotnet-ef database update --project ProjectTracker.Data --startup-project ProjectTracker.Ui.Api

Docker Commands
---
docker run -p 8200:8200 -e 'VAULT_DEV_ROOT_TOKEN_ID=dev-only-token' vault
