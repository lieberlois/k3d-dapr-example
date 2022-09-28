#!/bin/bash
set -euo pipefail

echo Restarting deployments ...
if ! kubectl get deployments | grep posts-depl; then 
    kubectl apply -f ./k8s
else
    kubectl rollout restart deployment/posts-depl
    kubectl rollout restart deployment/analytics-depl
    kubectl rollout restart deployment/url-depl
    kubectl rollout restart deployment/client-depl
fi
echo Done!