FROM microsoft/dotnet:2.0-sdk
WORKDIR /app

COPY *.sln ./
COPY PTZ.HomeManagement.Worker/PTZ.HomeManagement.Worker.csproj PTZ.HomeManagement.Worker/
RUN dotnet restore PTZ.HomeManagement.Worker/PTZ.HomeManagement.Worker.csproj
COPY . .

WORKDIR /app/PTZ.HomeManagement.Worker

ENV MailGun_ApiKey="";
ENV MailGun_ApiBaseUri="";
ENV MailGun_RequestUri="";
ENV MailGun_From="";
ENV MailGun_Domain="";

RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/PTZ.HomeManagement.Worker.dll"]