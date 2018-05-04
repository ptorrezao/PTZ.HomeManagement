#!/bin/bash
set -ev

TAG=$1
DOCKER_USERNAME=$2
DOCKER_PASSWORD=$3

docker-compose build 
docker tag ptorrezao/ptz.homemanagement:build ptorrezao/ptz.homemanagement:$TAG

# Login to Docker Hub and upload images
docker login -u="$DOCKER_USERNAME" -p="$DOCKER_PASSWORD"
docker push ptorrezao/ptz.homemanagement:$TAG
docker push ptorrezao/ptz.homemanagement:latest