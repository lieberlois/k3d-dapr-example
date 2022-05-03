#!/usr/bin/env bash

rollout() {
    kubectl rollout restart deployment posts-depl
    kubectl rollout restart deployment analytics-depl
    kubectl rollout restart deployment url-depl
}

build() {
    echo Building docker images ...
    docker build -t posts-service ./PostsService &
    docker build -t analytics-service ./AnalyticsService &
    docker build -t url-service ./UrlService &
    wait
}

tag() {
    echo Tagging images ...
    docker tag posts-service k3d-registry.localhost:5000/posts-service &
    docker tag analytics-service k3d-registry.localhost:5000/analytics-service &
    docker tag url-service k3d-registry.localhost:5000/url-service &
    wait
}

push() {
    echo Pushing to registry ...
    docker push k3d-registry.localhost:5000/posts-service &
    docker push k3d-registry.localhost:5000/analytics-service &
    docker push k3d-registry.localhost:5000/url-service &
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