#!/usr/bin/env bash

rollout() {
    kubectl rollout restart deployment posts-depl > /dev/null
    kubectl rollout restart deployment analytics-depl > /dev/null
    kubectl rollout restart deployment url-depl > /dev/null
}

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
running_deployments=$(kubectl get deployments)

if [[ 
    $running_deployments && 
    "$running_deployments" =~ "posts-depl" && 
    "$running_deployments" =~ "analytics-depl" && 
    "$running_deployments" =~ "url-depl" 
]]; then
    rollout
else
    kubectl delete -f ./k8s/posts-depl.yaml
    kubectl delete -f ./k8s/analytics-depl.yaml
    kubectl delete -f ./k8s/url-depl.yaml
    kubectl apply -f ./k8s
fi