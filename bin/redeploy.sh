#!/bin/bash
set -euo pipefail

echo Building docker images ...
docker build -t posts-service ./PostsService
docker build -t analytics-service ./AnalyticsService
docker build -t url-service ./UrlService
docker build -t client ./client

echo Importing images...
k3d -c dapr-cluster image import posts-service
k3d -c dapr-cluster image import analytics-service
k3d -c dapr-cluster image import url-service
k3d -c dapr-cluster image import client

echo Rolling out deployments ...
sh $(pwd)/bin/restart.sh