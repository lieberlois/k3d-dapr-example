# Example microservice application with DAPR and Kubernetes using Rancher K3D

## Startup

Create a K3D Cluster

```sh
k3d registry create registry.localhost --port 5000
k3d cluster create -c k3d.yaml --registry-use k3d-registry.localhost:5000
```

Install Dapr

```sh
# Install Dapr CLI
wget -q https://raw.githubusercontent.com/dapr/cli/master/install/install.sh -O - | /bin/bash
# Initialize in K8s
dapr init -k
```

Add the local registry domain name to your local hosts file.

```sh
# /etc/hosts
127.0.0.1       k3d-registry.localhost
```

Now build the necessary docker images, tag them and deploy to the local registry.

```sh
# Build
docker build -t posts-service ./PostsService
docker build -t analytics-service ./AnalyticsService

# Tag
docker tag posts-service k3d-registry.localhost:5000/posts-service
docker tag analytics-service k3d-registry.localhost:5000/analytics-service

# Push
docker push k3d-registry.localhost:5000/posts-service
docker push k3d-registry.localhost:5000/analytics-service
```

Now run the application by creating all resources in K8s.

```sh
kubectl apply -f ./k8s
```

## Cleanup

To clean up all resources, use the following commands.

```sh
k3d cluster delete dapper-cluster
k3d registry delete registry.localhost
```