PTZ.HomeManagement
============
[![GitHub Stars](https://img.shields.io/github/stars/ptorrezao/PTZ.HomeManagement.svg)](https://github.com/ptorrezao/PTZ.HomeManagement/stargazers)
[![GitHub Issues](https://img.shields.io/github/issues/ptorrezao/PTZ.HomeManagement.svg)](https://github.com/ptorrezao/PTZ.HomeManagement/issues)
[![Build Status](https://travis-ci.org/ptorrezao/PTZ.HomeManagement.svg?branch=master)](https://travis-ci.org/ptorrezao/PTZ.HomeManagement)
[![Sonarcloud Status](https://sonarcloud.io/api/project_badges/measure?project=PTZHZ&metric=alert_status)](https://sonarcloud.io/dashboard?id=PTZHZ)
![](https://sonarcloud.io/api/project_badges/measure?project=PTZHZ&metric=coverage)
[![](https://images.microbadger.com/badges/version/ptorrezao/ptz.homemanagement.svg)](https://microbadger.com/images/ptorrezao/ptz.homemanagement "ptorrezao/ptz.homemanagement Docker Image")  
[![](https://images.microbadger.com/badges/image/ptorrezao/ptz.homemanagement.svg)](https://microbadger.com/images/ptorrezao/ptz.homemanagement "ptorrezao/ptz.homemanagement Docker Image")
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT) 

This is a asp.net webapp application powered by ASP.NET Core and Docker and main function is manage the household items (Warranties, etc).
(This started as Pet Project to learn ASP.NET Core and Docker, so keep that in mind)

![Preview](https://github.com/ptorrezao/PTZ.HomeManagement/blob/master/docs/preview.jpg?raw=true)
![Preview2](https://github.com/ptorrezao/PTZ.HomeManagement/blob/master/docs/preview2.jpg?raw=true)
![Preview3](https://github.com/ptorrezao/PTZ.HomeManagement/blob/master/docs/preview3.jpg?raw=true)

---

## Components/Modules
- MyFinance -> Allows me to keep track of Expenses and Incomes
- ExpirationReminder -> Allows me to keep on top of the calendar and expiration dates for services.

## Features
- Admin Template (light-bootstrap-dashboard) [Creative Tim](https://www.creative-tim.com/product/light-bootstrap-dashboard)
- Bootstrap-select [bootstrap-select](https://silviomoreto.github.io/bootstrap-select/)
- Mailgun [Mailgun](https://www.mailgun.com/email-api)
- Tiny Colorpicker - A lightweight cross browser color picker. [Page](http://baijs.com/tinycolorpicker/)

## WorkFlow
![WorkFlow](https://github.com/ptorrezao/PTZ.HomeManagement/blob/master/docs/workflow.png?raw=true)

## Informations
- Default User:Pwd (on first login will request to change the password)
	- admin@hmptz.local:Ch4ng3_Th1s 
		
## DBMS (via ORM EF)
### Configuration of DBMS (via Enviroment Variable [Case Sensitive])
	- DB_TYPE = SQLLite, SqlServer or PostgreSQL
	- DB_HOST=${DB_HOST} well you know..
	- DB_USER=${DB_USER}
	- DB_PASSWORD=${DB_PASSWORD}
	- DB_NAME=${DB_NAME}
	
## Logging (Error) (via Enviroment Variable [Case Sensitive])
### Log erros on Sentry 
	- Sentry_DSN=${Sentry_DSN}
	
## Email (via Enviroment Variable [Case Sensitive])
### MailGun
      - MailGun_ApiKey=${MailGun_ApiKey}
      - MailGun_ApiBaseUri=${MailGun_ApiBaseUri}
      - MailGun_RequestUri=${MailGun_RequestUri}
      - MailGun_From=${MailGun_From}
	  - MailGun_Domain=${MailGun_Domain}
## License
This project is licensed under the terms of the **MIT** license.
>You can check out the full license [here](https://github.com/ptorrezao/PTZ.HomeManagement/blob/master/LICENSE)

