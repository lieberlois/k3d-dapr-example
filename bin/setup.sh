# Setup resolv.conf entry
sudo sh -c 'echo "127.0.0.1       k3d-registry.localhost" >> /etc/hosts'

# Initialize k3d
k3d registry create registry.localhost --port 5000
k3d cluster create -c k3d.yaml --registry-use k3d-registry.localhost:5000

# Initialize in K8s
dapr init -k

# Build Images
sh $(pwd)/bin/redeploy.sh

