#!/bin/bash
set -ev

dotnet SonarScanner.MSBuild/SonarScanner.MSBuild.dll begin /k:"PTZ.HZ" /d:sonar.organization="ptorrezao-github" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login=$1
dotnet restore PTZ.HomeManagement.sln
dotnet build PTZ.HomeManagement/PTZ.HomeManagement.csproj
dotnet SonarScanner.MSBuild/SonarScanner.MSBuild.dll  end /d:sonar.login=$1