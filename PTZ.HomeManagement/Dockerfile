FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

ARG VCS_REF

LABEL org.label-schema.vcs-ref=$VCS_REF \
        org.label-schema.vcs-url="e.g. https://github.com/ptorrezao/PTZ.HomeManagement"

#Preapare Info for Db Connection
ENV SQLSERVER_HOST=sql
ENV SQLSERVER_PASSWORD=password
ENV SQLSERVER_USERNAME=sa
ENV DB_NAME=PTZHomeManagement
ENV DB_TYPE=SQLLite
ENV Sentry_DSN={YourKeyHere}
ENV Sentry_IncludeActivityData=True
ENV Sentry_IncludeRequestPayload=True
ENV Sentry_MinimumBreadcrumbLevel=Debug
ENV Sentry_MinimumEventLevel=Debug

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY PTZ.HomeManagement/PTZ.HomeManagement.csproj PTZ.HomeManagement/
RUN dotnet restore
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
