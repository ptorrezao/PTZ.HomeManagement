#!/bin/bash
set -ev

dotnet SonarScanner.MSBuild/SonarScanner.MSBuild.dll begin /k:"PTZHZ" /d:sonar.organization="ptorrezao-github" /d:sonar.cs.opencover.reportsPaths="${dir}/lcov.opencover.xml" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.cpd.exclusions="**/PTZ.HomeManagement.Lib/Data/Migrations/**" /d:sonar.exclusions="**/PTZ.HomeManagement/lib/**,PTZ.HomeManagement/lib/**,**/*bootstrap*/**/*,**/*jquery*,**/*datatables/**/**/*" /d:sonar.login=$1
dotnet build PTZ.HomeManagement.sln
dotnet test PTZ.HomeManagement.MyFinance.Services.Test/PTZ.HomeManagement.MyFinance.Services.Test.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../lcov
dotnet SonarScanner.MSBuild/SonarScanner.MSBuild.dll  end /d:sonar.login=$1