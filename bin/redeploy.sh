#!/usr/bin/env bash

build() {
    echo Building docker images ...
    docker build -t posts-service ./PostsService
    docker build -t analytics-service ./AnalyticsService
    docker build -t url-service ./UrlService
    docker build -t client ./client
}

tag() {
    echo Tagging images ...
    docker tag posts-service k3d-registry.localhost:5000/posts-service
    docker tag analytics-service k3d-registry.localhost:5000/analytics-service
    docker tag url-service k3d-registry.localhost:5000/url-service
    docker tag client k3d-registry.localhost:5000/client
}

push() {
    echo Pushing to registry ...
    docker push k3d-registry.localhost:5000/posts-service
    docker push k3d-registry.localhost:5000/analytics-service
    docker push k3d-registry.localhost:5000/url-service
    docker push k3d-registry.localhost:5000/client
}

build
tag
push

echo Rolling out deployments ...
sh $(pwd)/bin/restart.sh