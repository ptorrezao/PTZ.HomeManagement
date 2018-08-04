#!/bin/bash
set -ev

chmod +x SonarScanner.MSBuild/sonar-scanner-3.2.0.1227/bin/sonar-scanner
dotnet SonarScanner.MSBuild/SonarScanner.MSBuild.dll begin /k:"PTZ.HZ" /d:sonar.organization="ptorrezao-github" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.cpd.exclusions="**/PTZ.HomeManagement.Lib/Data/Migrations/**" /d:sonar.exclusions="**/PTZ.HomeManagement/lib/**,PTZ.HomeManagement/lib/**,**/*bootstrap*/**/*,**/*jquery*,**/*datatables/**/**/*" /d:sonar.login=$1
dotnet restore PTZ.HomeManagement.sln
dotnet build PTZ.HomeManagement.sln
dotnet SonarScanner.MSBuild/SonarScanner.MSBuild.dll  end /d:sonar.login=$1