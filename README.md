# Example microservice application with DAPR and K3D

## Startup

Create a K3D Cluster

```sh
k3d cluster create -c k3d.yaml
```

Now build the necessary docker images.

```sh
docker build -t posts-service ./PostsService
```






