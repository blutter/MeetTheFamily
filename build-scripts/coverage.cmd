dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:Exclude="[xunit*]*" /p:CoverletOutput=./lcov.info src\FamilyTreeTests