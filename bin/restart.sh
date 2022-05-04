#!/usr/bin/env bash

echo Restarting deployments ...
kubectl rollout restart deployment posts-depl > /dev/null
kubectl rollout restart deployment analytics-depl > /dev/null
kubectl rollout restart deployment url-depl > /dev/null
echo Done!