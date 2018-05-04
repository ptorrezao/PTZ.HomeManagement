#!/bin/bash
set -ev
dotnet restore PTZ.HomeManagement.sln
dotnet build PTZ.HomeManagement/PTZ.HomeManagement.csproj