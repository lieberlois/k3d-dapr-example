#!/usr/bin/env bash

build() {
    echo Building docker images ...
    docker build -t posts-service ./PostsService
    docker build -t analytics-service ./AnalyticsService
    docker build -t url-service ./UrlService
    wait
}

tag() {
    echo Tagging images ...
    docker tag posts-service k3d-registry.localhost:5000/posts-service
    docker tag analytics-service k3d-registry.localhost:5000/analytics-service
    docker tag url-service k3d-registry.localhost:5000/url-service
    wait
}

push() {
    echo Pushing to registry ...
    docker push k3d-registry.localhost:5000/posts-service
    docker push k3d-registry.localhost:5000/analytics-service
    docker push k3d-registry.localhost:5000/url-service
    wait
}

build
tag
push

echo Rolling out deployments ...
sh $(pwd)/bin/restart.sh