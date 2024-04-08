param ($version='latest')

$currentFolder = $PSScriptRoot
$slnFolder = Join-Path $currentFolder "../../"

Write-Host "********* BUILDING DbMigrator *********" -ForegroundColor Green
$dbMigratorFolder = Join-Path $slnFolder "src/DashBoardTest01.DbMigrator"
Set-Location $dbMigratorFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t mycompanyname/dashboardtest01-db-migrator:$version .

Write-Host "********* BUILDING Angular Application *********" -ForegroundColor Green
$angularAppFolder = Join-Path $slnFolder "../angular"
Set-Location $angularAppFolder
yarn
npm run build:prod
docker build -f Dockerfile.local -t mycompanyname/dashboardtest01-angular:$version .

Write-Host "********* BUILDING Api.Host Application *********" -ForegroundColor Green
$hostFolder = Join-Path $slnFolder "src/DashBoardTest01.HttpApi.Host"
Set-Location $hostFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t mycompanyname/dashboardtest01-api:$version .

### ALL COMPLETED
Write-Host "COMPLETED" -ForegroundColor Green
Set-Location $currentFolder