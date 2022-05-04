#!/usr/bin/env bash

redis_depl_name="redis-depl"
rabbitmq_depl_name="rabbitmq-depl"

rollout() {
    kubectl rollout restart deployment posts-depl > /dev/null
    kubectl rollout restart deployment analytics-depl > /dev/null
    kubectl rollout restart deployment url-depl > /dev/null
}

redis_count=$(dapr components -k -n pubsub | grep pubsub.redis | wc -l)
[[ $redis_count -gt 0 ]] && target_architecture="RabbitMQ" || target_architecture="Redis"

read -p "Have you changed the DAPR configuration to $target_architecture (y/n)? " choice

if [[ $choice =~ ^[Yy]$ ]]
then
    echo Reconfiguring...
else
    echo Exiting...
    exit
fi

if [[ 
    $redis_count -gt 0
]]; then
    echo Switching to RabbitMQ...
    kubectl apply -f ./k8s/rabbitmq-depl.yaml
    kubectl apply -f ./k8s/dapr-pubsub.yaml
    kubectl delete -f ./k8s/redis-depl.yaml
    sh $(pwd)/bin/restart.sh
else
    echo Switching to Redis...
    kubectl apply -f ./k8s/redis-depl.yaml
    kubectl apply -f ./k8s/dapr-pubsub.yaml
    kubectl delete -f ./k8s/rabbitmq-depl.yaml
    sh $(pwd)/bin/restart.sh
fi