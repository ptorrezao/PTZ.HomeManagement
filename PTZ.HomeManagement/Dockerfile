FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

ARG VCS_REF

LABEL org.label-schema.vcs-ref=$VCS_REF \
        org.label-schema.vcs-url="e.g. https://github.com/ptorrezao/PTZ.HomeManagement"

#Preapare Info for Db Connection
ENV DB_HOST=sql
ENV DB_NAME=
ENV DB_PASSWORD=
ENV DB_TYPE=
ENV DB_USER=

#Sentry
ENV Sentry_DSN=
ENV Sentry_IncludeActivityData=True
ENV Sentry_IncludeRequestPayload=True
ENV Sentry_MinimumBreadcrumbLevel=Debug

#Mailgun
ENV Sentry_MinimumEventLevel=Debug
ENV MailGun_ApiKey="";
ENV MailGun_ApiBaseUri="";
ENV MailGun_RequestUri="";
ENV MailGun_From="";
ENV MailGun_Domain="";

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY . .

COPY PTZ.HomeManagement/PTZ.HomeManagement.csproj PTZ.HomeManagement/
RUN ls PTZ.HomeManagement/
RUN dotnet restore PTZ.HomeManagement/PTZ.HomeManagement.csproj --configfile PTZ.HomeManagement/nuget.config
COPY . .


WORKDIR /src/PTZ.HomeManagement
RUN npm install
RUN npm install gulp

RUN gulp -b "PTZ.HomeManagement" Generate_App
RUN gulp -b "PTZ.HomeManagement" Generate_LBD
RUN gulp -b "PTZ.HomeManagement" CopyEmailTemplates

RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PTZ.HomeManagement.dll"]
