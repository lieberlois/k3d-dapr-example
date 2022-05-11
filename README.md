# Microservices with DAPR and Kubernetes using Rancher K3D

## Technologies

This section describes the used technologies for this project.

### Services

The gist of this demo application is in the creating of posts and the distributed analysis of these. The `PostsService` exposes a restful API with one GET- and one POST-route, that are responsible for listing and creating new posts. Once a post is created, an event is published onto the pubsub broker. The `AnalyticsService` listens to the topic where this event is created and analyzes the post data (for this demo application, it only performs a word count). After an artificial delay, the analyzed data is yet again published onto the pubsub broker and will later be stored in the PostgreSQL database, once the `PostsService` has reacted to the published event. Before the post is stored in the `PostgreSQL` instance of the `PostsService`, a URL has to be found for the resource. This URL can be queried by service invocation to the `UrlService`, which returns `http://www.example.com/lower-case-post-title`.

| Service          	| Technology            	|
|------------------	|-----------------------	|
| AnalyticsService 	| Golang w/ Gorilla Mux 	|
| PostsService      | ASP.NET Core 6        	|
| UrlService        | Node.js, TS, dapr-client  |

### Deployment Strategy

All of the listed services are being dockerized with their respective Dockerfiles and then deployed in a local Rancher K3D Kubernetes cluster. By using the `annotations`-section of the deployment manifests, Dapr then injects a sidecar container into each pod of the deployments, that handles all microservice-relevant communication (pubsub, state, etc.). For pubsub-communication, an instance of Redis / RabbitMQ is being used during development, which can easily be switched out by editing the `dapr-pubsub.yaml`-manifest. 

#### Azure Resources

To use `Azure Service Bus` for pubsub-communication, create a Kubernetes secret before startup:

```sh
# Replace the 'connectionString'-secret with the connection string to your Service Bus Namespace (use a Shared access policy for this)
# Note: the namespace has to be set to at least "Standard" pricing tier

$ kubectl create secret generic azure-service-bus --from-literal=connectionString="Endpoint=<...>"
```

Alternatively, you can use `Terraform` to automatically deploy the necessary Azure resources and store the necessary secrets in your local Kubernetes cluster. Note: the connection string is configured as an output within Terraform, so this configuration should NOT be used in production!

```sh
# Deploy infrastructure
$ az login
$ terraform apply

# Add connection string as a Kubernetes secret
$ kubectl create secret generic azure-service-bus --from-literal=connectionString="$(terraform output service_bus_conn)"
```

### Observabilty 

Since Dapr allows developers to observe each and every event, service invocation, etc. [Zipkin](http://localhost/zipkin) or [Jaeger](http://localhost/jaeger) can be used to follow traces within the cluster. Use the file ```dapr-tracing.yaml``` to decide between the two. Also, the Dapr Dashboard (you can start it with `dapr dashboard -k`), can be used to show health metrics and such for each Dapr Sidecar.

## Getting Started

Create a K3D Cluster

```sh
$ k3d registry create registry.localhost --port 5000
$ k3d cluster create -c k3d.yaml --registry-use k3d-registry.localhost:5000
```

Install Dapr in your K8s cluster

```sh
# Install Dapr CLI
$ wget -q https://raw.githubusercontent.com/dapr/cli/master/install/install.sh -O - | /bin/bash
# Initialize in K8s
$ dapr init -k
```

Add the local registry domain name `k3d-registry.localhost` to your local hosts file.

```sh
# /etc/hosts
$ sudo sh -c 'echo "127.0.0.1       k3d-registry.localhost" >> /etc/hosts'
```

Now build the necessary docker images, tag them, deploy to the local registry and apply the manifests to K8s.
Note that this command might take a while (several minutes) when executed for the first time.

```sh
# Build
$ ./bin/redeploy.sh
```

## Development

For development purposes, there are some utility scripts in the ./bin folder.

* ```restart.sh``` will restart the deployments that run the implemented services.
* ```redeploy.sh``` will rebuild and push the images and redeploy the containers.
* ```reconfigure.sh``` will seamlessly switch out Redis for RabbitMQ (or vice versa).

## Cleanup

To clean up all resources, use the following commands.

```sh
# Kubernetes
$ kubectl delete all --all
$ k3d cluster delete dapper-cluster
$ k3d registry delete registry.localhost

# Terraform (only if used)
$ terraform destroy
```