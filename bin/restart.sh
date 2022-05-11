#!/usr/bin/env bash

echo Restarting deployments ...
kubectl delete -f ./k8s/posts-depl.yaml
kubectl delete -f ./k8s/analytics-depl.yaml
kubectl delete -f ./k8s/url-depl.yaml
kubectl apply -f ./k8s
echo Done!