# Example microservice application with DAPR and Kubernetes using Rancher K3D

## Startup

Create a K3D Cluster

```sh
k3d registry create registry.localhost --port 5000
k3d cluster create -c k3d.yaml --registry-use k3d-registry.localhost:5000
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

# Tag
docker tag posts-service k3d-registry.localhost:5000/posts-service

# Push
docker push k3d-registry.localhost:5000/posts-service
```

Now run the application by creating all resources in K8s.

```sh
kubectl apply -f ./k8s
```

