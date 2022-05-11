#!/usr/bin/env bash

build() {
    echo Building docker images ...
    docker build -t posts-service ./PostsService > /dev/null &
    docker build -t analytics-service ./AnalyticsService > /dev/null &
    docker build -t url-service ./UrlService > /dev/null &
    wait
}

tag() {
    echo Tagging images ...
    docker tag posts-service k3d-registry.localhost:5000/posts-service > /dev/null &
    docker tag analytics-service k3d-registry.localhost:5000/analytics-service > /dev/null &
    docker tag url-service k3d-registry.localhost:5000/url-service > /dev/null &
    wait
}

push() {
    echo Pushing to registry ...
    docker push k3d-registry.localhost:5000/posts-service > /dev/null &
    docker push k3d-registry.localhost:5000/analytics-service > /dev/null &
    docker push k3d-registry.localhost:5000/url-service > /dev/null &
    wait
}

build
tag
push

echo Rolling out deployments ...
sh $(pwd)/bin/restart.sh